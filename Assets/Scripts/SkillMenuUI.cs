using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillMenuUI : MonoBehaviour
{
    public static bool GameIsSkillMenu = false;
    public GameObject skillMenuUI;

    public GameObject CurrentLevelText;

    public GameObject XPLeftText;

    public GameObject SkillPointsText;
    //private CharacterController character;

    // Update is called once per frame
    private void Awake()
    {
        //character = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsSkillMenu)
            {
                ExitSkillMenu();
            }
            else
            {
                OpenSkillMenu();
            }
        }

    }

    private void ExitSkillMenu()
    {
        skillMenuUI.SetActive(false);
        WoodcuttingMinigameController.InactivityTimer = 0.5f;
        Time.timeScale = 1f;
        GameIsSkillMenu = false;
    }

    private void OpenSkillMenu()
    {
        skillMenuUI.SetActive(true);
        CurrentLevelText.GetComponent<TextMeshProUGUI>().text = "CURRENT LEVEL: " + CharacterController.currentLevel;
        XPLeftText.GetComponent<TextMeshProUGUI>().text = "XP FOR NEXT LEVEL: " + CharacterController.GetXPForNextLevel();
        SkillPointsText.GetComponent<TextMeshProUGUI>().text = "SKILL POINTS LEFT: " + CharacterController.skillPoints;
        Time.timeScale = 0f;
        GameIsSkillMenu = true;
    }
    public void AcquireStaminaRegen()
    {
        CharacterController.skills.SetSkill(CharacterController.AcquiredSkills.SkillEnum.StaminaRegen);
        SkillPointsText.GetComponent<TextMeshProUGUI>().text = "SKILL POINTS LEFT: " + CharacterController.skillPoints;
    }
}
