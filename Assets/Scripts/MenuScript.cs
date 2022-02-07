using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public string firstLevelName;


    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelName);
    }

    public void CloseGame()
    {
        Application.Quit();
    
    }

}

