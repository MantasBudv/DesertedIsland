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

    [Header("Public variables")]
    public Inventory inventory;
    GMStaticValues values;

    [Header("ItemsNeeded")]
    [SerializeField] ItemAmount[] level1;
    [SerializeField] ItemAmount[] level2;


    private int boatLevel = 1;
    private bool _playerInRange = false;
    private Sprite[] tempSprite;


    private void OnValidate()
    {
        inventory = FindObjectOfType<Inventory>();
        values = FindObjectOfType<GMStaticValues>();
    }

    private void Start()
    {
        boatLevel = values.GetBoatLevel();
        ChangeSprites();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && boatLevel != 3)
        {
            box.SetActive(true);
            _playerInRange = true;
            ChangeText();
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
        }
    }

    private bool CanUpgrade()
    {
        if (boatLevel == 1)
        {
            if ((inventory.ItemCount(level1[0].Item) >= level1[0].Amount) &&
                    (inventory.ItemCount(level1[1].Item) >= level1[1].Amount))
            {
                return true;
            }
        }
        if (boatLevel == 2)
        {
            if ((inventory.ItemCount(level2[0].Item) >= level2[0].Amount) &&
                    (inventory.ItemCount(level2[1].Item) >= level2[1].Amount))
            {
                return true;
            }
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
        ChangeText();
        ChangeSprites();
    }

    private void RemoveBoatMaterials()
    {
        if (boatLevel == 1)
        {
            for (int i = 0; i < level1[0].Amount; i++)
            {
                inventory.RemoveItem(level1[0].Item.name);
            }
            for (int i = 0; i < level1[1].Amount; i++)
            {
                inventory.RemoveItem(level1[1].Item.name);
            }
        }
        if (boatLevel == 2)
        {
            for (int i = 0; i < level2[0].Amount; i++)
            {
                inventory.RemoveItem(level2[0].Item.name);
            }
            for (int i = 0; i < level2[1].Amount; i++)
            {
                inventory.RemoveItem(level2[1].Item.name);
            }
        }
    }

    private void ChangeSprites()
    {
        tempSprite = Resources.LoadAll<Sprite>("ItemSprites/Original_items");
        gameObject.GetComponent<SpriteRenderer>().sprite = boatSprites[boatLevel - 1];
        if (boatLevel == 1)
        {
            itemSprites[0].sprite = tempSprite[level1[0].Item.indexOnSheet]; 
            itemSprites[1].sprite = tempSprite[level1[1].Item.indexOnSheet]; 
        }
        if (boatLevel == 2)
        {
            itemSprites[0].sprite = tempSprite[level2[0].Item.indexOnSheet];
            itemSprites[1].sprite = tempSprite[level2[1].Item.indexOnSheet];
        }
    }

    private void ChangeText()
    {
        if (boatLevel == 1)
        {
            
            Texts[0].text = inventory.ItemCount(level1[0].Item).ToString() + "/" + level1[0].Amount;
            if (inventory.ItemCount(level1[0].Item) < level1[0].Amount)
            {
                Texts[0].color = new Color(255, 0, 0);
            }
            else
            {
                Texts[0].color = new Color(0, 255, 0);
            }
           
            Texts[1].text = inventory.ItemCount(level1[1].Item).ToString() + "/" + level1[1].Amount;
            if (inventory.ItemCount(level1[1].Item) < level1[1].Amount)
            {
                Texts[1].color = new Color(255, 0, 0);
            }
            else
            {
                Texts[1].color = new Color(0, 255, 0);
            }
            
        }

        if (boatLevel == 2)
        {

            Texts[0].text = inventory.ItemCount(level2[0].Item).ToString() + "/" + level2[0].Amount;
            if (inventory.ItemCount(level2[0].Item) < level2[0].Amount)
            {
                Texts[0].color = new Color(255, 0, 0);
            }
            else
            {
                Texts[0].color = new Color(0, 255, 0);
            }

            Texts[1].text = inventory.ItemCount(level2[1].Item).ToString() + "/" + level2[1].Amount;
            if (inventory.ItemCount(level2[1].Item) < level2[1].Amount)
            {
                Texts[1].color = new Color(255, 0, 0);
            }
            else
            {
                Texts[1].color = new Color(0, 255, 0);
            }

        }
    }
}
