using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class WoodcuttingMinigameController : MonoBehaviour
{
    private bool isActive = true;
    private int attempts;

    private int collectedWood;
    [SerializeField] private Item itemDrop;

    public static float InactivityTimer = 1f;

    private bool m1Down = false;
    private bool isSwinging = false;
    private bool isSwingingAnimation = false;
    private float swingingTimer;
    private const float swingAnimationTime = 0.6f;
    private const float swingActionTime = 1.2f;
    private float slowdownAmount;
    private float slowdownTimer = 0.0f;

    private const float xPos = 1422f;
    private const float yMin = -915f;
    private const float yMax = -140f;

    private float currentAttemptY = yMin;
    private float currentGoalY = yMin;
    private float increaseAmount = 3f;
    

    private bool isSlidingUp = true;
    
    private GameObject _axe;
    private GameObject _goalBar;
    private GameObject _attemptBar;

    private Animator anim;

    private void OnEnable()
    {
        _axe = transform.GetChild(0).gameObject;
        _goalBar = transform.GetChild(2).gameObject;
        _attemptBar = transform.GetChild(3).gameObject;
        anim = _axe.GetComponent<Animator>();
        InitMinigame();
    }

    private void InitMinigame()
    {
        isActive = true;
        InactivityTimer = 1f;
        attempts = 1;
        collectedWood = 0;
        NewGoalBar();
    }

    private void NewGoalBar()
    {
        if (attempts <= 0)
        {
            WoodcuttingUI.GameIsWCMenu = false;
            gameObject.SetActive(false);
            for (int i = 0; i < collectedWood; i++)
            {
                bool wasPickedUp = Inventory.instance.Add(itemDrop);
            }
            return;
        }

        increaseAmount = Random.Range(5.8f, 17.4f);
        currentAttemptY = yMin;
        currentGoalY = Random.Range(yMin, yMax);
        _goalBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, currentGoalY);
        attempts -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        m1Down = Input.GetButtonDown("Fire1");
        AssignColorOnDistance();
        InactivityTimer -= Time.deltaTime;
        if (m1Down && InactivityTimer < 0)
        {
            SwingAxe();
        }
            
    }

    void SwingAxe()
    {
        if (isSwinging)
            return;
        
        anim.SetBool("IsSwinging", true);
        isSwinging = true;
        isSwingingAnimation = true;
        swingingTimer = 0.0f;
        slowdownTimer = 0.0f;
        slowdownAmount = increaseAmount / 10;
    }
    
    private void AssignColorOnDistance()
    {
        float distance = Math.Abs(currentAttemptY - currentGoalY);
        if (distance > 255)
        {
            _attemptBar.GetComponent<Image>().color = Color.red;
        }
        else if (distance > 127.5f)
        {
            float greenColorIntensity = (255 - (distance * 2 - 255)) / 255f;
            Color color = new Color(1f, greenColorIntensity, 0f);
            _attemptBar.GetComponent<Image>().color = color;

        }
        else
        {
            float redColorIntensity = distance * 2 / 255f;
            Color color = new Color(redColorIntensity, 1f, 0);
            _attemptBar.GetComponent<Image>().color = color;
        }

    }

    private void UpdateSwinging()
    {
        swingingTimer += Time.fixedDeltaTime;
        slowdownTimer += Time.fixedDeltaTime;
        
        if (isSwingingAnimation)
        {
            if (slowdownTimer >= swingAnimationTime / 10)
            {
                increaseAmount -= slowdownAmount;
                slowdownTimer -= swingAnimationTime / 10;
            }
            if (swingingTimer >= swingAnimationTime)
            {
                anim.SetBool("IsSwinging", false);
                isSwingingAnimation = false;
            }
        }

        if (swingingTimer >= swingActionTime)
        {
            isSwinging = false;
            AssignRewards();
            NewGoalBar();
        }
    }

    private void AssignRewards()
    {
        float distance = Math.Abs(currentAttemptY - currentGoalY);
        if (distance < 100)
        {
            int collectedThisTime = 3 - ((int)distance / 33);
            collectedWood += collectedThisTime;
        }
    }
    private void FixedUpdate()
    {
        if (isActive == false)
            return;

        if (isSwinging)
        {
            UpdateSwinging();
        }
        
        if (isSlidingUp == true)
        {
            currentAttemptY += increaseAmount;
            if (currentAttemptY >= yMax)
            {
                isSlidingUp = !isSlidingUp;
            }
        }
        else
        {
            currentAttemptY -= increaseAmount;
            if (currentAttemptY <= yMin)
            {
                isSlidingUp = !isSlidingUp;
            }
        }
        
        _attemptBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, currentAttemptY);
    }
    
    
}
