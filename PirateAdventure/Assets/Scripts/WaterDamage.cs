using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDamage : MonoBehaviour
{
    public float damageTime = 2f;
    public int waterDamage = 5;
    public Vector2 knockback = Vector2.zero;

    Damageable damageable;
    public GameObject damageablePlayer;
    private Coroutine _damageCoroutine; // = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = damageablePlayer.GetComponent<Damageable>();
        Debug.Log("There's a collision");

        if (collision.gameObject.CompareTag("WaterDetector"))
        {
            //if (_damageCoroutine != null)
            //{
              //  StopCoroutine(_damageCoroutine);
            //}

            _damageCoroutine = StartCoroutine(UnderwaterDamage());
            Debug.Log("Water damage!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WaterDetector"))
        {
            if (_damageCoroutine !=null)
            {
                StopCoroutine(_damageCoroutine);
                Debug.Log("no more water damage");
            }
            
        }
    }

    IEnumerator UnderwaterDamage()
    {
        var wait = new WaitForSeconds(2);
        while(true)
        {
            Debug.Log("We waited 2 sec and then took damage");
            Damageable damageable = damageablePlayer.GetComponent<Damageable>();
            bool gotHit = damageable.Hit(waterDamage, knockback);
            yield return wait;
        }
            
    }


    
}
