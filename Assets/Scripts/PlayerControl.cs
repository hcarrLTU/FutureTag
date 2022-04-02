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
    public Camera Camera;
    public GameObject PlayerMarker;
    public GameObject GameBall;
    Animator animator;
    public static string winner;

    public ParticleSystem lungeParticleSystem;

    private float pointsTotal = 20; // point cap for win condition
    private float runSpeed = 10; // just for testing
    private float lungeSpeed = 7.5f; // same
    private float lungeDuration = 1f;
    private float lungeCooldown = 0;
    //private Vector3 lungePoint;
    private bool canMove;
    //public bool isRunning;
    //public float stunCooldown = 0;
    public bool stunned;
    private float verticalSpeed;
    private float horizontalSpeed;
    private float verticalInput;
    private float horizontalInput;

    float movementRaycastLength = 1;

    Collider playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        pointsGained = 0;
        Ball ballScript = GameBall.GetComponent<Ball>();
        animator = gameObject.GetComponent<Animator>();
        ParticleSystem ps = GetComponent<ParticleSystem>();
        //playerCollider = GetComponent<Collider>;

        LayerMask MarkerMask = LayerMask.GetMask("InsideWalls");
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
        Vector3 markerPosition = Camera.WorldToScreenPoint(this.transform.position);
        //Debug.Log(markerPosition.ToString());
        //markerPosition.x = markerPosition.x * 750 - 375;
        //markerPosition.y = markerPosition.y * 500 - 250;
        //markerPosition.x = markerPosition.x / Screen.width - Screen.width / 2;
        //markerPosition.y = markerPosition.y / Screen.height - Screen.height / 2;
        //markerPosition.z = 0;
        //PlayerMarker.GetComponent<RectTransform>().localPosition = markerPosition;
        PlayerMarker.GetComponent<RectTransform>().anchoredPosition = markerPosition;

        RaycastHit hit;
        Physics.SphereCast(Camera.transform.position, 0.25f, (this.transform.position - Camera.transform.position).normalized, out hit, (this.transform.position - Camera.transform.position).magnitude, 1 << 6);
        //Debug.Log("SphereCast hit: " + hit.point);
        //Debug.Log("Player at: " + this.transform.position);

        if (hit.transform)//(hit.transform == this.transform)
        {
            PlayerMarker.SetActive(true);
        }
        else
        {
            PlayerMarker.SetActive(false);
        }

        Debug.DrawLine(Camera.transform.position, hit.point);
        Debug.DrawLine(this.transform.position, this.transform.forward, Color.white, 10);

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
                    if (Physics.Raycast(this.transform.position, new Vector3(0, 0, 1), movementRaycastLength))//, 1 << 6 | 7))
                    {
                        verticalSpeed = 0;
                    }
                }
                if (Input.GetKey(downKey)) // move down
                {
                    verticalSpeed -= runSpeed * Time.deltaTime;
                    if (Physics.Raycast(this.transform.position, new Vector3(0, 0, -1), movementRaycastLength))//, 1 << 6 | 7))
                    {
                        verticalSpeed = 0;
                    }
                }
                if (Input.GetKey(leftKey)) // move left
                {
                    horizontalSpeed -= runSpeed * Time.deltaTime;
                    if (Physics.Raycast(this.transform.position, new Vector3(-1, 0, 0), movementRaycastLength))//, 1 << 6 | 7))
                    {
                        horizontalSpeed = 0;
                    }
                }
                if ((Input.GetKey(rightKey))) // move right
                {
                    horizontalSpeed += runSpeed * Time.deltaTime;
                    if (Physics.Raycast(this.transform.position, new Vector3(1, 0, 0), movementRaycastLength))//, 1 << 6 | 7))
                    {
                        horizontalSpeed = 0;
                    }
                }
                if ((verticalSpeed != 0) && (horizontalSpeed != 0)) //makes diagonal speed equal to runSpeed
                {
                    animator.SetBool("isRunning", false);
                    verticalSpeed = verticalSpeed * (1 / Mathf.Sqrt(2));
                    horizontalSpeed = horizontalSpeed * (1 / Mathf.Sqrt(2));
                    Debug.Log("Vertical speed: " + verticalSpeed);
                    Debug.Log("Horizontal speed: " + horizontalSpeed);
                    this.transform.forward = new Vector3(-verticalSpeed, 0, horizontalSpeed); //looks in the direction of movement
                }
                else if ((verticalSpeed != 0) || (horizontalSpeed != 0))
                {
                    animator.SetBool("isRunning", false);
                    Debug.Log("Vertical speed: " + verticalSpeed);
                    Debug.Log("Horizontal speed: " + horizontalSpeed);
                    this.transform.forward = new Vector3(-verticalSpeed, 0, horizontalSpeed); //looks in the direction of movement
                }
                else
                {
                    animator.SetBool("isRunning", true);
                }
            }
            if ((Input.GetKey(lungeKey)) && (lungeCooldown == 0) && (hasBall == false) && (!Physics.SphereCast(this.transform.position, 1, this.transform.forward, out hit, 2)))
            {
                if (!Physics.Raycast(this.transform.position, this.transform.forward, 6*movementRaycastLength))//, 1 << 6 | 7))
                {
                    StartCoroutine(Lunge());
                    canMove = true;
                    lungeCooldown = 3;
                }
            }
            if (lungeCooldown > 0)
            {
                lungeCooldown -= Time.deltaTime;
                //Debug.Log("Lunge cooldown: " + lungeCooldown);
            }
            else if (lungeCooldown < 0)
            {
                lungeCooldown = 0;
            }
            if (stunned == true)
            {
                canMove = false;
                StartCoroutine(Shake(1f, 0.5f));
                //Debug.Log("Stun cooldown: " + stunCooldown);
            }
            //else if (stunCooldown <= 0)
            //{
            //    stunCooldown = 0;
            //    canMove = true;
            //    Debug.Log("Stun over, player can move");
            //}
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
                WinText.winner = this.name;
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

        //particle stuff goes here
        lungeParticleSystem.Play();

        verticalSpeed = -this.transform.forward.x;
        horizontalSpeed = this.transform.forward.z;
        Vector3 targetPosition = this.transform.position + (new Vector3(horizontalSpeed, 0, verticalSpeed) * lungeDuration * lungeSpeed);

        while (t < lungeDuration)
        {
            t += Time.deltaTime;
            //raycast in front of the player to see if theres room
            //this.transform.position = Vector3.Lerp(startPosition, targetPosition, 1/lungeSpeed); //old teleport
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, (t*lungeSpeed)/2);
            //Debug.Log("Time elapsed in lunge: " + t);
            if (this.transform.position == targetPosition)
            {
                //Debug.Log("Reached target position " + t);
                isLunging = false;
                canMove = true;
                yield break;
            }
            yield return null;
        }
        isLunging = false;
        Debug.Log("Test");
        //this.transform.position = targetPosition;
    }

    void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.tag == "Ball"))// && (hasBall == false))
        {
                hasBall = true;
                //Debug.Log(this.name + " grabbed stationary ball");
        }
        else if ((other.gameObject.tag == "Player") && (hasBall == false))
        {
            if (isLunging == true)
            {
                Ball ballScript = GameBall.GetComponent<Ball>();
                ballScript.ballTransferring = true;
                Debug.Log(this.name + " grabbed ball from other player");
            }
        }
        else if ((other.gameObject.tag == "Player") & (hasBall == true)){
            stunned = true;
        }
        //CameraScript Shake = Camera.GetComponent<CameraShake>();
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Debug.Log("Player shaking");
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            //stunCooldown -= 1;
            //Debug.Log("Stun cooldown: " + stunCooldown);
            float x = Random.Range(-1f, 1f) * magnitude;
            //float y = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, transform.localPosition.y, z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = originalPosition;
        stunned = false;
        canMove = true;
        Debug.Log("Stun over, player can move");
        yield break;
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