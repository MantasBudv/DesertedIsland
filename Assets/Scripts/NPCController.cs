﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCController : MonoBehaviour
{

    [SerializeField] private float moveSpeedRegular = 2.2f;

    [SerializeField] private float moveSpeedScared = 3f;

    [SerializeField] private float concernedRange = 1.5f;

    [SerializeField] private float scaredRange = 0.5f;

    [SerializeField] private GameObject itemDrop;

    [SerializeField] private Animator anim;
    
    private Rigidbody2D _player;
    private Rigidbody2D _rb;
    private float _rotationRads;
    private Vector2 _positionOffset;
    private float _distanceToPlayer;
    private NpcState _state;
    
    private float dyingEnd;
    private bool _facingRight;
    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _state = new NpcState();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (_state.Action != CurrentAction.Dying)
        {
            UpdateDistanceToPlayer();
            UpdateState();
            UpdateRotation();
            UpdatePosition();
        }
        else
        {
            if (Time.fixedTime < dyingEnd) return;
            _player.GetComponent<CharacterController>().GiveXP(160);
            Instantiate(itemDrop, _rb.position, Quaternion.identity);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void UpdateRotation()
    {
        if (_state.ShouldRotate() == false)
        {
            return;
        }
        
        _rotationRads = _state.GetRotation(_positionOffset) + UnityEngine.Random.Range(-Mathf.PI / 16, Mathf.PI / 16);
    }

    private void UpdatePosition()
    {
        if (_state.Action == CurrentAction.IdleStanding)
        {
            anim.SetBool("IsMoving", false);
            _rb.velocity = new Vector2(0.0f, 0.0f);
            return;
        }

        anim.SetBool("IsMoving", true);
        var movementSpeed = (_state.Action == CurrentAction.Scared) ? moveSpeedScared : moveSpeedRegular;
        
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
        _rb.velocity = positionMovement * movementSpeed;
    }

    private void UpdateDistanceToPlayer()
    {
        var playerPosition = _player.position;
        var npcPosition = _rb.position;
        _positionOffset = npcPosition - playerPosition;
        _distanceToPlayer = Vector2.Distance(npcPosition, playerPosition);
        
    }
    private void UpdateState()
    {
        _state.Update(_distanceToPlayer, scaredRange, concernedRange);
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

    private void commitDying()
    {
        if (_state.Action == CurrentAction.Dying)
        {
            return;
        }
        _state.Action = CurrentAction.Dying;
        anim.SetBool("IsDying", true);
        dyingEnd = Time.fixedTime + 1.0f;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        _rb.velocity = new Vector2(0.0f, 0.0f);
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

    private class NpcState
    {
        public CurrentAction Action;
        private StateTimer _state;
        private MovementTimer _movement;
        
        public NpcState()
        {
            Action = CurrentAction.IdleStanding;
            _state = new StateTimer();
            _movement = new MovementTimer();
        }

        public bool IsScared(float distanceToPlayer, float scaredRange)
        {
            return distanceToPlayer < scaredRange;
        }

        public bool IsConcerned(float distanceToPlayer, float concernedRange)
        {
            return distanceToPlayer < concernedRange;
        }

        public void Update(float distanceToPlayer, float scaredRange, float concernedRange)
        {

            var currentTime = Time.fixedTime;
            if (_movement.TimeEnd <= currentTime) //Update Rotation
            {
                if (IsScared(distanceToPlayer, scaredRange))
                {
                    Action = CurrentAction.Scared;
                    AssignStateTimer();
                    AssignMovementTimer();
                }
                
                else if (IsConcerned(distanceToPlayer, concernedRange))
                {
                    Action = CurrentAction.Concerned;
                    AssignStateTimer();
                    AssignMovementTimer();
                }
            }
            if (_state.TimeEnd <= currentTime) // Update State and Rotation
            {
                if (Action == CurrentAction.IdleWalking)
                {
                    Action = CurrentAction.IdleStanding;
                }
                else
                {
                    Action = CurrentAction.IdleWalking;
                }
                
                AssignStateTimer();
                AssignMovementTimer();
                
            }
        }

        private void AssignStateTimer()
        {
            _state.TimeStart = Time.fixedTime;
            switch (Action)
            {
                case CurrentAction.IdleStanding:
                    _state.TimeWindow = 2.0f;
                    _state.TimeEnd = _state.TimeStart + _state.TimeWindow;
                    break;

                case CurrentAction.IdleWalking:
                    _state.TimeWindow = 1.0f;
                    _state.TimeEnd = _state.TimeStart + _state.TimeWindow;
                    _movement.IsRotated = false;
                    break;

                case CurrentAction.Concerned:
                    _state.TimeWindow = 1.0f;
                    _state.TimeEnd = _state.TimeStart + _state.TimeWindow;
                    _movement.IsRotated = false;
                    break;

                case CurrentAction.Scared:
                    _state.TimeWindow = 2.0f;
                    _state.TimeEnd = _state.TimeStart + _state.TimeWindow;
                    _movement.IsRotated = false;
                    break;
                
                default:
                    throw new NotImplementedException();
            }
        }

        private void AssignMovementTimer()
        {
            _movement.TimeStart = Time.fixedTime;
            _movement.TimeWindow = _state.TimeWindow / 8;
            _movement.TimeEnd = _movement.TimeStart + _movement.TimeWindow;
            /*switch (Action)
            {
                case CurrentAction.IdleStanding:
                    _movement.TimeWindow = _state.TimeWindow / 8;
                    _movement.TimeEnd = _movement.TimeStart + _movement.TimeWindow;
                    break;

                case CurrentAction.IdleWalking:
                    _movement.TimeWindow = _state.TimeWindow / 8;
                    _movement.TimeEnd = _movement.TimeStart + _movement.TimeWindow;
                    _movement.IsRotated = false;
                    break;

                case CurrentAction.Concerned:
                    _movement.TimeWindow = _state.TimeWindow / 8;
                    _movement.TimeEnd = _movement.TimeStart + _movement.TimeWindow;
                    _movement.IsRotated = false;
                    break;

                case CurrentAction.Scared:
                    _movement.TimeWindow = _state.TimeWindow / 8;
                    _movement.TimeEnd = _movement.TimeStart + _movement.TimeWindow;
                    _movement.IsRotated = false;
                    break;

                default:
                    throw new NotImplementedException();
            }*/
        }
        
        public bool ShouldRotate()
        {
            if (_movement.IsRotated == false)
            {
                _movement.IsRotated = true;
                return true;
            }
            
            return false;
        }

        public float GetRotation(Vector2 positionOffset)
        {
            switch (Action)
            {
                case CurrentAction.IdleStanding:
                    throw new NotImplementedException();

                case CurrentAction.IdleWalking:
                    return Random.Range(0.0f, 2 * Mathf.PI);

                case CurrentAction.Concerned:
                    return Mathf.Atan2(positionOffset.y, positionOffset.x);

                case CurrentAction.Scared:
                    return Mathf.Atan2(positionOffset.y, positionOffset.x);

                default:
                    throw new NotImplementedException();
                    
            }
            
        }
    }

    private class StateTimer
    {
        public float TimeStart;
        public float TimeWindow;
        public float TimeEnd;

        public StateTimer()
        {
            TimeStart = 0f;
            TimeWindow = 0f;
            TimeEnd = 0f;
        }
    }

    private class MovementTimer
    {
        public float TimeStart;
        public float TimeWindow;
        public float TimeEnd;

        public bool IsRotated;
        
        public MovementTimer()
        {
            TimeStart = 0f;
            TimeWindow = 0f;
            TimeEnd = 0f;

            IsRotated = true;
        }
    }

    private enum CurrentAction
    {
        IdleStanding,
        IdleWalking,
        Concerned,
        Scared,
        Dying
    }
}