using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode lungeKey; // no function yet
    public KeyCode boostKey; // no function yet
    //public float runSpeed; // use this for individual run speeds for the actual game
    //public float lungeSpeed; // same
    public bool hasBall;
    public bool isLunging;
    public float pointsGained;
    public Text Player1Points;
    public Text Player2Points;
    public GameObject GameBall;

    private float pointsTotal = 20; // point cap for win condition
    private float runSpeed = 10; // just for testing
    private float lungeSpeed = 15; // same
    private float lungeDuration = 0.015f;
    private float lungeCooldown = 0;
    //private Vector3 lungePoint;
    private bool canMove;
    public float stunCooldown = 0;
    private float verticalSpeed;
    private float horizontalSpeed;
    private float verticalInput;
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        pointsGained = 0;
        Ball ballScript = GameBall.GetComponent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "Player 1")
        {
            Player1Points.text = "Points: " + (pointsGained).ToString() + "/" + pointsTotal + "\nLunge cooldown: " + lungeCooldown; // add/remove "/XXX" depending on speed
        }
        if (gameObject.name == "Player 2")
        {
            Player2Points.text = "Points: " + (pointsGained).ToString() + "/" + pointsTotal + "\nLunge cooldown: " + lungeCooldown;
        }
        Vector3 newPosition = this.transform.position;
        verticalSpeed = 0;
        horizontalSpeed = 0;
        if (canMove == true)
        {
            //verticalInput = Input.GetAxis("Vertical");
            //horizontalInput = Input.GetAxis("Horizontal");
            //verticalSpeed = Input.GetAxisRaw("Vertical") * runSpeed;
            //horizontalSpeed = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (isLunging == false)
            {
                if (Input.GetKey(upKey)) // move up
                {
                    verticalSpeed += runSpeed * Time.deltaTime;
                }
                if (Input.GetKey(downKey)) // move down
                {
                    verticalSpeed -= runSpeed * Time.deltaTime;
                }
                if (Input.GetKey(leftKey)) // move left
                {
                    horizontalSpeed -= runSpeed * Time.deltaTime;
                }
                if (Input.GetKey(rightKey)) // move right
                {
                    horizontalSpeed += runSpeed * Time.deltaTime;
                }
                if ((verticalSpeed != 0) || (horizontalSpeed != 0))
                {
                    this.transform.forward = new Vector3(-verticalSpeed, 0, horizontalSpeed); //looks in the direction of movement
                }
                if ((verticalSpeed != 0) && (horizontalSpeed != 0)) //makes diagonal speed equal to runSpeed
                {
                    verticalSpeed = verticalSpeed * (1 / Mathf.Sqrt(2));
                    horizontalSpeed = horizontalSpeed * (1 / Mathf.Sqrt(2));
                }
            }
            if ((Input.GetKey(lungeKey)) && (lungeCooldown == 0) && (hasBall == false))
            {
                StartCoroutine(Lunge());
                canMove = true;
                lungeCooldown = 3;
            }
            if (lungeCooldown > 0)
            {
                lungeCooldown -= Time.deltaTime;
                Debug.Log("Lunge cooldown: " + lungeCooldown);
            }
            else if (lungeCooldown < 0)
            {
                lungeCooldown = 0;
            }
            if (stunCooldown > 0)
            {
                canMove = false;
                stunCooldown -= Time.deltaTime;
                Debug.Log("Stun cooldown: " + stunCooldown);
            }
            else if (stunCooldown < 0)
            {
                stunCooldown = 0;
                canMove = true;
            }
        }
        newPosition.x += horizontalSpeed;
        newPosition.z += verticalSpeed;
        //newPosition = new Vector3(horizontalInput, 0, verticalInput);
        this.transform.position = newPosition;

        if (hasBall == true)
        {
            pointsGained += Time.deltaTime; // change to deltaTime to get rid of inconsistency
            if (pointsGained > pointsTotal)
            {
                pointsGained = pointsTotal;
            }
            if (pointsGained == pointsTotal) // turn into coroutine?
            {
                SceneManager.LoadScene("WinScreen");
                Debug.Log(this.name + " wins");
                //SceneManager.LoadScene("SampleScene");
            }
        }
    }

    IEnumerator Lunge()
    {
        canMove = false;
        isLunging = true;
        float t = 0;
        Vector3 startPosition = transform.position;

        verticalSpeed = -this.transform.forward.x;
        horizontalSpeed = this.transform.forward.z;
        Vector3 targetPosition = this.transform.position + (new Vector3(horizontalSpeed, 0, verticalSpeed) * lungeSpeed);

        while (t < lungeDuration)
        {
            t += Time.deltaTime;
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, t/lungeDuration);
            Debug.Log("Time elapsed: " + t);
            yield return null;
        }
        isLunging = false;
        //this.transform.position = targetPosition;
    }

    void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.tag == "Ball"))// && (hasBall == false))
        {
                hasBall = true;
                Debug.Log(this.name + " grabbed stationary ball");
        }
        else if ((other.gameObject.tag == "Player") && (hasBall == false))
        {
            //if (isLunging == true)
            //{
                Ball ballScript = GameBall.GetComponent<Ball>();
                ballScript.ballTransferring = true;
                //other.gameObject.stunCooldown = 1;
                Debug.Log(this.name + " grabbed ball from other player");
            //}
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if ((other.gameObject.tag == "Player1") && (hasBall == false))
    //    {
    //        Ball ballScript = other.gameObject.GetComponent<Ball>();
    //        ballScript.ballTransferring = true;
    //        //other.gameObject.stunCooldown = 1;
    //        Debug.Log(this.name + " grabbed ball from other player");
    //    }
    //}

    //void Lunge2() // old method of doing lunge, only keeping around for reference
    //{
    //    canMove = false;
    //    isLunging = true;
    //    for (float t = 0; t < lungeDuration; t++)
    //    {
    //        //GetComponent<Rigidbody>().velocity = this.transform.forward * lungeSpeed;
    //        verticalSpeed = -this.transform.forward.x;
    //        horizontalSpeed = this.transform.forward.z;
    //        this.transform.position = this.transform.position + (new Vector3(-verticalSpeed, 0, horizontalSpeed) * lungeSpeed);
    //    }
    //    GetComponent<Rigidbody>().velocity = this.transform.forward * 0;
    //    lungeCooldown = 100;
    //    isLunging = false;
    //    canMove = true;
    //}
}