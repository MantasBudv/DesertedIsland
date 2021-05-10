using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingStage2 : MonoBehaviour
{
    [SerializeField] GameObject fish;
    [SerializeField] Transform hook;
    [SerializeField] Transform fishGrounds;
    [SerializeField] Transform leftPivot;
    [SerializeField] Transform rightPivot;
    [SerializeField] Transform topPivot;
    [SerializeField] Transform bottomPivot;
    [SerializeField] SetupFishing fishingScript;
    [SerializeField] float reelPower = 0.1f;
    [SerializeField] float gravityPower = 0.005f;
    public float reelVelocity;
    public float hookPosition;
    public float fishSpeed;
    private float errorTimer;
    bool isCaught;
    bool moveLeft;
    bool pressedA;
    float fishSpeedTimer = 1f;
    public static bool pause;
    // Start is called before the first frame update
    void Start()
    {
        fishingScript = GetComponentInParent<SetupFishing>();
        errorTimer = 0.5f;
        pause = false;
        pressedA = false;
        moveLeft = false;
        isCaught = false;
        fish.transform.position = new Vector3(leftPivot.position.x, fishGrounds.position.y, 0);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (pause) return;

        errorTimer -= Time.deltaTime;
        fishSpeedTimer -= Time.deltaTime;
        if (!isCaught)
        {
            MoveFish();
        }
        else
        {
            ReelFishIn();
        }
        if (fishSpeedTimer < 0)
        {
            System.Random rand = new System.Random();
            fishSpeed = Convert.ToSingle(rand.NextDouble()) * 3f + 0.5f;
            fishSpeedTimer = 1.5f;
        }
    }
    void Update()
    { 
        if (pause) return;
        if (isCaught)
        {
            CheckReelInputs();
        }
        if (Input.GetMouseButtonDown(0) && errorTimer < 0f)
        {
            CheckIfCaught();
        }
    }
   
    private void CheckIfCaught()
    {
        if (fish.transform.position.x < this.transform.parent.position.x + 0.2f  && fish.transform.position.x > this.transform.parent.position.x - 0.2f)
        {
            isCaught = true;
            fish.transform.position = hook.position - new Vector3(0.1f, 0.1f, 0);
            fish.transform.parent = hook;
            float distance = topPivot.position.y - bottomPivot.position.y + 0.8f;
            hookPosition = (hook.position.y - bottomPivot.position.y) / distance;
            Debug.Log(hookPosition);
        }
        else
        {
            fishingScript.CountMistake();
            errorTimer = 0.5f;
        }
    }

    void MoveFish()
    {
        CheckDirection();
        if (moveLeft)
        {
            fish.transform.Translate(Vector2.left * fishSpeed * Time.deltaTime);
        }
        else
        {
            fish.transform.Translate(Vector2.right * fishSpeed * Time.deltaTime);
        }

    }
    void CheckDirection()
    {
        if (fish.transform.position.x >= rightPivot.position.x)
        {
            fish.transform.localScale = new Vector3(-1.5f, 1.5f, 1);
            moveLeft = true;
        }
        if (fish.transform.position.x <= leftPivot.position.x)
        {
            fish.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            moveLeft = false;
        }
    }
    void CheckReelInputs()
    {
        if (Input.GetKeyDown(KeyCode.A) && !pressedA)
        {
            pressedA = true;
            reelVelocity += reelPower * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.D) && pressedA)
        {
            pressedA = false;
            reelVelocity += reelPower * Time.deltaTime;
        }
    }
    void ReelFishIn()
    {

        reelVelocity -= gravityPower * Time.deltaTime;
        reelVelocity = Mathf.Clamp(reelVelocity, -0.003f, 0.005f);
        
        if (hookPosition == 0)
        {
            fishingScript.CountMistake();
            reelVelocity = 0.013f;
        }
        if (hookPosition == 1)
        {
            fishingScript.Win();
        }
        hookPosition += reelVelocity;
        hookPosition = Mathf.Clamp(hookPosition, 0, 1);
        hook.transform.position = Vector3.Lerp(bottomPivot.position, topPivot.position + new Vector3(0, 0.8f, 0), hookPosition);

        
    }

}
