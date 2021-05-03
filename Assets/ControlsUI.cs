using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    public GameObject controlsUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            controlsUI.SetActive(!controlsUI.activeSelf);
        }
    }
}
