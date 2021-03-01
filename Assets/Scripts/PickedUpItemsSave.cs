using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickedUpItemsSave : MonoBehaviour
{
    public static List<double> pickedUpItems = new List<double>();
    private int count = 0;

    private void Awake()
    {
        Debug.Log(count);
        count = 0;
        InvokeRepeating("deletePickedUpItems", 0.005f, 0.005f);
    }
    public void Start() 
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }
    private void deletePickedUpItems()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("Item");
        foreach(GameObject item in array)
        {
            Debug.Log(item.GetComponent<ItemPickUp>().getPositionHash());
            if (pickedUpItems.Contains(item.GetComponent<ItemPickUp>().getPositionHash()))
            {
                Destroy(item);
            }
        }
        count += 1;
        Debug.Log(count);
        if (count >= 100) 
        {
            CancelInvoke();
        }
    }
    private void ChangedActiveScene(Scene current, Scene next)
    {
        InvokeRepeating("deletePickedUpItems", 0.005f, 0.005f);
    }
}