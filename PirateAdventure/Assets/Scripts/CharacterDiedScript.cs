using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterDiedScript : MonoBehaviour
{
    public static CharacterDiedScript instance;
    
    public AudioClip respawnNoise;
    public float volume = 1f;
    
    [SerializeField] public static bool hasRespawned = false;
    

    private void OnAwake()
    {
        instance = this;
        respawnNoise = GetComponent<AudioClip>();
        
    }


    public void RespawnDeath()
    {

        if (!hasRespawned)
        {
            
            if (GoldBarScript.currentGold > 49)
            {
                //GoldBarScript.instance.DecreaseGold(50);
                //CharacterEvents.characterDied.Invoke(gameObject, 50);
                Debug.Log("1");
                //RespawnFX();
                
            }
            else
            {
                //GoldBarScript.instance.DecreaseGold(GoldBarScript.currentGold);
                //CharacterEvents.characterDied.Invoke(gameObject, GoldBarScript.currentGold);
                Debug.Log("2");
                //RespawnFX();
                
            }
            hasRespawned = true;
            AudioSource.PlayClipAtPoint(respawnNoise, gameObject.transform.position, volume);

            PlayerController playerController = GetComponent<PlayerController>();
          
        }
        else if (hasRespawned)
        {
        Debug.Log("3");
        //hasRespawned = false;
        //return;
        }
        //hasRespawned = false;
        Debug.Log("4");
        //return;
    }
    
    public void RespawnFX()
    {
        Damageable damageable = GetComponent<Damageable>();
        Animator animator = GetComponent<Animator>();

        CharacterEvents.characterHealed.Invoke(gameObject, (damageable.MaxHealth / 2));
        damageable.Health = (damageable.MaxHealth / 2);
        damageable.IsAlive = true;
        animator.SetBool(AnimationsStrings.isAlive, true);
        
        
    }
}
