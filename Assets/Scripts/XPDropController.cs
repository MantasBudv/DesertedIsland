using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class XPDropController : MonoBehaviour
{
    private float timer = 0.0f;
    private float timerEnd = 0.8f;

    private float ySpeed = 0.02f;
    public TextMeshPro Text;
    private Transform coords;

    private void Awake()
    {
        coords = gameObject.GetComponent<Transform>();
    }

    public void SetXPAmount(int amount)
    {
        Text.sortingOrder = 8;
        Text.text = amount + " XP";
    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= timerEnd)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        Vector3 temp = transform.position;
        temp.y += ySpeed;
        transform.position = temp;
    }
}
