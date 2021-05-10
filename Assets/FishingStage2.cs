using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingStage2and3 : MonoBehaviour
{
    [SerializeField] GameObject fish;
    [SerializeField] Transform hook;
    [SerializeField] Transform fishGrounds;
    [SerializeField] Transform leftPivot;
    [SerializeField] Transform rightPivot;
    [SerializeField] Transform topPivot;
    [SerializeField] Transform bottomPivot;
    [SerializeField] float reelPower = 0.1f;
    [SerializeField] float gravityPower = 0.005f;
    public float reelVelocity;
    public float hookPosition;
    public float fishSpeed = 0.1f;
    bool isCaught;
    bool moveLeft;
    bool pressedA;
    float fishSpeedTimer = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        pressedA = false;
        moveLeft = false;
        isCaught = false;
        fish.transform.position = new Vector3(leftPivot.position.x, fishGrounds.position.y, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
    private void Update()
    {
        if (isCaught)
        {
            CheckReelInputs();
        }
        if (Input.GetMouseButton(0))
        {
            CheckIfCaught();
        }
    }
   
    private void CheckIfCaught()
    {
        if (fish.transform.position.x < this.transform.parent.position.x + 0.15f  && fish.transform.position.x > this.transform.parent.position.x - 0.15f)
        {
            isCaught = true;
            fish.transform.position = hook.position - new Vector3(0.15f, 0.14f, 0);
            fish.transform.parent = hook;
            float distance = topPivot.position.y - bottomPivot.position.y + 0.8f;
            hookPosition = (hook.position.y - bottomPivot.position.y) / distance;
            Debug.Log(hookPosition);
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
            Debug.Log("A");
        }
        if (Input.GetKeyDown(KeyCode.D) && pressedA)
        {
            Debug.Log("D");
            pressedA = false;
            reelVelocity += reelPower * Time.deltaTime;
        }
    }
    void ReelFishIn()
    {

        reelVelocity -= gravityPower * Time.deltaTime;
        reelVelocity = Mathf.Clamp(reelVelocity, -0.003f, 0.005f);
        hookPosition += reelVelocity;
        hookPosition = Mathf.Clamp(hookPosition, 0, 1);
        hook.transform.position = Vector3.Lerp(bottomPivot.position, topPivot.position + new Vector3(0, 0.8f, 0), hookPosition);
    }

}
