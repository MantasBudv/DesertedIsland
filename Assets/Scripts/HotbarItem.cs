using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Item", menuName = "Hotbar/Item")]
public class HotbarItem : ScriptableObject
{
    new public string name = "New Item";
    public List<Sprite> icon = new List<Sprite>();
    public int level = 0;
    public int key = 0;

    public bool active = false;
    public virtual void SetActive (bool value) 
    {
        //Debug.Log("Setting " + name + " " + value);
        active = value;
    }
}
