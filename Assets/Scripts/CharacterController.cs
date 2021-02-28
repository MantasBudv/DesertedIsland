using UnityEngine.EventSystems;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float MOVE_SPEED = 5f;
    public Rigidbody2D rb;
    private Vector2 moveDir;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        GetInputs();
    }
    void FixedUpdate() 
    {
        Move();
    }

    void GetInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rb.velocity = moveDir * MOVE_SPEED;
    }
}
