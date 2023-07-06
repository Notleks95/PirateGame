using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 30;
    public Vector3 spinRotationSpeed = new Vector3(0, 100, 0);

    public AudioClip pickupSource;
    public float volume = 1f;

    // Start is called before the first frame update
    private void OnAwake()
    {
        pickupSource = GetComponent<AudioClip>();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if(damageable)
        {
            bool wasHealed = damageable.Heal(healthRestore);

            if (wasHealed)
            {
                AudioSource.PlayClipAtPoint(pickupSource, gameObject.transform.position, volume);
                
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }

}
