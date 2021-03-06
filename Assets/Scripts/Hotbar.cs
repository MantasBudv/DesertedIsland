using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Hotbar : MonoBehaviour
{
    public Transform itemsParent;
    HotbarSlot[] slots;
    void Start()
    {
        slots = itemsParent.GetComponentsInChildren<HotbarSlot>();
    }
    public void Update()
    {
        UpdateActiveValues();
        UpdateUI();
    }
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item.active)
            {
                slots[i].SetActive();
            }
            else
            {
                slots[i].SetInactive();
            }
        }
    }
    void UpdateActiveValues()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            slots[0].upgradeItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            setActiveSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            setActiveSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            setActiveSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            setActiveSlot(3);
        }
    }
    void setActiveSlot(int num)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i != num)
            {
                slots[i].SetInactive();
            }
            else
            {
                slots[i].SetActive();
            }
        }
    }

    public List<int> GetLevels()
    {
        List<int> levels = new List<int>();
        foreach (var item in slots)
        {
            levels.Add(item.item.level);
        }
        return levels;
    }

    public void SetLevels(List<int> levels)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].upgradeItem(levels[i]);
        }
    }
}
