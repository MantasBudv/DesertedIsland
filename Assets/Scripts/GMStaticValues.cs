using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMStaticValues : MonoBehaviour
{
    private static int boatLevel = 1;

    public void SetBoatLevel(int level)
    {
        boatLevel = level;
    }

    public int GetBoatLevel()
    {
        return boatLevel;
    }
}
