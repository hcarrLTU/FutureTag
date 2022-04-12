using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    public AudioClip blip;

    void Start()
    {
        //AudioSource audioSource = GetComponent<AudioSource>();
    }

    public void CloseGame()
    {
        //AudioSource audioSource = GetComponent<AudioSource>();
        //audioSource.PlayOneShot(blip, 1f);
        Application.Quit();
    }

}
