using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{
    public Text WinnerText;
    public static string winner;

    public GameObject P1Background;
    public GameObject P2Background;
    // Start is called before the first frame update
    void Start()
    {
        //winner = "TEST";
        WinnerText.text = winner + " WINS!";
        if (winner == "Player 1")
        {
            P1Background.SetActive(true);
            P2Background.SetActive(false);
        }
        else
        {
            P1Background.SetActive(false);
            P2Background.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
