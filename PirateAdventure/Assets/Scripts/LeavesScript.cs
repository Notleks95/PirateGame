using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeavesScript : MonoBehaviour
{
    public Animator animator;

    
    public AudioClip pickupSource;
    public float volume = 1f;
       

    private void OnAwake()
    {
        pickupSource = GetComponent<AudioClip>();
        animator = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            animator.Play("leaves_cut");
            AudioSource.PlayClipAtPoint(pickupSource, gameObject.transform.position, volume);
            Debug.Log("Leaves go swoosh");
        }           
    }

}


