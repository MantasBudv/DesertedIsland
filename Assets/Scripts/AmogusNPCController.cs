using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Transactions;
using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class AmogusNPCController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 2.2f;

    [SerializeField] private float attackRange = 10.5f;

    [SerializeField] private float retreatRange = 2.5f;
    
    [SerializeField] private Animator anim;
    
    [SerializeField] private  GameObject throws;
    
    private Rigidbody2D _player;
    private Rigidbody2D _rb;
    private float _distanceToPlayer;
    private float _rotationRads;
    private Vector2 _positionOffset;
    
    private CurrentAction _action;
    
    private float _dyingTimer;

    private float _idleTimerStart;
    private float _idleTimerEnd;

    private float _attackTimer;

    private float _walkTimerStart;
    private float _walkTimerEnd;

    private bool _facingRight;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _action = CurrentAction.Idle;
        _dyingTimer = 0;
        _facingRight = true;
    }       
    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    
    
    private bool IsRetreatRange()
    {
        return _distanceToPlayer < retreatRange;
    }

    private bool IsAttackRange()
    {
        return _distanceToPlayer < attackRange;
    }
    
    private void CommitDying()      
    {
        _action = CurrentAction.Dying;
        anim.SetBool("isDying", true);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }

    private void CommitIdleWalking()
    {
        _action = CurrentAction.IdleWalking;
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", true);
        _walkTimerStart = 0f;
        _walkTimerEnd = Random.Range(1.5f, 3.0f);
        _rotationRads = Random.Range(0, Mathf.PI);
    }

    private void CommitIdle()
    {
        _action = CurrentAction.Idle;
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", false);
        _idleTimerStart = 0f;
        _idleTimerEnd = Random.Range(1.5f, 3.0f);
    }

    private void CommitAttacking()
    {
        _action = CurrentAction.Attacking;
        anim.SetBool("isAttacking", true);
        anim.SetBool("isWalking", false);
        _attackTimer = 0f;
    }
    
    private void Shoot()
    {
        _rotationRads = Mathf.Atan2(_positionOffset.y, _positionOffset.x) + Mathf.PI;
        GameObject projectile = Instantiate(throws, transform.position,
            Quaternion.Euler(
                Mathf.Cos(_rotationRads),
                Mathf.Sin(_rotationRads),
                0
                )
            );
        Rigidbody2D rockrb = projectile.GetComponent<Rigidbody2D>();
        rockrb.AddForce(_positionOffset * -4f, ForceMode2D.Impulse);
    }

    private void CommitDodgeWalking()
    {
        _action = CurrentAction.DodgeWalking;
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWalking", true);
        _walkTimerStart = 0f;
        _walkTimerEnd = Random.Range(1.5f, 3.0f);
        _rotationRads = Random.Range(0, Mathf.PI);
    }
    private void FixedUpdate()
    {
        UpdateDistanceToPlayer();
        switch (_action)
        {
            case CurrentAction.Dying:
                ExecuteDeath();
                break;
            case CurrentAction.Attacking:
                ExecuteAttack();
                break;
            case CurrentAction.DodgeWalking:
                ExecuteDodgeWalking();
                break;
            case CurrentAction.Idle:
                Idle();
                break;
            case CurrentAction.IdleWalking:
                ExecuteIdleWalking();
                break;
        }
    }
    
    private void ExecuteIdleWalking()
    {
        _walkTimerStart += Time.deltaTime;
        if (IsRetreatRange())
        {
            CommitDying();
            return;
        }

        if (IsAttackRange())
        {
            CommitAttacking();
            return;
        }

        if (_walkTimerStart >= _walkTimerEnd)
        {
            CommitIdle();
            return;
        }
        else
        {
            
            UpdatePosition();
        }
    }

    private void ExecuteDodgeWalking()
    {
        _walkTimerStart += Time.deltaTime;
        if (IsRetreatRange())
        {
            CommitDying();
            return;
        }
        if (_walkTimerStart >= _walkTimerEnd)
        {
            if (IsAttackRange())
            {
                CommitAttacking();
            }
            else
            {
                CommitIdle();
            }
        }
        else
        {
            UpdatePosition();
        }
    }
    
    private void UpdatePosition()
    {
        Vector2 positionMovement = new Vector2(
            Mathf.Cos(_rotationRads),
            Mathf.Sin(_rotationRads)
        );
        
        if (positionMovement.x > 0 && !_facingRight)
        {
            Flip();
        }
        else if (positionMovement.x < 0 && _facingRight)
        {
            Flip();
        }
        
        _rb.velocity = positionMovement * moveSpeed;
        
    }

    private void Idle()
    {
        _rb.velocity = new Vector2(0.0f, 0.0f);
        _idleTimerStart += Time.deltaTime;
        if (IsRetreatRange())
        {
            CommitDying();
            return;
        }

        if (IsAttackRange())
        {
            CommitAttacking();
            return;
        }
        
        if (_idleTimerStart >= _idleTimerEnd)
        {
            CommitIdleWalking();
            return;
        }
    }

    private void ExecuteAttack()
    {
        _rb.velocity = new Vector2(0.0f, 0.0f);
        _attackTimer += Time.deltaTime;
        
        if (_positionOffset.x < 0 && !_facingRight)
        {
            Flip();
        }
        else if (_positionOffset.x > 0 && _facingRight)
        {
            Flip();
        }
        
        if (IsRetreatRange())
        {
            CommitDying();
        }
        if (_attackTimer >= 0.98f)
        {
            Shoot();
            if (IsAttackRange())
            {
                CommitDodgeWalking();
            }
            else
            {
                CommitIdle();
            }
        }
    }
    
    private void ExecuteDeath()
    {
        if (_positionOffset.x < 0 && !_facingRight)
        {
            Flip();
        }
        else if (_positionOffset.x > 0 && _facingRight)
        {
            Flip();
        }
        _rb.velocity = new Vector2(0.0f, 0.0f);
        _dyingTimer += Time.deltaTime;
        if (_dyingTimer >= 0.98f)
        {
            var Character = GameObject.FindGameObjectWithTag("Player");
            Character.GetComponent<CharacterController>().GiveXP(400);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void UpdateDistanceToPlayer()
    {
        var playerPosition = _player.position;
        var npcPosition = _rb.position;
        _positionOffset = npcPosition - playerPosition;
        _distanceToPlayer = Vector2.Distance(npcPosition, playerPosition);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Throwable"))
        {
            Destroy(other.gameObject);
            if (_action != CurrentAction.Dying)
            {
                CommitDying();
            }
        }
    }
    
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _facingRight = !_facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private enum CurrentAction
    {
        Idle,
        Attacking,
        IdleWalking,
        DodgeWalking,
        Dying
    }
}
