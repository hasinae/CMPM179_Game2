using UnityEngine;
using System.Collections.Generic;

public class WalkSounds : MonoBehaviour
{
    [SerializeField] List<Collider> grassPaths = new List<Collider>();

    public AudioSource audioSource;  // Reference to AudioSource component
    public AudioClip bearSound;       // Bear sound effect
    public AudioClip grassSound;      // Grass sound effect
    public AudioClip gravelSound;     // Gravel sound effect

    private CharacterController controller;  // Reference to CharacterController for movement
    private bool isWalking = false;   // Keep track of whether the player is walking
    private bool onGrass = false;            // Keep track of whether the player is on grass

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (move.magnitude > 0 && !isWalking)
        {
            // Player has started moving
            isWalking = true;
            PlayWalkingSound();
        }
        else if (move.magnitude == 0 && isWalking)
        {
            // Player has stopped moving
            isWalking = false;
            audioSource.Stop();
        }
    }

    void PlayWalkingSound()
    {
        // Play the correct sound based on whether the player is on grass or gravel
        audioSource.loop = true;  // Loop the walking sound while the player is moving

        if (onGrass)
        {
            audioSource.clip = grassSound;
        }
        else
        {
            audioSource.clip = gravelSound;
        }

        audioSource.Play();
    }

    void OnTriggerEnter(Collider collision)
    {
       if (collision.transform.CompareTag("Grass"))
        {
            grassPaths.Add(collision);
            onGrass = true;

            if (isWalking)
            {
                // If the player is already walking, switch to grass sound
                PlayWalkingSound();
            }

            else {
                audioSource.Stop();
            }
        }
        else if(collision.transform.CompareTag("BearHome"))
        {
            // Play some sort of constant bear growling sound
            audioSource.clip = bearSound;
            audioSource.loop = true;
            audioSource.Play();
            Debug.Log("Bear growling sounds");
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if(collision.transform.CompareTag("Grass"))
        {
            grassPaths.Remove(collision);
            if(grassPaths.Count == 0)
            {   
                // Player has left all grassy areas
                onGrass = false;

                if (isWalking)
                {
                    // Play some sort of constant graveling walking sound
                    PlayWalkingSound();
                    Debug.Log("Gravel walking sounds");
                }
                
            }

            else {
                // Play some sort of constant grass walking sound
                Debug.Log("Grass walking sounds");

            }
        }
        else if(collision.transform.CompareTag("BearHome"))
        {
            if (audioSource.clip == bearSound) {
                // Stop the bear growling sound
                audioSource.Stop();
                Debug.Log("No more bear growling sounds");
            }
        }
    }
}
