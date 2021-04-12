using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
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
    public bool Add (Item item) 
    {
        if (items.Contains(item)) 
        {
            int index = items.FindIndex(i => item.Equals(i));
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
    public void Remove (Item item) 
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public List<int> GetItemsQuant()
    {
        return itemsQuantity;
    }

    public void LoadInventory(List<Item> items, List<int> quant)
    {
        items.Clear();
        itemsQuantity.Clear();

        foreach (var i in items)
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

}
