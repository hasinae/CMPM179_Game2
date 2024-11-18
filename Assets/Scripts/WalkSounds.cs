using UnityEngine;
using System.Collections.Generic;

public class WalkSounds : MonoBehaviour
{
    [SerializeField] List<Collider> grassPaths = new List<Collider>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.CompareTag("Grass"))
        {
            if(grassPaths.Count == 0)
            {
                // Play some sort of constant grass walking sound
                Debug.Log("Grass walking sounds");
            }
            grassPaths.Add(collision);
        }
        else if(collision.transform.CompareTag("BearHome"))
        {
            // Play some sort of constant bear growling sound
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
                // Play some sort of constant graveling walking sound
                Debug.Log("Gravel walking sounds");
            }
        }
        else if(collision.transform.CompareTag("BearHome"))
        {
            // Stop the bear growling sound
            Debug.Log("No more bear growling sounds");
        }
    }
}
