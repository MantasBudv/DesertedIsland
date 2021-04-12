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

    [SerializeField] private float moveSpeedRegular = 2.2f;

    [SerializeField] private float moveSpeedAgressive = 1.5f;

    //[SerializeField] private float concernedRange = 1.5f;

    [SerializeField] private float agressiveRange = 10f;

    [SerializeField] private GameObject aggresiveNpc;
    
    private Rigidbody2D _player;
    private Rigidbody2D _rb;
    private float _rotationRads;
    private Vector2 _positionOffset;
    private float _distanceToPlayer;

    private CurrentAction _action;
    private bool _shouldRotate;

    private float duplicateTimer;
    
    public static int SplitCount = 7;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _action = CurrentAction.Agressive;
        duplicateTimer = 0.0f;
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
            return;
        }

        var movementSpeed = (_action == CurrentAction.Agressive) ? moveSpeedAgressive : moveSpeedRegular;
        
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

    private void Duplicate()
    {
        Debug.Log("VEIKIA");
        if (SplitCount > 0)
        {
            SplitCount--;
            var pos = _rb.position;
            pos.y += 0.5f;
        
            Instantiate(aggresiveNpc, pos, Quaternion.identity);
        }
    }
    
    public bool IsAgressive()
    {
        return _distanceToPlayer < agressiveRange;
    }
    
    public void UpdateState()
    {
        if (IsAgressive())
        {
            _action = CurrentAction.Agressive;
            duplicateTimer += Time.deltaTime;
        }
        else
        {
            _action = CurrentAction.Idle;
            duplicateTimer = 0.0f;
        }
    }
    private enum CurrentAction
    {
        Idle,
        Agressive,
    }
}
