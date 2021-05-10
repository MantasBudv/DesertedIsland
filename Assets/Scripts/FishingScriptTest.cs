using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingScriptTest : MonoBehaviour
{
    [SerializeField] GameObject fishingMinigame;
    [SerializeField] Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        fishingMinigame.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
    }
}
