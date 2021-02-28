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
    
    [SerializeField] private float moveSpeed = 2.5f;

    [SerializeField] private float concernedRange = 5f;

    [SerializeField] private float scaredRange = 0f;

    [SerializeField] private Rigidbody2D player;

    private Rigidbody2D rb;
    private float rotationRads;
    private Vector2 positionOffset;
    private float distanceToPlayer;
    private NpcState state;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = new NpcState();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (state.ShouldRotate() == false)
        {
            return;
        }

        rotationRads = state.GetRotation(positionOffset);
    }

    private void UpdatePosition()
    {
        if (state.Action == CurrentAction.IdleStanding) return;
        
        Vector2 positionMovement = new Vector2(
            Mathf.Cos(rotationRads) * moveSpeed,
            Mathf.Sin(rotationRads) * moveSpeed
        );
        
        Vector2 currentPosition = rb.position;
        var newPosition = currentPosition + positionMovement;
        
        rb.MovePosition(newPosition);
    }

    private void UpdateDistanceToPlayer()
    {
        var playerPosition = player.position;
        var npcPosition = rb.position;
        positionOffset = npcPosition - playerPosition;
        distanceToPlayer = Vector2.Distance(npcPosition, playerPosition);
        //distanceToPlayer = Vector2.Distance(rb.transform.position, player.transform.position);
    }
    private void UpdateState()
    {
        state.Update(distanceToPlayer, scaredRange, concernedRange);
    }

    private class NpcState
    {
        public CurrentAction Action;
        
        private StateTimer State;

        private MovementTimer Movement;
        
        public NpcState()
        {
            Action = CurrentAction.IdleStanding;
            State = new StateTimer();
            Movement = new MovementTimer();
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
            if (State.TimeEnd <= currentTime) // Update State and Rotation
            {
                Debug.Log(currentTime);
                Debug.Log(State.TimeEnd);
                if (IsScared(distanceToPlayer, scaredRange))
                {
                    Action = CurrentAction.Scared;
                }
                
                else if (IsConcerned(distanceToPlayer, concernedRange))
                {
                    Action = CurrentAction.Concerned;
                }
                
                else
                {
                    CurrentAction[] idles =
                    {
                        CurrentAction.IdleStanding, CurrentAction.IdleWalking
                    };
                    Action = idles[Random.Range(0, idles.Length)];
                }
                
                AssignStateTimer();
                AssignMovementTimer();
                
            }
            else if (Movement.TimeEnd <= currentTime) //Update Rotation
            {
                AssignMovementTimer();
            }
        }

        private void AssignStateTimer()
        {
            Debug.Log(State.TimeStart);
            Debug.Log(State.TimeEnd);
            State.TimeStart = Time.fixedTime;
            switch (Action)
            {
                case CurrentAction.IdleStanding:
                    State.TimeWindow = UnityEngine.Random.Range(1.0f, 3.0f);
                    State.TimeEnd = State.TimeStart + State.TimeWindow;
                    break;

                case CurrentAction.IdleWalking:
                    State.TimeWindow = UnityEngine.Random.Range(0.7f, 1.3f);
                    State.TimeEnd = State.TimeStart + State.TimeWindow;
                    break;

                case CurrentAction.Concerned:
                    State.TimeWindow = UnityEngine.Random.Range(0.5f, 2.0f);
                    State.TimeEnd = State.TimeStart + State.TimeWindow;
                    break;

                case CurrentAction.Scared:
                    State.TimeWindow = UnityEngine.Random.Range(1.5f, 2.0f);
                    State.TimeEnd = State.TimeStart + State.TimeWindow;
                    break;
            }
        }

        private void AssignMovementTimer()
        {
            Movement.TimeStart = Time.fixedTime;
            switch (Action)
            {
                case CurrentAction.IdleStanding:
                    Movement.TimeWindow = State.TimeWindow;
                    Movement.TimeEnd = Movement.TimeStart + Movement.TimeWindow;
                    break;

                case CurrentAction.IdleWalking:
                    Movement.TimeWindow = State.TimeWindow;
                    Movement.TimeEnd = Movement.TimeStart + Movement.TimeWindow;
                    break;

                case CurrentAction.Concerned:
                    Movement.TimeWindow = State.TimeWindow;
                    Movement.TimeEnd = Movement.TimeStart + Movement.TimeWindow;
                    break;

                case CurrentAction.Scared:
                    Movement.TimeWindow = State.TimeWindow / 3;
                    Movement.TimeEnd = Movement.TimeStart + Movement.TimeWindow;
                    break;
            }

            Movement.IsRotated = false;
        }

        public bool ShouldRotate()
        {
            if (Movement.IsRotated == false)
            {
                Movement.IsRotated = true;
                return true;
            }
            
            return false;
        }

        public float GetRotation(Vector2 positionOffset)
        {
            switch (Action)
            {
                case CurrentAction.IdleStanding:
                    return 0;

                case CurrentAction.IdleWalking:
                    return Random.Range(0.0f, 2 * Mathf.PI);

                case CurrentAction.Concerned:
                    return Mathf.Atan2(positionOffset.y, positionOffset.x);

                case CurrentAction.Scared:
                    return Mathf.Atan2(positionOffset.y, positionOffset.x);
                default:
                    return -1;
                    
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
        Scared
    }
}

