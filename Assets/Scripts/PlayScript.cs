using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScript : MonoBehaviour
{
    public AudioClip blip;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene()
    {
        //AudioSource audioSource = GetComponent<AudioSource>();
        //audioSource.PlayOneShot(blip, 1f);
        SceneManager.LoadScene("SampleScene");
    }
}
