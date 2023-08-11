using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldScript : MonoBehaviour
{
    public int value;

    public AudioClip pickupSource;
    public float volume = 1f;

    private void OnAwake()
    {
        pickupSource = GetComponent<AudioClip>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickupSource, gameObject.transform.position, volume);
            CharacterEvents.goldCollected.Invoke(gameObject, value);
            Destroy(gameObject);
            GoldBarScript.instance.IncreaseGold(value);
        }
    }
}
