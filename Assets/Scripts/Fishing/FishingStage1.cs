using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingStage1 : MonoBehaviour
{

    [SerializeField] Transform topPivot;
    [SerializeField] Transform bottomPivot;
    [SerializeField] Transform topFishingPivot;
    [SerializeField] Transform bottomFishingPivot;
    [SerializeField] GameObject fishingPlot;
    [SerializeField] GameObject Stage2;
    [SerializeField] SetupFishing fishingScript;
    [SerializeField] Transform hook;
    public float hookStartingVelocity = 1.2f;
    public float hookStoppingPower = 0.5f;
    public float hookVelocity;
    private float errorTimer;
    bool moveUp;
    bool stopMoving;
    public static bool pause;
    // Start is called before the first frame update
    void Start()
    {
        pause = false;
        errorTimer = 0.5f;
        fishingScript = GetComponentInParent<SetupFishing>();
        SetupFishingPivots();
        stopMoving = false;
        moveUp = false;
        hookVelocity = hookStartingVelocity;
        
    }


    void Update()
    {
        if (pause) return;
        if (Input.GetMouseButtonDown(0))
        {
            stopMoving = true;
            Debug.Log("YES!");
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        errorTimer -= Time.deltaTime;
        if (pause) return;
        Hook();
        
    }
    void Hook()
    {

        if (stopMoving && hookVelocity >= 0)
        {
            hookVelocity -= hookStoppingPower;
        }
        if (hookVelocity > 0)
        {
            CheckDirection();
            if (moveUp)
            {
                hook.Translate(Vector2.up * hookVelocity * Time.deltaTime);
            }
            else
            {
                hook.Translate(Vector2.down * hookVelocity * Time.deltaTime);
            }
        }
        else if (errorTimer <= 0)
        {
            
            CheckFinishedPosition();
        }
 
    }
    void CheckDirection()
    {
        if (hook.position.y >= topPivot.position.y)
        {
            moveUp = false;
        }
        if (hook.position.y <= bottomPivot.position.y)
        {
            moveUp = true;
            if (!stopMoving)
            {
                hookVelocity += 0.5f;
                hookStoppingPower += 0.001f;

            }
        }
    }
    void CheckFinishedPosition()
    {
        if (hook.position.y + 0.1f <= topFishingPivot.position.y && hook.position.y - 0.1 >= bottomFishingPivot.position.y)
        {
            Stage2.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else
        {

            fishingScript.CountMistake();
            stopMoving = false;
            hookVelocity = hookStartingVelocity;
            errorTimer = 0.5f;
        }
    }
    private void SetupFishingPivots()
    {
        double topLimit = topPivot.position.y - 0.5f;
        double bottomLimit = topPivot.position.y - 3f;
        float topPosition = GetRandomNumber(bottomLimit, topLimit);
        topFishingPivot.position = new Vector3(0, topPosition ,0);
        bottomFishingPivot.position = new Vector3(0, topPosition - GetRandomNumber(0.5, 1),0);
        fishingPlot.transform.localScale = new Vector3(5, (topPosition - bottomFishingPivot.position.y), 1);
        fishingPlot.transform.position = new Vector3(this.transform.position.x, topPosition - (topPosition - bottomFishingPivot.position.y) / 2, 0);
        
        fishingPlot.SetActive(true);
    }
    public float GetRandomNumber(double minimum, double maximum)
    {
        System.Random rand = new System.Random();
        return Convert.ToSingle(rand.NextDouble() * (maximum - minimum) + minimum);
    }
}
