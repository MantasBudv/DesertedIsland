using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public float BASE_MOVE_SPEED = 3.5f;
    public Rigidbody2D rb;
    public float movementSpeed;
    public Animator animator;
    private Vector2 moveDir;
    public GameObject gameOver;


    //Health
    public int maxHealth;
    private static int currentHealth;
    public HealthBar healthBar;

    //Stamina
    public int maxStamina;
    private static int currentStamina;
    public StaminaBar staminaBar;
    public float staminaRegenTime;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
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
        Animate();
    }
    void FixedUpdate() 
    {
        Move();
    }

    void GetInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
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
       
        animator.SetFloat("Speed", movementSpeed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Skeleton")
        {
            //Time.timeScale = 0f;
            //gameObject.SetActive(false);
            //Debug.Log("You died");
            //gameOver.SetActive(true);
            TakeDamage(1);
        }
    }

    //Health


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

    public void LoadStats(int maxHP, int currHP, int maxSTA, int currSTA)
    {
        maxHealth = maxHP;
        currentHealth = currHP;

        maxStamina = maxSTA;
        currentStamina = currSTA;

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
            staminaBar.SetStamina(++currentStamina);
        }
    }

    public int getCurrSTA()
    {
        return currentStamina;
    }


}
