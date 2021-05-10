using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    [SerializeField] Animator anim;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        anim = GameObject.FindGameObjectWithTag("BlackScreen").GetComponent<Animator>();
        anim.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            anim.gameObject.SetActive(true);
            anim.Play("FadeInBlack");
            anim.SetInteger("SceneNum",SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/"+ sceneName +".unity"));
            Debug.Log(SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + sceneName + ".unity"));
            
            //SceneManager.LoadScene(sceneName);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        anim.Play("FadeOutBlack");
        anim.gameObject.SetActive(false);
    }
    
}
