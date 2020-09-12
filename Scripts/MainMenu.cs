using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
     void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
    }

    public void QuitGame ()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void NextScene()
    {
        SceneManager.LoadSceneAsync(1);
    }

}
