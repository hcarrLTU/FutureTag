using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;
    public Camera Camera;
    public Text CountdownText;
    public int countdownLength = 1;
    public int secondsLeft = 4;
    public float countdownCooldown;
    public bool countdownComplete;
    public bool ballTransferring;

    public ParticleSystem ballParticleSystem;

    Collider ballCollider;

    public AudioClip countdown1;
    public AudioClip countdown2;

    // Start is called before the first frame update
    void Start()
    {
        countdownCooldown = countdownLength;
        countdownComplete = false;
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
        CameraShake cameraScript = Camera.GetComponent<CameraShake>();
        PlayerControl player1Script = Player1.GetComponent<PlayerControl>();
        PlayerControl player2Script = Player2.GetComponent<PlayerControl>();
        //PlayerControl player3Script = Player3.GetComponent<PlayerControl>();
        //PlayerControl player4Script = Player4.GetComponent<PlayerControl>();

        AudioSource audioSource = GetComponent<AudioSource>();

        Vector3 Player1Location = Player1.transform.position;
        Vector3 Player2Location = Player2.transform.position;

        //if (countdownCooldown > 0)
        //{
        //    countdownCooldown -= Time.deltaTime;
        //    if ((countdownCooldown == 3 * (countdownLength) / 4) || (countdownCooldown == 2 * (countdownLength) / 4 || (countdownCooldown == (countdownLength) / 4)))
        //    {
        //        audioSource.PlayOneShot(countdown1, 1f);
        //    }
        //}
        //else if (countdownCooldown < 0)
        //{
        //    countdownCooldown = 0;
        //}
        //else if (countdownComplete == false)
        //{
        //    //countdownCooldown = 0;
        //    audioSource.PlayOneShot(countdown2, 1f);
        //    player1Script.canMove = true;
        //    player2Script.canMove = true;
        //    countdownComplete = true;
        //}

        if (secondsLeft == 4)
        {
            CountdownText.text = 3.ToString();
        }
        else if (secondsLeft == 0)
        {
            CountdownText.text = "";
        }
        else
        {
            CountdownText.text = (secondsLeft).ToString();
        }

        if ((secondsLeft == 0) && (countdownComplete == false))
        {
            countdownComplete = true;
            audioSource.PlayOneShot(countdown2, 1f);
            player1Script.canMove = true;
            player2Script.canMove = true;
        }
        else if ((countdownCooldown <= 0) && (secondsLeft != 0) && (countdownComplete == false))
        {
            countdownCooldown = countdownLength;
            secondsLeft -= 1;
            audioSource.PlayOneShot(countdown1, 1f);
        }
        else if ((countdownCooldown > 0) && (secondsLeft != 0) && (countdownComplete == false))
        {
            countdownCooldown -= Time.deltaTime;
        }

        if (ballTransferring == true)
        {
            ballParticleSystem.Play();

            if ((player1Script.hasBall == true) && (player2Script.hasBall == false))
            {
                cameraScript.shaking = true;
                player2Script.hasBall = true;
                //Debug.Log("...to player 2");
                player1Script.hasBall = false;
                ballTransferring = false;
            }
            else if ((player1Script.hasBall == false) && (player2Script.hasBall == true))
            {
                cameraScript.shaking = true;
                player1Script.hasBall = true;
                //Debug.Log("...to player 1");
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