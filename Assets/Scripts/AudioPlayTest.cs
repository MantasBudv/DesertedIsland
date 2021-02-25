using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayTest : MonoBehaviour
{
    public static AudioPlayTest instance;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("GameLoop");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
