﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private void Awake()
    {
        if (!created)
        {
            created = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    private static bool created = false;
    public Transform itemsParent;
    public GameObject inventoryUI;
    Inventory inventory;
    InventorySlot[] slots;
    [SerializeField] GameObject CraftingUIWindow;
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            CraftingUIWindow.SetActive(false);
        }
        if (Input.GetButtonDown("Crafting"))
        {
            CraftingUIWindow.SetActive(!CraftingUIWindow.activeSelf);
            inventoryUI.SetActive(false);
        }
    }
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
                slots[i].AddCount(inventory.itemsQuantity[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
