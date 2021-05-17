using UnityEngine;
using System.Xml.Serialization;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    new public string ItemName = "New Item";
    public int indexOnSheet;
    public virtual void Use () 
    {
        Debug.Log("Using " + ItemName);
    }
}
