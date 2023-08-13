using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public int value = 100;
    public Animator anim;

    public AudioClip pickupSource;
    public float volume = 1f;
    

    private bool isopen = false;

    private void OnAwake()
    {
        pickupSource = GetComponent<AudioClip>();
        anim = GetComponent<Animator>();
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("InteractionDetector") && isopen == false)
        {
            anim.SetBool("isopen",true);
            Debug.Log("Chest Opens");
            AudioSource.PlayClipAtPoint(pickupSource, gameObject.transform.position, volume);
            CharacterEvents.goldCollected.Invoke(gameObject, value);
            GoldBarScript.instance.IncreaseGold(value);
            isopen = true;
            
        }
    }

    
}
