using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    [SerializeField] private int spawnPoint;
    [SerializeField] Animator anim;


    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        anim = GameObject.FindGameObjectWithTag("BlackScreen").GetComponent<Animator>();
        Invoke("Delay", 5);
            
    }

    

    void Delay()
    {
        //anim.gameObject.SetActive(false);
        return;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            anim.gameObject.SetActive(true);
            FindObjectOfType<CharacterController>().SetSpawnPoint(spawnPoint);
            anim.Play("FadeInBlack");
            anim.SetInteger("SceneNum",SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/"+ sceneName +".unity"));
            
            //SceneManager.LoadScene(sceneName);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        anim = GameObject.FindGameObjectWithTag("BlackScreen").GetComponent<Animator>();
        Time.timeScale = 1f;
        Debug.Log(anim);
        anim.gameObject.SetActive(true);
        anim.Play("FadeOutBlack");
        //anim.gameObject.SetActive(false);
    }
    
}
