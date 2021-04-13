﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController : MonoBehaviour
{
    private float timer = 2f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
