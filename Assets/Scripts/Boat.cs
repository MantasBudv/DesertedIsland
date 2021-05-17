﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boat : MonoBehaviour
{
    [Header("Refferences")]
    [SerializeField] TextMeshPro[] Texts;
    [SerializeField] SpriteRenderer[] itemSprites;
    [SerializeField] GameObject box;
    [SerializeField] Sprite[] boatSprites;
    [SerializeField] GameObject poofPrefab;
    [SerializeField] TextMeshProUGUI timeTaken;

    [Header("Public variables")]
    public Inventory inventory;
    GMStaticValues values;

    [Header("ItemsNeeded")]
    [SerializeField] ItemAmount[] level1;
    [SerializeField] ItemAmount[] level2;

    private List<ItemAmount[]> ItemsNeeded = new List<ItemAmount[]>();

    private int boatLevel = 1;
    private bool _playerInRange = false;
    private Sprite[] tempSprite;
    private GameObject gameOverBox;


    private void OnValidate()
    {
        inventory = FindObjectOfType<Inventory>();
        values = FindObjectOfType<GMStaticValues>();
        
    }

    private void Start()
    {
        gameOverBox = GameObject.FindGameObjectWithTag("GameOverScreen");
        timeTaken = GameObject.FindGameObjectWithTag("TimeText").GetComponent<TextMeshProUGUI>();
        gameOverBox.SetActive(false);
        boatLevel = values.GetBoatLevel();
        
        ItemsNeeded.Add(level1);
        ItemsNeeded.Add(level2);
        Debug.Log(ItemsNeeded);
        if (boatLevel != 3)
        {
            ChangeSprites();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (boatLevel != 3)
            {
                box.SetActive(true);
                ChangeText();
            }
            _playerInRange = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            box.SetActive(false);
            _playerInRange = false;
        }
    }

    private void Update()
    {
        if (_playerInRange && Input.GetButtonDown("Interact"))
        {
            if (CanUpgrade())
            {
                UpgradeBoat();
            }
            else if (boatLevel == 3)
            {
                gameOverBox.SetActive(true);
                int day;
                float time;
                FindObjectOfType<Timer>().GetTime(out time, out day);
                timeTaken.text = "Day " + day.ToString() + " " + System.TimeSpan.FromMinutes(Mathf.Round(time / 10) * 10).ToString(@"hh\:mm");
                Time.timeScale = 0f;
            }
        }
    }

    private bool CanUpgrade()
    {
        if (boatLevel == 3)
        {
            return false;
        }
        if ((inventory.ItemCount(ItemsNeeded[boatLevel-1][0].Item) >= ItemsNeeded[boatLevel - 1][0].Amount) &&
                (inventory.ItemCount(ItemsNeeded[boatLevel - 1][1].Item) >= ItemsNeeded[boatLevel - 1][1].Amount))
        {
            return true;
        }


        Debug.LogError("Cant upgrade");
        return false;
    }

    private void UpgradeBoat()
    {
        RemoveBoatMaterials();
        boatLevel++;
        values.SetBoatLevel(boatLevel);
        gameObject.GetComponent<SpriteRenderer>().sprite = boatSprites[boatLevel - 1];
        if (boatLevel == 3)
        {
            box.SetActive(false);
        }
        Instantiate(poofPrefab, gameObject.transform.position, Quaternion.identity);
        if (boatLevel != 3)
        {
            ChangeText();
            ChangeSprites();
        }
        
    }

    private void RemoveBoatMaterials()
    {
        for (int i = 0; i < ItemsNeeded[boatLevel-1][0].Amount; i++)
        {
            inventory.RemoveItem(ItemsNeeded[boatLevel - 1][0].Item.ItemName);
        }
        for (int i = 0; i < ItemsNeeded[boatLevel - 1][1].Amount; i++)
        {
            inventory.RemoveItem(ItemsNeeded[boatLevel - 1][1].Item.ItemName);
        }

    }

    private void ChangeSprites()
    {
        tempSprite = Resources.LoadAll<Sprite>("ItemSprites/Original_items");
        gameObject.GetComponent<SpriteRenderer>().sprite = boatSprites[boatLevel - 1];

        itemSprites[0].sprite = tempSprite[ItemsNeeded[boatLevel - 1][0].Item.indexOnSheet];
        itemSprites[1].sprite = tempSprite[ItemsNeeded[boatLevel - 1][1].Item.indexOnSheet];

    }

    private void ChangeText()
    {
        Texts[0].text = inventory.ItemCount(ItemsNeeded[boatLevel - 1][0].Item).ToString() + "/" + ItemsNeeded[boatLevel - 1][0].Amount;
        if (inventory.ItemCount(ItemsNeeded[boatLevel - 1][0].Item) < ItemsNeeded[boatLevel - 1][0].Amount)
        {
            Texts[0].color = new Color(255, 0, 0);
        }
        else
        {
            Texts[0].color = new Color(0, 255, 0);
        }

        Texts[1].text = inventory.ItemCount(ItemsNeeded[boatLevel - 1][1].Item).ToString() + "/" + ItemsNeeded[boatLevel - 1][1].Amount;
        if (inventory.ItemCount(ItemsNeeded[boatLevel - 1][1].Item) < ItemsNeeded[boatLevel - 1][1].Amount)
        {
            Texts[1].color = new Color(255, 0, 0);
        }
        else
        {
            Texts[1].color = new Color(0, 255, 0);
        }
    }
}
