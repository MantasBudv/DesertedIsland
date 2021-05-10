using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Transactions;
using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class BombNPCController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 2.2f;

    [SerializeField] private Animator anim;

    [SerializeField] GameObject poofPrefab;
    
    private Rigidbody2D _player;
    private Rigidbody2D _rb;
    private float _distanceToPlayer;
    private float _rotationRads;
    private Vector2 _positionOffset;
    
    private CurrentAction _action;

    private float _idleTimerStart;
    private float _idleTimerEnd;

    private float _walkTimerStart;
    private float _walkTimerEnd;

    private bool _facingRight;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _action = CurrentAction.Idle;
        _facingRight = false;
    }       
    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    
    
    private void CommitDying()      
    {
        Instantiate(poofPrefab, gameObject.transform.position, Quaternion.identity);
        _action = CurrentAction.Dying;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void CommitIdleWalking()
    {
        _action = CurrentAction.IdleWalking;
        anim.SetBool("IsWalking", true);
        _walkTimerStart = 0f;
        _walkTimerEnd = Random.Range(1.5f, 3.0f);
        _rotationRads = Random.Range(0, 2 * Mathf.PI);
    }

    private void CommitIdle()
    {
        _action = CurrentAction.Idle;
        anim.SetBool("IsWalking", false);
        _idleTimerStart = 0f;
        _idleTimerEnd = Random.Range(1.5f, 3.0f);
    }

    private void FixedUpdate()
    {
        UpdateDistanceToPlayer();
        switch (_action)
        {
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
        UpdatePosition();
        if (_walkTimerStart >= _walkTimerEnd)
        {
            CommitIdle();
            return;
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

        if (_idleTimerStart >= _idleTimerEnd)
        {
            CommitIdleWalking();
            return;
        }
    }
    

    private void UpdateDistanceToPlayer()
    {
        var playerPosition = _player.position;
        var npcPosition = _rb.position;
        _positionOffset = npcPosition - playerPosition;
        _distanceToPlayer = Vector2.Distance(npcPosition, playerPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rotationRads = +Mathf.PI;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            CommitDying();
            return;
        }
        if (other.CompareTag("Throwable"))
        {
            Destroy(other.gameObject);
            if (_action != CurrentAction.Dying)
            {
                CommitDying();
            }

            return;
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
        IdleWalking,
        Dying
    }
}
