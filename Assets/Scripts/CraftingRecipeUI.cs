using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform arrowParrent;
    [SerializeField] InventorySlot[] itemSlots;

    [Header("Public Variables")]
    public Inventory ItemContainer;

    private CraftingRecipe craftingRecipe;
    public CraftingRecipe CraftingRecipe
    {
        get { return craftingRecipe; }
        set { SetCraftingRecipe(value); }
    }

    private Sprite[] tempSprite;

    
    public void OnCraftButtonClick()
    {
        if (craftingRecipe != null && ItemContainer != null)
        {
            if (craftingRecipe.CanCraft(ItemContainer))
            {
                if (!ItemContainer.IsFull())
                {
                    craftingRecipe.Craft(ItemContainer);
                }
                else
                {
                    Debug.LogError("Iventory is full");
                }
            }
            else
            {
                Debug.LogError("Not enough materials");
            }
        }
    }

    private void SetCraftingRecipe(CraftingRecipe newCraftingRecipe)
    {
        craftingRecipe = newCraftingRecipe;

        if(craftingRecipe != null)
        {
            int slotIndex = 0;
            slotIndex = SetSlots(craftingRecipe.Materials, slotIndex);
            arrowParrent.SetSiblingIndex(slotIndex);
            slotIndex = SetSlots(craftingRecipe.Results, slotIndex);

            Button b = itemSlots[slotIndex-1].gameObject.GetComponent<Button>();
            b.enabled = true;
            b.onClick.AddListener(delegate () { OnCraftButtonClick(); });

            for (int i = slotIndex; i < itemSlots.Length; i++)
            {
                itemSlots[i].gameObject.SetActive(false);
            }
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private int SetSlots(IList<ItemAmount> itemAmountList, int slotIndex)
    {
        for (int i = 0; i < itemAmountList.Count; i++, slotIndex++)
        {
            ItemAmount itemAmount = itemAmountList[i];
            InventorySlot itemSlot = itemSlots[slotIndex];

            tempSprite = Resources.LoadAll<Sprite>("ItemSprites/Original_items");
            itemSlot.icon.sprite = tempSprite[itemAmount.Item.indexOnSheet];
            itemSlot.AddCount(itemAmount.Amount);
            itemSlot.icon.enabled = true;
            itemSlot.transform.parent.gameObject.SetActive(true);

        }

        return slotIndex;
    }


}
