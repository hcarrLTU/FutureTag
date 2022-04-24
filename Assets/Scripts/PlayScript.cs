using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScript : MonoBehaviour
{
    public AudioClip blip;
    public GameObject TitleScreen;
    public GameObject ControlScreen;

    // Start is called before the first frame update
    void Start()
    {
        ControlScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        //audioSource.PlayOneShot(blip, 1f);
        //SceneManager.LoadScene("SampleScene");
        TitleScreen.SetActive(false);
        ControlScreen.SetActive(true);
    }
}
