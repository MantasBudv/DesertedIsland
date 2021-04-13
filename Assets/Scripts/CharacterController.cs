using System;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [Header("BasicInfo")]
    public Rigidbody2D rb;
    public Camera camera;
    public Transform test;
    public Animator animator;
    private Vector2 moveDir;
    public GameObject gameOver;
    [Space(10)]

    [Header("Movement")]
    public float movementSpeed;
    public float BASE_MOVE_SPEED = 3.5f;
    [Space(10)]

    [Header("Health")]
    public static AcquiredSkills skills;
    public int maxHealth;
    private static int currentHealth;
    public HealthBar healthBar;
    [Space(10)]

    [Header("Stamina")]
    public int maxStamina;
    private static int currentStamina;
    public StaminaBar staminaBar;
    public float staminaRegenTime;
    [Space(10)]

    [Header("Attack")]
    public GameObject sword;
    public GameObject rock;
    private bool isAttacking;
    private bool meleeAttack;
    private bool rangedAttack;
    private float attackTime = 0.25f;
    private float attackRate = 0.25f;
    private float rangedRate = 0.3f;
    private float nextFire = 0f;


    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        camera = Camera.main;
        if (skills == null)
            skills = new AcquiredSkills();
        if (UIButtons.newgame)
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            UIButtons.newgame = false;
        }
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetStamina(currentStamina);
        InvokeRepeating("RegenStamina", staminaRegenTime, staminaRegenTime);
    }

    void Update()
    {
        GetInputs();
        Move();
        CheckRanged();
        Animate();

        if (isAttacking)
        {
            attackTime -= Time.deltaTime;
            if (attackTime < 0f)
            {
                animator.SetBool("Attacking", false);
            }
        }
        
    }
    void FixedUpdate() 
    {
        
    }

    void GetInputs()
    {
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        meleeAttack = Input.GetButtonDown("Fire1");
        rangedAttack = Input.GetButtonDown("Fire2");
        bool runButton = Input.GetButton("Run");
        
        moveDir = new Vector2(moveX, moveY);
        if (runButton && currentStamina > 0 && moveDir != Vector2.zero)
        {
            movementSpeed = 2 * Mathf.Clamp(moveDir.magnitude, 0.0f, 1.0f);
            ReduceStamina();
        }
        else
        {
            movementSpeed = Mathf.Clamp(moveDir.magnitude, 0.0f, 1.0f);
        }
        moveDir.Normalize();
    }

    void Move()
    {
        rb.velocity = moveDir * movementSpeed * BASE_MOVE_SPEED;
    }

    void Animate()
    {
        if (moveDir != Vector2.zero)
        {
            animator.SetFloat("Horizontal", moveDir.x);
            animator.SetFloat("Vertical", moveDir.y);
        }
        if (meleeAttack && sword.activeInHierarchy)
        {
            isAttacking = true;
            attackTime = attackRate;
            animator.SetBool("Attacking", true);
        }

        animator.SetFloat("Speed", movementSpeed);
    }

    void CheckRanged()
    {
        if (rangedAttack && Time.time > nextFire)
        {
            nextFire = Time.time + rangedRate;
            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            moveDir = mousePosition - rb.position;
            moveDir.Normalize();
            Shoot();
        }
    }
    //Health
    void Shoot()
    {
        GameObject projectile = Instantiate(rock, test);
        Rigidbody2D rockrb = projectile.GetComponent<Rigidbody2D>();
        rockrb.AddForce(moveDir * 6f, ForceMode2D.Impulse);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth == 0)
        {
            Time.timeScale = 0f;
            gameObject.SetActive(false);
            Debug.Log("You died");
            gameOver.SetActive(true);
        }
        
    }


    public int getCurrHP()
    {
        return currentHealth;
    }

    public void LoadStats(int maxHP, int currHP, int maxSTA, int currSTA, bool[] currSkills)
    {
        maxHealth = maxHP;
        currentHealth = currHP;

        maxStamina = maxSTA;
        currentStamina = currSTA;
        skills = new AcquiredSkills(currSkills);
        //Debug.Log(skills.HasAcquired(AcquiredSkills.SkillEnum.StaminaRegen));
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetStamina(currentStamina);
    }

    //Stamina

    void ReduceStamina()
    {
        currentStamina--;
        staminaBar.SetStamina(currentStamina);
    }

    void RegenStamina()
    {
        if (currentStamina != maxStamina)
        {
            if (skills.HasAcquired(AcquiredSkills.SkillEnum.StaminaRegen))
            {
                currentStamina += 10;
            }
            staminaBar.SetStamina(++currentStamina);
        }
    }

    public int getCurrSTA()
    {
        return currentStamina;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Skeleton")
        {
            //Time.timeScale = 0f;
            //gameObject.SetActive(false);
            //Debug.Log("You died");
            //gameOver.SetActive(true);
            TakeDamage(1);
        }
    }

    public class AcquiredSkills
    {
        static int size = 1;
        public bool[] _hasAcquired;
        public AcquiredSkills()
        {
            _hasAcquired = new bool[size];
        }

        public AcquiredSkills(bool[] acquired)
        {
            _hasAcquired = new bool[size];
            for (int i = 0; i < size; i++)
            {
                _hasAcquired[i] = acquired[i];
            }
        }
        public enum SkillEnum
        {
            StaminaRegen
        }

        public void SetSkill(SkillEnum skill)
        {
            switch (skill)
            {
                case SkillEnum.StaminaRegen:
                    _hasAcquired[0] = true;
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
        }

        public bool HasAcquired(SkillEnum skill)
        {
            switch (skill)
            {
                case SkillEnum.StaminaRegen:
                    return _hasAcquired[0];
                default:
                    Debug.Log("Error");
                    return false;
            }
        }

        public bool[] Copy()
        {
            bool[] copy = new bool[size];
            for (int i = 0; i < size; i++)
            {
                copy[i] = _hasAcquired[i];
            }
            return copy;
        }

    }
}

