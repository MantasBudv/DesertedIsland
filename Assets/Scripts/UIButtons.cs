using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            Debug.Log("exiting");
            ExitGame();
        }
    }

}
