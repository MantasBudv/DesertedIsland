using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMenuUI : MonoBehaviour
{
    public static bool GameIsSkillMenu = false;
    public GameObject skillMenuUI;
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
        Time.timeScale = 1f;
        GameIsSkillMenu = false;
    }

    private void OpenSkillMenu()
    {
        skillMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsSkillMenu = true;
    }

    public void AcquireStaminaRegen()
    {
        CharacterController.skills.SetSkill(CharacterController.AcquiredSkills.SkillEnum.StaminaRegen);
    }
}
