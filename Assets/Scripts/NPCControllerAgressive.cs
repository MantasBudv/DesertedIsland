using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCControllerAgressive : MonoBehaviour
{

    [SerializeField] private float moveSpeedAttacking = 2.2f;

    [SerializeField] private float moveSpeedAgressive = 1.5f;

    //[SerializeField] private float concernedRange = 1.5f;

    [SerializeField] private float agressiveRange = 10f;
    
    [SerializeField] private float attackRange = 0.9f;

    [SerializeField] private GameObject aggresiveNpc;

    [SerializeField] private Animator anim;
    
    private Rigidbody2D _player;
    private Rigidbody2D _rb;
    private float _rotationRads;
    private Vector2 _positionOffset;
    private float _distanceToPlayer;

    private CurrentAction _action;
    private bool _shouldRotate;

    private float duplicateTimer;
    private float dyingTimer;
    private float attackTimer;
    private float attackCooldown;
    
    public static int SplitCount = 7;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _action = CurrentAction.Agressive;
        duplicateTimer = 0.0f;
        dyingTimer = 0.0f;
        attackTimer = 0.0f;
        attackCooldown = 0.0f;
    }
    private void UpdateRotation()
    {
        if (_action == CurrentAction.Idle)
        {
            return;
        }
        
        _rotationRads = Mathf.Atan2(_positionOffset.y, _positionOffset.x) + Mathf.PI;
    }
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (_action == CurrentAction.Dying)
        {
            executeDeath();
            return;
        }
        UpdateDistanceToPlayer();
        UpdateState();
        IsDuplicate();
        UpdateRotation();
        UpdatePosition();
    }
    
    private void UpdatePosition()
    {
        if (_action == CurrentAction.Idle)
        {
            _rb.velocity = new Vector2(0.0f, 0.0f);
            anim.SetBool("IsWalking", false);
            return;
        }

        float movementSpeed = 0;
        if (_action == CurrentAction.Agressive)
        {
            movementSpeed = moveSpeedAgressive;
        }

        if (_action == CurrentAction.Attacking)
        {
            movementSpeed = moveSpeedAttacking;
        }
        //var movementSpeed = (_action == CurrentAction.Agressive) ? moveSpeedAgressive : moveSpeedRegular;
        
        
        anim.SetBool("IsWalking", true);
        
        Vector2 positionMovement = new Vector2(
            Mathf.Cos(_rotationRads),
            Mathf.Sin(_rotationRads)
        );
        _rb.velocity = positionMovement * movementSpeed;
    }

    private void UpdateDistanceToPlayer()
    {
        var playerPosition = _player.position;
        var npcPosition = _rb.position;
        _positionOffset = npcPosition - playerPosition;
        _distanceToPlayer = Vector2.Distance(npcPosition, playerPosition);
        
    }

    private void IsDuplicate()
    {
        var currentTime = Time.fixedTime;
        if (duplicateTimer >= 2.0f)
        {
            Duplicate();
            duplicateTimer = 0.0f;
        }
    }

    private void executeDeath()
    {
        _rb.velocity = new Vector2(0.0f, 0.0f);
        dyingTimer += Time.deltaTime;
        if (dyingTimer >= 0.6f)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void Duplicate()
    {
        
        if (SplitCount > 0)
        {
            SplitCount--;
            var pos = _rb.position;
            pos.y += Random.Range(-2f, 2f);
            pos.x += Random.Range(-2f, 2f);
        
            Instantiate(aggresiveNpc, pos, Quaternion.identity);
        }
    }
    
    public bool IsAgressive()
    {
        return _distanceToPlayer < agressiveRange;
    }

    public bool isInAttackRange()
    {
        return _distanceToPlayer < attackRange;
    }
    
    public void UpdateState()
    {
        if (attackTimer >= 0.5f)
        {
            attackCooldown = 2.3f;
        }
        if (isInAttackRange() && attackTimer == 0f && attackCooldown <= 0.0f ||
            _action == CurrentAction.Attacking && attackTimer <= 0.7f)
        {
            _action = CurrentAction.Attacking;
            anim.SetBool("IsAttacking", true);
            attackTimer += Time.deltaTime;
            duplicateTimer = 0.0f;
        }
        else if (IsAgressive())
        {
            _action = CurrentAction.Agressive;
            anim.SetBool("IsAttacking", false);
            duplicateTimer += Time.deltaTime;
            attackTimer = 0.0f;
        }
        else
        {
            _action = CurrentAction.Idle;
            anim.SetBool("IsAttacking", false);
            duplicateTimer = 0.0f;
            attackTimer = 0.0f;
        }

        attackCooldown -= Time.deltaTime;
    }

    private void commitDying()
    {
        _action = CurrentAction.Dying;
        anim.SetBool("IsDying", true);
        aggresiveNpc.GetComponent<CapsuleCollider2D>().enabled = false;
    }
    private enum CurrentAction
    {
        Idle,
        Agressive,
        Attacking,
        Dying
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            commitDying();
        }
        if (other.CompareTag("Throwable"))
        {
            Destroy(other.gameObject);
            commitDying();
        }
    }
}
