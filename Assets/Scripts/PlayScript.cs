using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip forward;
    public AudioClip backward;
    public GameObject TitleScreen;
    public GameObject ControlScreen;
    public GameObject CreditsScreen;

    // Start is called before the first frame update
    void Start()
    {
        ControlScreen.SetActive(false);
        //CreditsScreen.SetActive(false);
        //audioSource.PlayOneShot(forward, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene()
    {
        audioSource.PlayOneShot(forward, 1f);
        //SceneManager.LoadScene("SampleScene");
        ControlScreen.SetActive(true);
        TitleScreen.SetActive(false);
    }

    public void Credits()
    {
        audioSource.PlayOneShot(forward, 1f);
        CreditsScreen.SetActive(true);
        TitleScreen.SetActive(false);
    }

    public void BackToMenu()
    {
        audioSource.PlayOneShot(backward, 1f);
        TitleScreen.SetActive(true);
        CreditsScreen.SetActive(false);
    }

    public void SwitchToMenuScene()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
