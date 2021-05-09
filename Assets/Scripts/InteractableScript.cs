using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractableScript : MonoBehaviour
{
    [SerializeField] private GameObject miniGame;
    [SerializeField] private Item itemDrop;
    public GameObject pickaxe;
    
    private GameObject _highlighting;
    private GameObject _outerLayer;
    private GameObject _innerLayer;
    private bool _playerInRange;
    private bool _playerIsInteracting;
    

    private float _timer;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _timer = 0;
        _playerInRange = false;
        _playerIsInteracting = false;
        _highlighting = transform.GetChild(0).gameObject;
        _outerLayer = transform.GetChild(1).gameObject;
        _innerLayer = transform.GetChild(2).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerInRange = true;
            _highlighting.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerInRange = false;
            _highlighting.SetActive(false);
            
        }
    }

    private void Update()
    {
        if (_playerInRange && Input.GetMouseButton(0) && pickaxe.activeInHierarchy)
        {
            if (_playerIsInteracting == true)
            {
                _timer += Time.deltaTime;
                _innerLayer.gameObject.transform.localScale = new Vector3(_timer, _timer, 1);
                if (_timer >= 1)
                {
                    //Instantiate(itemDrop, _rb.position, Quaternion.identity);
                    var Character = GameObject.FindGameObjectWithTag("Player");
                    Character.GetComponent<CharacterController>().GiveXP(80);
                    
                    int amount = Random.Range(2, 5);
                    for (int i = 0; i < amount; i++)
                    {
                        bool wasPickedUp = Inventory.instance.Add(itemDrop);
                    }

                    gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
            else
            {
                _playerIsInteracting = true;
                _timer = 0;
                _outerLayer.SetActive(true);
                _innerLayer.SetActive(true);
                _innerLayer.gameObject.transform.localScale = new Vector3(0, 0, 1);
            }
        }
        else
        {
            _outerLayer.SetActive(false);
            _innerLayer.SetActive(false);
            _playerIsInteracting = false;
        }
    }

    public void LaunchMiniGame()
    {
        miniGame.SetActive(true);
    }
}
