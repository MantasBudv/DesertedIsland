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
    Transform topFishLimit;
    Transform bottomFishLimit;
    [SerializeField] Transform hook;
    float hookPosition;
    public float hookStartingVelocity = 1.2f;
    public float hookStoppingPower = 0.5f;
    public float hookVelocity;
    bool moveUp;
    bool stopMoving;
    // Start is called before the first frame update
    void Start()
    {
        SetupFishingPivots();
        stopMoving = false;
        moveUp = false;
        hookVelocity = hookStartingVelocity;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        Hook();
        
    }

    void Hook()
    {
        if (Input.GetMouseButton(0))
        {
            stopMoving = true;
            Debug.Log("YES!");
        }

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
        else
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
    }
    private void SetupFishingPivots()
    {
        double topLimit = topPivot.position.y - 0.5f;
        double bottomLimit = topPivot.position.y - 3f;
        float topPosition = GetRandomNumber(bottomLimit, topLimit);
        topFishingPivot.position = new Vector3(0, topPosition ,0);
        bottomFishingPivot.position = new Vector3(0, topPosition - GetRandomNumber(0.5, 1),0);
        fishingPlot.transform.localScale = new Vector3(5, (topPosition - bottomFishingPivot.position.y), 1);
        fishingPlot.transform.position = new Vector3(0, topPosition - (topPosition - bottomFishingPivot.position.y) / 2, 0);
        
        fishingPlot.SetActive(true);
        fishingPlot.transform.position = new Vector3(0, topPosition - (topPosition - bottomFishingPivot.position.y) / 2, 0);
    }
    public float GetRandomNumber(double minimum, double maximum)
    {
        System.Random rand = new System.Random();
        return Convert.ToSingle(rand.NextDouble() * (maximum - minimum) + minimum);
    }
}
