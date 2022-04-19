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
    public RectTransform PointMeter;
    public RectTransform LungeMeter;
    public Camera Camera;
    public GameObject PlayerMarker;
    public GameObject GameBall;
    public GameObject OtherPlayer;
    Animator animator;
    public static string winner;

    public ParticleSystem lungeParticleSystem;

    private float pointsTotal = 20; // point cap for win condition
    private float runSpeed = 10; // just for testing
    private float lungeSpeed = 7.5f; // same
    private float lungeDuration = 1f;
    private float lungeCooldown = 0;
    //private Vector3 lungePoint;
    public bool canMove;
    public bool isRunning;
    public bool isStunned;
    public float stunCooldown = 0;
    public float footstepCooldown = 0;
    private float verticalSpeed;
    private float horizontalSpeed;
    private float verticalInput;
    private float horizontalInput;
    private float movementRaycastLength = 1f;

    public AudioClip footstep;
    public AudioClip stun;
    public AudioClip lunge;
    public AudioClip nearwin;

    // Start is called before the first frame update
    void Start()
    {
        canMove = false;
        //animator.SetBool("isRunning", true);
        pointsGained = 0;
        lungeCooldown = 0;
        footstepCooldown = 0;

        Ball ballScript = GameBall.GetComponent<Ball>();
        animator = gameObject.GetComponent<Animator>();
        ParticleSystem ps = GetComponent<ParticleSystem>();
        //AudioSource audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        if (gameObject.name == "Player 1")
        {
            Player1Points.text = "Points: " + (pointsGained).ToString() + "/" + pointsTotal + "\nLunge cooldown: " + lungeCooldown; // add/remove "/XXX" depending on speed
        }
        if (gameObject.name == "Player 2")
        {
            Player2Points.text = "Points: " + (pointsGained).ToString() + "/" + pointsTotal + "\nLunge cooldown: " + lungeCooldown;
        }
        PointMeter.sizeDelta = new Vector2(1.25f, (pointsGained / pointsTotal) * 2.5f);
        PointMeter.anchoredPosition = new Vector2(0, -1.25f + (pointsGained / pointsTotal) * 1.25f);
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
        //Physics.SphereCast(Camera.transform.position, 0.1f, this.transform.position - Camera.transform.position, out hit, 1 << 7);
        Physics.Raycast(Camera.transform.position, this.transform.position - Camera.transform.position, out hit, 1 << 7);
        //Debug.Log("SphereCast hit: " + hit.point);
        //Debug.Log("Player at: " + this.transform.position);

        if (hit.transform == this.transform)
        {
            PlayerMarker.SetActive(false);
        }
        else
        {
            PlayerMarker.SetActive(true);
        }

        //if (Physics.SphereCast(Camera.transform.position, 1, this.transform.position - Camera.transform.position, out hit, 1 << 7))
        //{
        //    PlayerMarker.SetActive(true);
        //}
        //else
        //{
        //    PlayerMarker.SetActive(false);
        //}

        Vector3 newPosition = this.transform.position;
        verticalSpeed = 0;
        horizontalSpeed = 0;
        animator.SetBool("isRunning", true);
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
                    if (Physics.Raycast(this.transform.position, new Vector3(0, 0, 1), movementRaycastLength, 1 << 6 | 7))
                    {
                        verticalSpeed = 0;
                    }
                    else
                    {
                        verticalSpeed += runSpeed * Time.deltaTime;
                    }
                }
                if (Input.GetKey(downKey)) // move down
                {
                    if (Physics.Raycast(this.transform.position, new Vector3(0, 0, -1), movementRaycastLength, 1 << 6 | 7))
                    {
                        verticalSpeed = 0;
                    }
                    else
                    {
                        verticalSpeed -= runSpeed * Time.deltaTime;
                    }
                }
                if (Input.GetKey(leftKey)) // move left
                {
                    if (Physics.Raycast(this.transform.position, new Vector3(-1, 0, 0), movementRaycastLength, 1 << 6 | 7))
                    {
                        horizontalSpeed = 0;
                    }
                    else
                    {
                        horizontalSpeed -= runSpeed * Time.deltaTime;
                    }
                }
                if (Input.GetKey(rightKey)) // move right
                {
                    if (Physics.Raycast(this.transform.position, new Vector3(1, 0, 0), movementRaycastLength, 1 << 6 | 7))
                    {
                        horizontalSpeed = 0;
                    }
                    else
                    {
                        horizontalSpeed += runSpeed * Time.deltaTime;
                    }
                }
                if (!(Input.GetKey(upKey)) && !(Input.GetKey(downKey)))
                {
                    verticalSpeed = 0;
                    //newPosition.z = this.transform.position.z;
                }
                if (!(Input.GetKey(leftKey)) && !(Input.GetKey(rightKey)))
                {
                    horizontalSpeed = 0;
                    //newPosition.x = this.transform.position.x;
                }
                if ((verticalSpeed != 0) && (horizontalSpeed != 0)) //makes diagonal speed equal to runSpeed
                {
                    if (footstepCooldown == 0)
                    {
                        audioSource.PlayOneShot(footstep, 1f);
                        footstepCooldown = 60;
                    }
                    animator.SetBool("isRunning", false);
                    verticalSpeed = verticalSpeed * (1 / Mathf.Sqrt(2));
                    horizontalSpeed = horizontalSpeed * (1 / Mathf.Sqrt(2));
                    this.transform.forward = new Vector3(-verticalSpeed, 0, horizontalSpeed); //looks in the direction of movement
                }
                else if ((verticalSpeed != 0) || (horizontalSpeed != 0))
                {
                    if (footstepCooldown == 0)
                    {
                        audioSource.PlayOneShot(footstep, 1f);
                        footstepCooldown = 60;
                    }
                    animator.SetBool("isRunning", false);
                    this.transform.forward = new Vector3(-verticalSpeed, 0, horizontalSpeed); //looks in the direction of movement
                }
                else
                {
                    animator.SetBool("isRunning", true);
                }
            }

            //Debug.Log(this.name + " vertical speed: " + verticalSpeed);
            //Debug.Log(this.name + " horizontal speed: " + horizontalSpeed);

            if ((Input.GetKey(lungeKey)) && (lungeCooldown == 0) && (hasBall == false))// && (!Physics.SphereCast(this.transform.position, 1, this.transform.forward, out hit, 1 << 6 | 7)))
            {
                //if (!Physics.Raycast(this.transform.position, this.transform.forward, movementRaycastLength, 1 << 6 | 7))
                //{
                    StartCoroutine(Lunge());
                    canMove = true;
                    lungeCooldown = 3;
                //}
                //if ((Input.GetKey(lungeKey)) && (lungeCooldown == 0) && (hasBall == false))
                //{
                //    StartCoroutine(Lunge());
                //    canMove = true;
                //    lungeCooldown = 3;
                //}
            }
            if (lungeCooldown > 0)
            {
                LungeMeter.sizeDelta = new Vector2(1.25f, ((3 - lungeCooldown) / 3) * 2.5f);
                LungeMeter.anchoredPosition = new Vector2(0, -1.25f + ((3 - lungeCooldown) / 3) * 1.25f);
                lungeCooldown -= Time.deltaTime;
                //Debug.Log("Lunge cooldown: " + lungeCooldown);
            }
            else if (lungeCooldown < 0)
            {
                lungeCooldown = 0;
            }

            if (isStunned == true)
            {
                StartCoroutine(Shake(1f, 1f));
                //Debug.Log("Stun cooldown: " + stunCooldown);
                //stunCooldown -= 1;
                //StartCoroutine(Shake(0.01f, 0.1f));
            }
            //if (stunCooldown <= 0)
            //{
            //    isStunned = false;
            //    stunCooldown = 0;
            //    canMove = true;
            //}
        }
            newPosition.x += horizontalSpeed;
            newPosition.z += verticalSpeed;
            //newPosition = new Vector3(horizontalInput, 0, verticalInput);
            this.transform.position = newPosition;

        if (footstepCooldown > 0)
        {
            footstepCooldown -= 1;
        }
        if (footstepCooldown <= 0)
        {
            footstepCooldown = 0;
        }

        if (hasBall == true)
            {
                animator.SetBool("hasBall", true);
                pointsGained += Time.deltaTime; // change to deltaTime to get rid of inconsistency
            if (pointsGained >= (0.9f * pointsTotal))
            {
                audioSource.PlayOneShot(nearwin, 0.2f);
            }
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
            if ((hasBall == false) && (pointsGained >= (0.9f * pointsTotal)))
        {
            pointsGained = 0.8f * pointsTotal;
            animator.SetBool("hasBall", false);
        }
        }

        IEnumerator Lunge()
        {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(lunge, 1f);
            canMove = false;
            isLunging = true;
        animator.SetBool("isLunging", true);
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
            //this.transform.position = Vector3.Lerp(startPosition, targetPosition, 1/lungeSpeed); //old teleport
            Vector3 lastPosition = this.transform.position;
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, (t * lungeSpeed) / 2);
            //Debug.Log("Time elapsed: " + t);
                if (Physics.Raycast(this.transform.position, this.transform.forward, 1f, 1 << 6 | 7)) //lungeSpeed/2 or movementRaycastLength or (targetPosition - this.transform.position).magnitude / (lungeSpeed/2)
            {
                targetPosition = this.transform.position;
                Debug.Log("Reached target position early. Travelled " + (this.transform.position - lastPosition).magnitude);
            }
                if (this.transform.position == targetPosition)
                {
                    //Debug.Log("Reached target position " + t);
                    isLunging = false;
                    animator.SetBool("isLunging", false);
                    canMove = true;
                    yield break;
                }
                yield return null;
            }
            isLunging = false;
        animator.SetBool("isLunging", false);
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

                    PlayerControl otherPlayerScript = OtherPlayer.GetComponent<PlayerControl>();
                    otherPlayerScript.isStunned = true;

                    //Debug.Log(this.name + " grabbed ball from other player");
                }
            }
            //else if ((other.gameObject.tag == "Player") & (hasBall == true)){
            //    if (other.gameObject.GetComponent<PlayerControl>.isLunging == true)
            //    {
            //        stunned = true;
            //    }
            //}
            else if ((other.gameObject.tag == "Player") & (hasBall == true))
            {
                //stunCooldown = 20;
            }
            //CameraScript Shake = Camera.GetComponent<CameraShake>();
        }

        public IEnumerator Shake(float duration, float magnitude)
        {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(stun, 1f);
        animator.SetBool("isStunned", true);
            canMove = false;
            //Debug.Log("Player shaking");
            Vector3 originalPosition = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-0.1f, 0.1f) * magnitude;
            //float y = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-0.1f, 0.1f) * magnitude;

                //transform.position = new Vector3(transform.localPosition.x + x, transform.localPosition.y, transform.localPosition.z + z);
                elapsed += Time.deltaTime;
                yield return 0;
            }
            transform.position = originalPosition;
            isStunned = false;
            canMove = true;
            animator.SetBool("isStunned", false);
            //animator.SetBool("isRunning", true);
            //Debug.Log("Stun over, player can move");
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