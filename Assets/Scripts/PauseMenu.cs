using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public KeyCode pauseKey;

    void Update()
    {
        //if (Input.GetKey(pauseKey))
        //{
        //    pauseMenu.SetActive(true);
        //    Time.timeScale = 0f;

        //}
        bool changing = false;
        if ((Input.GetMouseButtonDown(0)))
        {
            if (pauseMenu.activeSelf == false)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                changing = true;
            }
            else if ((pauseMenu.activeSelf == true) && (changing == false))
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
            }
        }

    }


   public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void Resume ()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }
}
