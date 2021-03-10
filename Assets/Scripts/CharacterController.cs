using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public float BASE_MOVE_SPEED = 3.5f;
    public Rigidbody2D rb;
    public float movementSpeed;
    public Animator animator;
    private Vector2 moveDir;
    public GameObject gameOver;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        GetInputs();
        Animate();
    }
    void FixedUpdate() 
    {
        Move();
    }

    void GetInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        moveDir = new Vector2(moveX, moveY);
        movementSpeed = Mathf.Clamp(moveDir.magnitude, 0.0f, 1.0f);
        moveDir.Normalize();
    }

    void Move()
    {
        rb.velocity = moveDir * movementSpeed * BASE_MOVE_SPEED;
    }

    void Animate()
    {
        if (moveDir != Vector2.zero)
        {
            animator.SetFloat("Horizontal", moveDir.x);
            animator.SetFloat("Vertical", moveDir.y);
        }
       
        animator.SetFloat("Speed", movementSpeed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Skeleton")
        {
            Time.timeScale = 0f;
            gameObject.SetActive(false);
            Debug.Log("You died");
            gameOver.SetActive(true);
        }
    }
    
}
