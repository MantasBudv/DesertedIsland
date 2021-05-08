using UnityEngine;
using System.Xml.Serialization;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public int indexOnSheet;
    public virtual void Use () 
    {
        Debug.Log("Using " + name);
    }
}
