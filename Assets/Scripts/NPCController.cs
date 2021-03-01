using System;
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

    [SerializeField] private Vector2 minRoamingCoords = new Vector2(-10, 5);
    
    [SerializeField] private Vector2 maxRoamingCoords = new Vector2(10, -5);
    
    [SerializeField] private float moveSpeed = 0.05f;

    [SerializeField] private float concernedRange = 1.5f;

    [SerializeField] private float scaredRange = 0.5f;

    [SerializeField] private Rigidbody2D player;

    private Rigidbody2D _rb;
    private float _rotationRads;
    private Vector2 _positionOffset;
    private float _distanceToPlayer;
    private NpcState _state;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _state = new NpcState();
    }

    private void FixedUpdate()
    {
        UpdateDistanceToPlayer();
        UpdateState();
        UpdateRotation();
        UpdatePosition();
    }

    private void UpdateRotation()
    {

        if (_state.ShouldRotate() == false)
        {
            return;
        }
        
        _rotationRads = _state.GetRotation(_positionOffset) + UnityEngine.Random.Range(0.0f, Mathf.PI / 4);
    }

    private void UpdatePosition()
    {
        if (_state.IsStanding())
        {
            return;
        }
        
        
        Vector2 positionMovement = new Vector2(
            Mathf.Cos(_rotationRads) * moveSpeed,
            Mathf.Sin(_rotationRads) * moveSpeed
        );

        Vector2 currentPosition = _rb.position;
        var newPosition = currentPosition + positionMovement;
        
        if (IsCollision(newPosition))
        {
            while (IsCollision(newPosition))
            {
                _rotationRads += UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                positionMovement = new Vector2(
                    Mathf.Cos(_rotationRads) * moveSpeed,
                    Mathf.Sin(_rotationRads) * moveSpeed
                );
                newPosition = currentPosition + 60 * positionMovement;
            }

            _state.AssignBumped();
            return;
        }

        _rb.MovePosition(newPosition);
    }

    private bool IsCollision(Vector2 position)
    {
        return position.x < minRoamingCoords.x || position.y > minRoamingCoords.y
                                               || position.x > maxRoamingCoords.x
                                               || position.y < maxRoamingCoords.y;
    }

    private void UpdateDistanceToPlayer()
    {
        var playerPosition = player.position;
        var npcPosition = _rb.position;
        _positionOffset = npcPosition - playerPosition;
        _distanceToPlayer = Vector2.Distance(npcPosition, playerPosition);
        //distanceToPlayer = Vector2.Distance(rb.transform.position, player.transform.position);
    }
    private void UpdateState()
    {
        _state.Update(_distanceToPlayer, scaredRange, concernedRange);
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

        public bool IsStanding()
        {
            return Action == CurrentAction.BumpedStanding || Action == CurrentAction.IdleStanding;
        }
        
        public bool IsScared(float distanceToPlayer, float scaredRange)
        {
            return distanceToPlayer < scaredRange;
        }

        public bool IsConcerned(float distanceToPlayer, float concernedRange)
        {
            return distanceToPlayer < concernedRange;
        }

        public void AssignBumped()
        {
            Action = CurrentAction.BumpedStanding;
            AssignStateTimer();
            AssignMovementTimer();
        }
        
        public void Update(float distanceToPlayer, float scaredRange, float concernedRange)
        {
            var currentTime = Time.fixedTime;
            if (_state.TimeEnd <= currentTime) // Update State and Rotation
            {
                if (Action == CurrentAction.BumpedStanding)
                {
                    Action = CurrentAction.BumpedWalking;
                }
                
                else if (IsScared(distanceToPlayer, scaredRange))
                {
                    Action = CurrentAction.Scared;
                }
                
                else if (IsConcerned(distanceToPlayer, concernedRange))
                {
                    Action = CurrentAction.Concerned;
                }
                
                else if (Action == CurrentAction.IdleStanding)
                {
                    Action = CurrentAction.IdleWalking;
                }
                else
                {
                    Action = CurrentAction.IdleStanding;
                }
                
                AssignStateTimer();
                AssignMovementTimer();
                
            }
            else if (_movement.TimeEnd <= currentTime) //Update Rotation
            {
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
                    _state.TimeWindow = 2.0f;
                    _state.TimeEnd = _state.TimeStart + _state.TimeWindow;
                    break;

                case CurrentAction.Concerned:
                    _state.TimeWindow = 1.0f;
                    _state.TimeEnd = _state.TimeStart + _state.TimeWindow;
                    break;

                case CurrentAction.Scared:
                    _state.TimeWindow = 1.0f;
                    _state.TimeEnd = _state.TimeStart + _state.TimeWindow;
                    break;
                
                case CurrentAction.BumpedStanding:
                    _state.TimeWindow = 1.0f;
                    _state.TimeEnd = _state.TimeStart + _state.TimeWindow;
                    break;
                
                case CurrentAction.BumpedWalking:
                    _state.TimeWindow = 1.0f;
                    _state.TimeEnd = _state.TimeStart + _state.TimeWindow;
                    break;
                
                default:
                    throw new NotImplementedException();
            }
        }

        private void AssignMovementTimer()
        {
            _movement.TimeStart = Time.fixedTime;
            switch (Action)
            {
                case CurrentAction.IdleStanding:
                    _movement.TimeWindow = _state.TimeWindow;
                    _movement.TimeEnd = _movement.TimeStart + _movement.TimeWindow;
                    break;

                case CurrentAction.IdleWalking:
                    _movement.TimeWindow = _state.TimeWindow;
                    _movement.TimeEnd = _movement.TimeStart + _movement.TimeWindow;
                    _movement.IsRotated = false;
                    break;

                case CurrentAction.Concerned:
                    _movement.TimeWindow = _state.TimeWindow;
                    _movement.TimeEnd = _movement.TimeStart + _movement.TimeWindow;
                    _movement.IsRotated = false;
                    break;

                case CurrentAction.Scared:
                    _movement.TimeWindow = _state.TimeWindow;
                    _movement.TimeEnd = _movement.TimeStart + _movement.TimeWindow;
                    _movement.IsRotated = false;
                    break;
                
                case CurrentAction.BumpedStanding:
                    _movement.TimeWindow = _state.TimeWindow;
                    _movement.TimeEnd = _movement.TimeStart + _movement.TimeWindow;
                    break;
                
                case CurrentAction.BumpedWalking:
                    _movement.TimeWindow = _state.TimeWindow;
                    _movement.TimeEnd = _movement.TimeStart + _movement.TimeWindow;
                    break;
                
                default:
                    throw new NotImplementedException();
            }

            
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
                
                case CurrentAction.BumpedWalking:
                    throw new NotImplementedException();
                
                case CurrentAction.BumpedStanding:
                    throw new NotImplementedException();
                
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
        BumpedStanding,
        BumpedWalking,
    }
}

