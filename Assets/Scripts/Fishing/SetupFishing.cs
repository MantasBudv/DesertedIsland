using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetupFishing : MonoBehaviour
{
    [SerializeField] GameObject stage1;
    [SerializeField] GameObject stage2;
    [SerializeField] GameObject fishGrounds;
    [SerializeField] TextMeshPro replyText;
    [SerializeField] CharacterController controller;
    [SerializeField] Inventory inventory;
    [SerializeField] Item fish;
    public GameObject[] Xses;
    private int mistakeCount;
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        controller = GameObject.Find("Character").GetComponent<CharacterController>();
        mistakeCount = 0;
        stage2.SetActive(false);
        fishGrounds.SetActive(false);
        Invoke("initiateGame", 2);
    }

    void initiateGame()
    { 
        stage1.SetActive(true);
    }

    public void CountMistake()
    {
        mistakeCount++;
        Xses[mistakeCount - 1].SetActive(true);
        if (mistakeCount >= 3)
        {
            Lose();
        }
    }

    private void Lose()
    {
        FishingStage1.pause = true;
        FishingStage2.pause = true;
        replyText.text = "Failed";
        replyText.color = Color.red;
        replyText.enabled = true;
        Invoke("CloseGame", 2);

    }
    private void CloseGame()
    {
        controller.enabled = true;
        FishingMinigameStarter.inMinigame = false;
        Destroy(this.gameObject);
    }
    private void CloseGameWin()
    {
        controller.enabled = true;
        FishingMinigameStarter.inMinigame = false;
        Destroy(this.gameObject);
        inventory.AddItem(fish);
        controller.GiveXP(300 + (3 - mistakeCount) * 50);
    }
    public void Win()
    {
        FishingStage1.pause = true;
        FishingStage2.pause = true;
        replyText.text = "Success!";
        replyText.color = Color.green;
        replyText.enabled = true;
        Invoke("CloseGameWin", 2);

    }
}
