using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFootsteps : MonoBehaviour
{

    Rigidbody2D rb;
    public GameObject footstep;
    Vector3 offset = new Vector3(0,0.2f);

    float StepRate = 0.2f;
    float NextStep;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((rb.velocity != Vector2.zero) && (Time.time > NextStep))
        {
            NextStep = Time.time + StepRate;
            Debug.Log("Walking");
            
            Instantiate(footstep, gameObject.transform.position - offset, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("Footstep");

        }

    }


}
