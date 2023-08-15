using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    public static Vector3 respawnPoint;
    public GameObject playerPosition;

    private void Start()
    {
        respawnPoint = playerPosition.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //
        if (collision.gameObject.CompareTag("Interactable"))
        {
            Debug.Log("Interacted!");
            
        }
        else if (collision.gameObject.CompareTag("Checkpoint"))
        {
            Debug.Log("CheckpointReached!");
            respawnPoint = playerPosition.transform.position;
            Destroy(collision.gameObject, 2f);
        }

        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("Underwater!");

        }
    }
}
