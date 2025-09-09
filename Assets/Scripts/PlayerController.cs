using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //needed to restart the game when the player enters the death zone (trigger event)
using TMPro;

public class PlayerController : MonoBehaviour
{

    //These public variables are initialized in the Inspector
    private float speed = 5;
    public TMP_Text countText;
    public TMP_Text timeText;  //  variable to display the timer text in Unity
    public float startingTime;  // variable to hold the game's starting time
    public string min;
    public string sec;

    // References to game objects
    public GameObject Portal;
    public GameObject WinText;
    public GameObject VictoryCube; // My cheap way of opening the WIN scene when Lvl2 is completed

    //These private variables are initialized in the Start
    private Rigidbody rb;
    public int count;
    private bool gameOver; //  bool to define game state on or off.
    private bool hasGrown;

    // Audio
    public AudioClip coinSFX;
    private AudioSource audioSource;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        startingTime = Time.time;
        gameOver = false;

        audioSource = GetComponent<AudioSource>();  // access the audio source component of player

        Portal.SetActive(false);
        WinText.SetActive(false);
        VictoryCube.SetActive(false);
    }
    private void Update()
    {
        if (gameOver) // condition that the game is NOT over; returns the false value
            return;
        float timer = Time.time - startingTime;     // local variable to updated time
        min = ((int)timer / 60).ToString();     // calculates minutes
        sec = (timer % 60).ToString("f0");      // calculates seconds

        timeText.text = "Elapsed Time: " + min + ":" + sec;     // update UI time text
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Create movement vector
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // Preserve current Y velocity for gravity
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(movement.x * speed, currentVelocity.y, movement.z * speed);

        // Snap instantly for snappy controls
        rb.linearVelocity = targetVelocity;


        // Old movement code in case we want to revert

        // float moveHorizontal = Input.GetAxis("Horizontal");
        // float moveVertical = Input.GetAxist("Vertical");
        // Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // rb.AddForce(movement * speed);
    }

    
    private void OnCollisionEnter(Collision other)
        // This is for breakable objects or anything else we need
        // Specifically for it you need to collide with something, because triggers don't have physics
    {
        if (other.gameObject.CompareTag("Breakable"))
        {
            if (transform.localScale.x >= 1.5f)
            {
                AudioManager.instance.PlaySound(AudioManager.instance.wallBreak);
                Destroy(other.gameObject);
            }
        }

        // Portal logic for crossing into the next level
        if (other.gameObject.CompareTag("Portal"))
        {
            // Change logic later to go to next scene.
            // Maybe an if statement to check what level the player is on and direct them accordingly
            WinText.SetActive(false);
            SceneManager.LoadScene("LevelTwo");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //This event/function handles trigger events (collsion between a game object with a rigid body)

        if (other.gameObject.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            //PLAY SOUND EFFECT
            audioSource.clip = coinSFX;
            audioSource.Play();

        }
        // string currentSceneName = SceneManager.GetActiveScene().name;
        if (other.gameObject.CompareTag("DeathZone"))
        {
            SceneManager.LoadScene("DeathMenu");
        }

        // Grow Pad
        if (other.CompareTag("Grow"))
        {
            // Small to Normal, otherwise Normal to Big
            if (transform.localScale.x < 1.0f)
            {
                transform.localScale = Vector3.one * 1.0f;
            }
            else
            {
                transform.localScale = Vector3.one * 1.5f;
            }
            Destroy(other.gameObject);
            return;
        }

        // Shrink Pad
        if (other.CompareTag("Shrink"))
        {
            // Big to Normal, otherwise Normal to Small
            if (transform.localScale.x > 1.0f)
            {
                transform.localScale = Vector3.one * 1.0f;
            }
            else
            {
                transform.localScale = Vector3.one * 0.5f;
            }
            Destroy(other.gameObject);
            return;
        }


        // Bounce Pad
        if (other.gameObject.CompareTag("Jump"))
        {
            rb.AddForce(new Vector3(0.0f, 300.0f, 0.0f));
            AudioManager.instance.PlaySound(AudioManager.instance.bouncePad); // Sound doesn't work yet
            return;
        }

        // Victory Cube
        if (other.gameObject.CompareTag("VictoryCube"))
        {
            SceneManager.LoadScene("WIN");
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 10)
        {
            gameOver = true; // returns true value to signal game is over
            timeText.color = Color.green;  // changes timer's color
            Portal.SetActive(true);
            WinText.SetActive(true);
            VictoryCube.SetActive(true);
        }
    }
}