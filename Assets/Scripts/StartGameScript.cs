using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    public KeyCode P1Up;
    public KeyCode P1Down;
    public KeyCode P1Left;
    public KeyCode P1Right;
    public KeyCode P1Lunge;
    public Text P1ReadyText;
    public bool P1IsReady = false;

    public KeyCode P2Up;
    public KeyCode P2Down;
    public KeyCode P2Left;
    public KeyCode P2Right;
    public KeyCode P2Lunge;
    public Text P2ReadyText;
    public bool P2IsReady = false;

    public Vector3 ButtonPosition;

    AsyncOperation asyncLoad;
    public GameObject LoadingIndicator;
    // Start is called before the first frame update
    void Start()
    {
        ButtonPosition = this.transform.position;
        this.transform.position = new Vector3(0, -2000, 0);

        asyncLoad = SceneManager.LoadSceneAsync("SampleScene");
        asyncLoad.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(P1Lunge))
        {
            P1ReadyText.text = "Player 1 ready!";
            P1IsReady = true;
        }
        if (Input.GetKey(P2Lunge))
        {
            P2ReadyText.text = "Player 2 ready!";
            P2IsReady = true;
        }
        if ((P1IsReady = true) && (P2IsReady == true))
        {
            this.transform.position = ButtonPosition;
        }
        //if ((Input.GetKey(P1Up)) && (Input.GetKey(P1Down)) && (Input.GetKey(P1Left)) && (Input.GetKey(P1Right)) && (Input.GetKey(P1Lunge)))
        //{
        //    P1ReadyText.text = "Player 1 ready!";
        //    P1IsReady = true;
        //}
        //if ((Input.GetKey(P2Up)) && (Input.GetKey(P2Down)) && (Input.GetKey(P2Left)) && (Input.GetKey(P2Right)) && (Input.GetKey(P2Lunge)))
        //{
        //    P1ReadyText.text = "Player 2 ready!";
        //    P2IsReady = true;
        //}
    }

    public void LoadScene()
    {
        LoadingIndicator.SetActive(true);
        //SceneManager.LoadScene("SampleScene");
        asyncLoad.allowSceneActivation = true;
    }
}
