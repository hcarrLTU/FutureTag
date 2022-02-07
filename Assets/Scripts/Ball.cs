using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;
    public bool ballTransferring;

    Collider ballCollider;

    // Start is called before the first frame update
    void Start()
    {
        ballTransferring = false;
        ballCollider = GetComponent<Collider>();
        //Player1 = GameObject.FindWithTag("Player 1");
        //Player2 = GameObject.FindWithTag("Player 2");
        //Player3 = GameObject.FindWithTag("Player 3");
        //Player4 = GameObject.FindWithTag("Player 4");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl player1Script = Player1.GetComponent<PlayerControl>();
        PlayerControl player2Script = Player2.GetComponent<PlayerControl>();
        //PlayerControl player3Script = Player3.GetComponent<PlayerControl>();
        //PlayerControl player4Script = Player4.GetComponent<PlayerControl>();

        Vector3 Player1Location = Player1.transform.position;
        Vector3 Player2Location = Player2.transform.position;

        if (ballTransferring == true)
        {
            Debug.Log("Ball transferred...");
            if ((player1Script.hasBall == true) && (player2Script.hasBall == false))
            {
                player2Script.hasBall = true;
                Debug.Log("...to player 2");
                player1Script.hasBall = false;
                ballTransferring = false;
            }
            else if ((player1Script.hasBall == false) && (player2Script.hasBall == true))
            {
                player1Script.hasBall = true;
                Debug.Log("...to player 1");
                player2Script.hasBall = false;
                ballTransferring = false;
            }
        }

        if (player1Script.hasBall == true)
        {
            ballCollider.enabled = false;
            this.transform.position = new Vector3(Player1.transform.position.x, Player1.transform.position.y + 1, Player1.transform.position.z);
            if (player2Script.hasBall == true)
            {
                player2Script.hasBall = false; // currently favors player 1 if both players get the ball at start at the exact same frame. needs to be resolved in a more balanced way
            }
        }

        if (player2Script.hasBall == true)
        {
            ballCollider.enabled = false;
            this.transform.position = new Vector3(Player2.transform.position.x, Player2.transform.position.y + 1, Player2.transform.position.z);
        }

        //if (player1Script.hasBall == true) // if player 1 has the ball...
        //{
        //    this.transform.position = new Vector3(Player1.transform.position.x, Player1.transform.position.y + 1, Player1.transform.position.z); // ...the ball is always on top of player 1
        //    ////ballCollider.enabled = false;
        //    //if (ballTransferring == true)
        //    //{
        //    //    player1Script.hasBall = false; // ...player 1 no longer has the ball
        //    //    player2Script.hasBall = true;
        //    //    this.transform.position = new Vector3(Player2.transform.position.x, Player2.transform.position.y + 1, Player2.transform.position.z);
        //    //    Debug.Log("Ball transferred to Player 2");
        //    //}
        //    //ballTransferring = false;
        //}
        //else if (player2Script.hasBall == true) // if player 2 has the ball...
        //{
        //    this.transform.position = new Vector3(Player2.transform.position.x, Player2.transform.position.y + 1, Player2.transform.position.z); // ...the ball is always on top of player 2
        //    ////ballCollider.enabled = false;
        //    //if (ballTransferring == true)
        //    //{
        //    //    player2Script.hasBall = false; // ...player 2 no longer has the ball
        //    //    player1Script.hasBall = true;
        //    //    this.transform.position = new Vector3(Player1.transform.position.x, Player1.transform.position.y + 1, Player1.transform.position.z);
        //    //    Debug.Log("Ball transferred to Player 1");
        //    //}
        //    //ballTransferring = false;
        //}
        //if (ballTransferring == true) // if the ball is taken... // this part is just the above flipped around, i don't think it works any differently
        //{
        //    ballTransferring = false;
        //    if (player1Script.hasBall == true) // ...and player 1 has it...
        //    {
        //        player1Script.hasBall = false; // ...player 1 no longer has the ball
        //        player2Script.hasBall = true;
        //        ballCollider.enabled = !ballCollider.enabled;
        //        Debug.Log("Ball transferred to Player 2");
        //    }
        //    else if (player2Script.hasBall == true) // ...and player 2 has it...
        //    {
        //        player1Script.hasBall = true;
        //        player2Script.hasBall = false; // ...player 2 no longer has the ball
        //        ballCollider.enabled = !ballCollider.enabled;
        //        Debug.Log("Ball transferred to Player 1");
        //    }
        //}
    }
}