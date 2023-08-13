using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterDiedScript : MonoBehaviour
{
    public static CharacterDiedScript instance;
    
    public AudioClip pickupSource;
    public float volume = 1f;

    Animator animator;

    private void OnAwake()
    {
        instance = this;
        pickupSource = GetComponent<AudioClip>();
        animator = GetComponent<Animator>();
    }

    public void RespawnDeath()
    {
        AudioSource.PlayClipAtPoint(pickupSource, gameObject.transform.position, volume);
        CharacterEvents.goldCollected.Invoke(gameObject, -50);
        GoldBarScript.instance.IncreaseGold(-50);

    }
}
