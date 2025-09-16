using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    public TMP_Text countText;
    public TMP_Text timeText;
    public float startingTime;
    public string min;
    public string sec;
    public static PlayerController instance;

    // --- NEW ---
    public string finalTimeDisplay;   // Final frozen time as string
    public bool hasFinalTime;         // Did we freeze the time yet?

    public GameObject Portal;
    public GameObject WinText;
    public GameObject VictoryCube;

    private Rigidbody rb;
    public int count;
    private bool gameOver;
    private bool hasGrown;

    public AudioClip coinSFX;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        startingTime = Time.time;
        gameOver = false;

        audioSource = GetComponent<AudioSource>();

        Portal.SetActive(false);
        WinText.SetActive(false);
        VictoryCube.SetActive(false);
    }

    private void Update()
    {
        if (gameOver)
            return;

        float timer = Time.time - startingTime;
        min = ((int)timer / 60).ToString();
        sec = (timer % 60).ToString("f0");

        timeText.text = "Time: " + min + ":" + sec;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(movement.x * speed, currentVelocity.y, movement.z * speed);

        rb.linearVelocity = targetVelocity;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Breakable"))
        {
            if (transform.localScale.x >= 1.5f)
            {
                AudioManager.instance.PlaySound(AudioManager.instance.wallBreak);
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("Portal"))
        {
            // Freeze time before leaving
            FreezeFinalTimeIfNeeded();
            SceneManager.LoadScene("LevelTwo");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            audioSource.clip = coinSFX;
            audioSource.Play();
        }

        if (other.gameObject.CompareTag("DeathZone"))
        {
            SceneManager.LoadScene("DeathMenu");
        }

        if (other.CompareTag("Grow"))
        {
            if (transform.localScale.x < 1.0f)
                transform.localScale = Vector3.one * 1.0f;
            else
                transform.localScale = Vector3.one * 1.5f;

            Destroy(other.gameObject);
            return;
        }

        if (other.CompareTag("Shrink"))
        {
            if (transform.localScale.x > 1.0f)
                transform.localScale = Vector3.one * 1.0f;
            else
                transform.localScale = Vector3.one * 0.5f;

            Destroy(other.gameObject);
            return;
        }

        if (other.gameObject.CompareTag("Jump"))
        {
            rb.AddForce(new Vector3(0.0f, 300.0f, 0.0f));
            AudioManager.instance.PlaySound(AudioManager.instance.bouncePad);
            return;
        }

        if (other.gameObject.CompareTag("VictoryCube"))
        {
            // Freeze time before going to WIN scene
            FreezeFinalTimeIfNeeded();
            SceneManager.LoadScene("WIN");
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 10)
        {
            // Freeze time when win condition is reached
            FreezeFinalTimeIfNeeded();

            gameOver = true;
            timeText.color = Color.green;
            Portal.SetActive(true);
            WinText.SetActive(true);
            VictoryCube.SetActive(true);
        }
    }

    // --- NEW: Freeze helper ---
    private void FreezeFinalTimeIfNeeded()
    {
        if (hasFinalTime) return; // Don't overwrite if already frozen

        float t = Time.time - startingTime;
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);

        finalTimeDisplay = $"{minutes:00}:{seconds:00}";
        hasFinalTime = true;
    }
}
