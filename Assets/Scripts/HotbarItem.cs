using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Hotbar/Item")]
public class HotbarItem : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public int level = 0;
    public int key = 0;

    public bool active = false;
    public virtual void SetActive (bool value) 
    {
        Debug.Log("Setting " + name + " active");
        active = value;
    }
}
