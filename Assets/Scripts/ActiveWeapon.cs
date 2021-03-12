using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField]
     List<GameObject> weaponsUI = new List<GameObject>();
    [SerializeField]
    HotbarItem[] weapons;
    void Start()
    {
        Transform[] transform = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform t in transform)
        {
            if (t.gameObject.name != "Hand")
            {
                weaponsUI.Add(t.gameObject);    
            }
        }

        weapons = Resources.FindObjectsOfTypeAll<HotbarItem>();
        Debug.Log(weapons.Length);
    }

    void Update()
    {
        setActiveWeapon();
    }
    void setActiveWeapon() 
    {
        foreach (GameObject weaponUI in weaponsUI)
        {
            bool activate = false;
            foreach (HotbarItem weapon in weapons)
            {
                if (weaponUI.name == weapon.name && weapon.active == true)
                {
                    activate = true;
                }
            }
            if (activate)
            {
                weaponUI.SetActive(true);
            }
            else
            {
                weaponUI.SetActive(false);
            }
        }
    }
}
