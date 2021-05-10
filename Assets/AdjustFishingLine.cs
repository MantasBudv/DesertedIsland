using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustFishingLine : MonoBehaviour
{
    [SerializeField] Transform hook;
    float distance = 5.52f;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(1, (this.transform.position.y - hook.position.y) / distance, 1);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(1, (this.transform.position.y - hook.position.y) / distance, 1);
    }
}
