﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour , IItemContainer
{
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    public delegate void  OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public int space = 12;

    public List<Item> items = new List<Item>();
    public List<int> itemsQuantity = new List<int>();
    public bool AddItem (Item item) 
    {
        if (ContainsItem(item.ItemName)) 
        {
            //int index = items.FindIndex(i => item.Equals(i));
            int index = GetItemIndex(item.ItemName);
            itemsQuantity[index]++;
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
            return true;
        } 
        else if (items.Count >= space) 
        {
            Debug.Log("Inventory full!");
            return false;
        }
        
        items.Add(item);
        itemsQuantity.Add(1);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        return true;
    }
    public void RemoveItem (string name) 
    {
        Item itemToRemove = items.Find(item => item.ItemName == name);
        if (itemToRemove) {
            Debug.Log("removing item" + itemToRemove.ItemName);
            int index = items.FindIndex(i => itemToRemove.Equals(i));
            if (itemsQuantity[index] != 1) {
                itemsQuantity[index]--;
            } else {
                items.RemoveAt(index);
                itemsQuantity.RemoveAt(index);
            }
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }
    }

    private int GetItemIndex (string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ItemName == name)
                return i;
        }
        return -1;
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public bool ContainsItem(string name) {
        bool exists = false;
        GetItems().ForEach(item => {
            if (item.ItemName == name) {
                exists = true;
            }
        });
        return exists;
    }

    public List<int> GetItemsQuant()
    {
        return itemsQuantity;
    }

    public void LoadInventory(List<Item> itemsL, List<int> quant)
    {
        items.Clear();
        itemsQuantity.Clear();

        foreach (var i in itemsL)
        {
            items.Add(i);
        }

        foreach (var i in quant)
        {
            itemsQuantity.Add(i);
        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public bool IsFull()
    {
        if (items.Count == space)
            return true;
        else return false;
    }

    public int ItemCount(Item item)
    {
        if (items.Contains(item))
        {
            int index = items.FindIndex(i => item.Equals(i));
            return itemsQuantity[index];
        }
        else return 0;
    }

    public Inventory GetInventoryInstance()
    {
        return instance;
    }
}
