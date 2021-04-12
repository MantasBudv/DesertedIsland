using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeUIScript : MonoBehaviour
{
    private GameObject _canvas;
    private bool _isOpen;
    private bool _setActiveOperationValue;
    // private void Awake()
    // {
    //     if (!_created)
    //     {
    //         _created = true;
    //         DontDestroyOnLoad(transform.gameObject);
    //     }
    //     else {
    //         Destroy(gameObject);
    //     }
    // }
    // private static bool _created = false;
    private void OnEnable()
    {
        _isOpen = false;
        _setActiveOperationValue = false;
        _canvas = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) == true)
        {
            _isOpen = !_isOpen;
        }

        if (_isOpen && !_setActiveOperationValue)
        {
            _setActiveOperationValue = true;
            _canvas.SetActive(true);
        }

        if (!_isOpen && _setActiveOperationValue)
        {
            _setActiveOperationValue = false;
            Debug.Log("CIA");
            _canvas.SetActive(false);
        }
    }
}
