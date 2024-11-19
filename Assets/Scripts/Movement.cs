using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    public Renderer playerRenderer;
    public float movementSpeed;
    public bool hasCane = false;

    public TMP_Text needCaneText;
    public TMP_Text caneText;
    public TMP_Text winText;
    public TMP_Text loseText;
    public Button retryButton;

    public AudioSource audioSource;  // Reference to AudioSource component
    public AudioClip walkingSound;   // Walking sound effect
    public AudioClip canePickupSound; // Cane pick up sound effect
    public AudioClip winSound;        // Win sound effect
    public AudioClip loseSound;       // Lose sound effect
    public AudioClip reminderSound;   // Reminder sound effect

    private bool isWalking = false;   // Keep track of whether the player is walking


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerRenderer.enabled)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * movementSpeed);

            if (move.magnitude > 0 && !isWalking)
            {
                // Player has started moving
                isWalking = true;
                audioSource.clip = walkingSound;
                audioSource.loop = true;  // Loop the walking sound while the player is moving
                audioSource.Play();
            }
            else if (move.magnitude == 0 && isWalking)
            {
                // Player has stopped moving
                isWalking = false;
                audioSource.Stop();
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.CompareTag("Cane") && !hasCane)
        {
            hasCane = true;
            StartCoroutine(DisplayText(caneText, 3f));

            // Play some sort of pick up sound
            audioSource.PlayOneShot(canePickupSound);
            Debug.Log("Pickup Cane");
        }
        else if(collision.transform.CompareTag("CampSite"))
        {
            if(hasCane)
            {
                StartCoroutine(DisplayText(winText, 60f));
                retryButton.gameObject.SetActive(true);
                // Play a win sound
                audioSource.PlayOneShot(winSound);
                Debug.Log("Win!");
            }
            else
            {
                StartCoroutine(DisplayText(needCaneText, 3f));
                // Play some beep sound or something
                audioSource.PlayOneShot(reminderSound);
                Debug.Log("Get cane first");
            }
        }
        else if(collision.transform.CompareTag("Bear"))
        {
            StartCoroutine(DisplayText(loseText, 60f));
            retryButton.gameObject.SetActive(true);
            playerRenderer.enabled = false;

            // Maybe play a sound of bear or player dying lol
            audioSource.PlayOneShot(loseSound);
            Debug.Log("You died");
        }
    }

    IEnumerator DisplayText(TMP_Text textToDisplay, float timeToDisplay)
    {
        textToDisplay.gameObject.SetActive(true);

        yield return new WaitForSeconds(timeToDisplay);

        textToDisplay.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
