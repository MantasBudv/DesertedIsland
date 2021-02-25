using UnityEngine;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public GameObject itemNameBackground;
    public Transform itemName;
    public GameObject itemCountBackground;
    public Transform itemCount;

    
    Item item;
    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        itemName.GetComponent<TMPro.TextMeshProUGUI>().text = item.name;
        icon.enabled = true;
        itemNameBackground.SetActive(true);
    }
    public void AddCount(int newCount) {
        itemCount.GetComponent<TMPro.TextMeshProUGUI>().text = newCount.ToString();
        itemCountBackground.SetActive(true);
    }
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        itemNameBackground.SetActive(false);
        itemCountBackground.SetActive(false);
    }
    public void UseItem ()
    {
        if (item != null)
        {
            item.Use();
        }
    }
}
