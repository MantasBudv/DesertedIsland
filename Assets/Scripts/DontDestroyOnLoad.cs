using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        if (!created)
        {
            created = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    private static bool created = false;
}
