using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{
    public Text WinnerText;
    public static string winner;
    // Start is called before the first frame update
    void Start()
    {
        //winner = "TEST";
        WinnerText.text = winner + " WINS!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
