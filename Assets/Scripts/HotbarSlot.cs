using UnityEngine;
using UnityEngine.UI;
public class HotbarSlot : MonoBehaviour
{
    public Image icon;
    public HotbarItem item;
    public void Start() 
    {
        upgradeItem(item.level);
    }
    public void upgradeItem(int level)
    {
        item.level = level;
        // change item.icon
        icon.sprite = item.icon[level];
        icon.enabled = true;
    }
    public void SetActive()
    {
        item.SetActive(true);
        var tempOpacity = icon.color;
        tempOpacity.a = 1f;
        icon.color = tempOpacity;
    }
    public void SetInactive()
    {
        item.SetActive(false);
        var tempOpacity = icon.color;
        tempOpacity.a = 0.5f;
        icon.color = tempOpacity;
    }
}
