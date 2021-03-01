﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public float radius = 0.5f;
    private double hash;
    private SpriteRenderer spriteRenderer;
    public Item item;
    private void Awake() 
    {
        hash = (1000*transform.position.x) + (0.001*transform.position.y);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.icon;
    }
    public double getPositionHash()
    {
        return hash;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PickUp();
        }
    }

    void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp) 
        {
            PickedUpItemsSave.pickedUpItems.Add(hash);
            Destroy(gameObject);
        }
    }
}
