using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigameStarter : MonoBehaviour
{
    [SerializeField] GameObject fishingMinigame;
    [SerializeField] Camera mainCamera;
    public GameObject fishingRod;
    [SerializeField] CharacterController controller;
    private GameObject instance;
    public static bool inMinigame;
    private bool meleeAttack;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        inMinigame = false;
        fishingRod = GameObject.FindGameObjectWithTag("Fishing");
    }
    // Update is called once per frame
    void Update()
    {
        meleeAttack = Input.GetButtonDown("Fire1");
        if (meleeAttack && fishingRod.activeInHierarchy && CharacterController.isNearWater)
        {
            if (!inMinigame)
            {
                StartFishing();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inMinigame)
            {
                StopFishing();
            }
        }
    }
    private void StopFishing()
    {
        controller.enabled = true;
        Destroy(instance);
        inMinigame = false;
    }

    private void StartFishing()
    {
        
        Vector3 pos = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 1.5f, 0);
        instance = Instantiate(fishingMinigame,pos, Quaternion.identity);
        inMinigame = true;
        Invoke("DisableController", 0.1f);
        
    }
    private void DisableController()
    {
        controller.enabled = false;
    }
}
