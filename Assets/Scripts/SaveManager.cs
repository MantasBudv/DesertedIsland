using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public SaveData activeSave;
    public bool hasLoaded;
    public AudioMixer audioMixer;
    public GameObject loadingScreen;
    public Slider slider;
    public static float loadprog;
    

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

    /// <summary>
    /// CHANGE THE BUTTONS
    /// </summary>
    void Update()
    {
        if (loadprog == 1)
        {
            LoadValues();
            //loadingScreen.SetActive(false);
            loadprog = 0;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Load();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            DeleteSaveData();
        }
    }

    public void Save()
    {
        instance.activeSave.currentScene = SceneManager.GetActiveScene().name;                      //Current scene
        instance.activeSave.playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;                             //Player position
        instance.activeSave.maxHP = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().maxHealth;           //Player stats
        instance.activeSave.currHP = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().getCurrHP();
        instance.activeSave.maxSTA = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().maxStamina;
        instance.activeSave.currSTA = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().getCurrSTA();
        instance.activeSave.currSkills =
            CharacterController.skills.Copy();
        instance.activeSave.inventory = FindObjectOfType<Inventory>().GetItems();                   //Inventory (need to change how items
        instance.activeSave.invQuant = FindObjectOfType<Inventory>().GetItemsQuant();               //work first)
        audioMixer.GetFloat("Music", out instance.activeSave.musicVol);                             //Music volume
        audioMixer.GetFloat("Sounds", out instance.activeSave.soundVol);                            //Sound volume
        FindObjectOfType<Timer>().GetTime(out instance.activeSave.timer, out instance.activeSave.dayCount); //Time
        instance.activeSave.HotbarLevels = FindObjectOfType<Hotbar>().GetLevels();                  //Hotbar levels

        string dataPath = Application.persistentDataPath;

        var serializer = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(dataPath + "/" + activeSave.saveName + ".save", FileMode.Create);
        serializer.Serialize(stream, activeSave);
        stream.Close();

        Debug.Log(dataPath);

    }

    public void Load()
    {
        string datapath = Application.persistentDataPath;

        if(File.Exists(datapath + "/" + activeSave.saveName + ".save"))
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(datapath + "/" + activeSave.saveName + ".save", FileMode.Open);
            activeSave = serializer.Deserialize(stream) as SaveData;
            Debug.Log(activeSave.inventory);
            stream.Close();

            LoadValues();
            //hasLoaded = true;

            StartCoroutine(LoadScene(instance.activeSave.currentScene));

        }

    }

    public void LoadValues()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = instance.activeSave.playerPosition;
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().LoadStats(instance.activeSave.maxHP, instance.activeSave.currHP,
                                                             instance.activeSave.maxSTA, instance.activeSave.currSTA, instance.activeSave.currSkills);
        FindObjectOfType<Inventory>().LoadInventory(instance.activeSave.inventory, instance.activeSave.invQuant);
        audioMixer.SetFloat("Music", instance.activeSave.musicVol);
        audioMixer.SetFloat("Sounds", instance.activeSave.soundVol);
        FindObjectOfType<Timer>().SetTime(instance.activeSave.timer, instance.activeSave.dayCount);
        FindObjectOfType<Hotbar>().SetLevels(instance.activeSave.HotbarLevels);

        //uibuttons newgame = false;
    }

    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        //loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .09f);
            //slider.value = progress;
            loadprog = progress;

            yield return null;
        }
    }

    public void DeleteSaveData()
    {

    }

}


//-------------SAVE-DATA------------
[System.Serializable]
public class SaveData
{
    public string saveName;

    public Vector2 playerPosition;
    public string currentScene;
    public int currHP, maxHP, currSTA, maxSTA;
    public List<Item> inventory;
    public List<int> invQuant;
    //floor item hash thing list
    public List<int> HotbarLevels;
    public float timer;    
    public int dayCount;
    public float musicVol, soundVol;

    public bool[] currSkills;

}