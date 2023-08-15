using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDamage : MonoBehaviour
{
    public float damageTime = 1f;
    public int waterDamage = 5;
    public Vector2 knockback = Vector2.zero;
    

    Damageable damageable;
    public GameObject damageablePlayer;

    // Start is called before the first frame update
    void Awake()
    {
        //GameObject player = GameObject.FindGameObjectWithTag("Player");

        //        if (player == null)
        //      {
        //        Debug.Log("No player found in scene. Ensure player is tagged 'Player'");
        //  }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = damageablePlayer.GetComponent<Damageable>();
        Debug.Log("1");

        while (collision.gameObject.CompareTag("WaterDetector"))
        {
            //hit target
            StartCoroutine(UnderwaterDamage(damageTime));
            Debug.Log(collision.name + " has water damage for " + waterDamage);
            return;
        }
        //else
        //{
        //    Debug.Log(collision.name + " = Not water detector");
        //}

    }
    IEnumerator UnderwaterDamage(float damageTime)
    {
        Debug.Log("2r");
        Damageable damageable = damageablePlayer.GetComponent<Damageable>();
        bool gotHit = damageable.Hit(waterDamage, knockback);
        yield return new WaitForSeconds(damageTime);
    }


    
}
