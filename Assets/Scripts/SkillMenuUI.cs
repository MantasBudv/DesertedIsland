using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenuUI : MonoBehaviour
{
    public static bool GameIsSkillMenu = false;
    public GameObject skillMenuUI;

    public GameObject CurrentLevelText;

    public GameObject XPLeftText;

    public GameObject SkillPointsText;
    
    
        
    public GameObject StaminaRegenButton;
    public GameObject MoveSpeedButton;
    public GameObject XPBoostButton;
    public GameObject ResistanceBoostButton;
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
        ReturnColor();
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
        CheckIfCompleted();
        GameIsSkillMenu = true;
    }
    public void AcquireStaminaRegen()
    {
        Debug.Log(CharacterController.skillPoints);
        if (CharacterController.skills.HasAcquired(CharacterController.AcquiredSkills.SkillEnum.StaminaRegen)
            || CharacterController.skillPoints <= 0)
        {
            return;
        }
        CharacterController.skills.SetSkill(CharacterController.AcquiredSkills.SkillEnum.StaminaRegen);
        SkillPointsText.GetComponent<TextMeshProUGUI>().text = "SKILL POINTS LEFT: " + CharacterController.skillPoints;
        StaminaRegenButton.GetComponent<Image>().color = Color.green;
    }
    
    public void AcquireXPBoost()
    {
        if (CharacterController.skills.HasAcquired(CharacterController.AcquiredSkills.SkillEnum.XPBoost)            
            || CharacterController.skillPoints <= 0)
        {
            return;
        }
        CharacterController.skills.SetSkill(CharacterController.AcquiredSkills.SkillEnum.XPBoost);
        SkillPointsText.GetComponent<TextMeshProUGUI>().text = "SKILL POINTS LEFT: " + CharacterController.skillPoints;
        XPBoostButton.GetComponent<Image>().color = Color.green;
    }
    
    public void AcquireMoveSpeedBoost()
    {
        if (CharacterController.skills.HasAcquired(CharacterController.AcquiredSkills.SkillEnum.MoveSpeed)
            || CharacterController.skillPoints <= 0)
        {
            
            return;
        }
        CharacterController.skills.SetSkill(CharacterController.AcquiredSkills.SkillEnum.MoveSpeed);
        SkillPointsText.GetComponent<TextMeshProUGUI>().text = "SKILL POINTS LEFT: " + CharacterController.skillPoints;
        MoveSpeedButton.GetComponent<Image>().color = Color.green;
    }
    
    public void AcquireResistanceBoost()
    {
        if (CharacterController.skills.HasAcquired(CharacterController.AcquiredSkills.SkillEnum.ResistanceBoost)
            || CharacterController.skillPoints <= 0)
        {
            return;
        }
        CharacterController.skills.SetSkill(CharacterController.AcquiredSkills.SkillEnum.ResistanceBoost);
        SkillPointsText.GetComponent<TextMeshProUGUI>().text = "SKILL POINTS LEFT: " + CharacterController.skillPoints;
        ResistanceBoostButton.GetComponent<Image>().color = Color.green;
    }

    private void ReturnColor()
    {
        StaminaRegenButton.GetComponent<Image>().color = Color.white;
        XPBoostButton.GetComponent<Image>().color = Color.white;
        MoveSpeedButton.GetComponent<Image>().color = Color.white;
        ResistanceBoostButton.GetComponent<Image>().color = Color.white;
    }
    private void CheckIfCompleted()
    {
        if (CharacterController.skills.HasAcquired(CharacterController.AcquiredSkills.SkillEnum.StaminaRegen))
        {
            StaminaRegenButton.GetComponent<Image>().color = Color.green;
        }
        if (CharacterController.skills.HasAcquired(CharacterController.AcquiredSkills.SkillEnum.XPBoost))
        {
            XPBoostButton.GetComponent<Image>().color = Color.green;
        }
        if (CharacterController.skills.HasAcquired(CharacterController.AcquiredSkills.SkillEnum.MoveSpeed))
        {
            MoveSpeedButton.GetComponent<Image>().color = Color.green;
        }
        if (CharacterController.skills.HasAcquired(CharacterController.AcquiredSkills.SkillEnum.ResistanceBoost))
        {
            ResistanceBoostButton.GetComponent<Image>().color = Color.green;
        }
    }
}
