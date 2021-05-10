using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WoodcuttingUI : MonoBehaviour
{
    public GameObject axe;
    public static bool GameIsWCMenu = false;
    public GameObject WCCanvasUI;
    private bool meleeAttack;

    // Update is called once per frame
    private void Start()
    {
        SceneManager.sceneLoaded += LookForAxe;
        axe = GameObject.FindGameObjectWithTag("Axe");
    }

    private void LookForAxe(Scene scene, LoadSceneMode mode)
    {
        axe = GameObject.FindGameObjectWithTag("Axe");
    }

    void Update()
    {
        meleeAttack = Input.GetButtonDown("Fire1");

        if (meleeAttack && axe.activeInHierarchy)
        {
            if (GameIsWCMenu == false)
            {
                OpenWCMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsWCMenu)
            {
                ExitWCMenu();
            }
        }
    }
    private void ExitWCMenu()
    {
        WCCanvasUI.SetActive(false);
        GameIsWCMenu = false;
    }

    private void OpenWCMenu()
    {
        WCCanvasUI.SetActive(true);
        WoodcuttingMinigameController.InactivityTimer = 0.5f;
        GameIsWCMenu = true;
    }
}
