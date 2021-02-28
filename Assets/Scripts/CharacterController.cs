using UnityEngine.EventSystems;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private const float MOVE_SPEED = 5f;
    private Rigidbody2D rigidbody2D;
    private Vector3 moveDir;

    private void Awake() 
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }

        moveDir = new Vector3(moveX, moveY).normalized;
    }
    void FixedUpdate() 
    {
        rigidbody2D.velocity = moveDir * MOVE_SPEED;
    }
}
