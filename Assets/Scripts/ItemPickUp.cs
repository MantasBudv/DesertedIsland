using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public float radius = 0.5f;
    private double hash;
    private SpriteRenderer spriteRenderer;
    public Item item;
    private Sprite[] tempSprite;
    private void Awake() 
    {
        hash = (1000*transform.position.x) + (0.001*transform.position.y);
        spriteRenderer = GetComponent<SpriteRenderer>();
        tempSprite = Resources.LoadAll<Sprite>("ItemSprites/Original_items");
        spriteRenderer.sprite = tempSprite[item.indexOnSheet];
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
        Debug.Log("Picking up " + item.ItemName);
        bool wasPickedUp = Inventory.instance.AddItem(item);
        if (wasPickedUp) 
        {
            PickedUpItemsSave.pickedUpItems.Add(hash);
            Destroy(gameObject);
        }
    }
}
