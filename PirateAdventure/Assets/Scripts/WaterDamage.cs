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
    private Coroutine _damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = damageablePlayer.GetComponent<Damageable>();
        if (collision.gameObject.CompareTag("WaterDetector"))
        {
            _damageCoroutine = StartCoroutine(UnderwaterDamage());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WaterDetector"))
        {
            if (_damageCoroutine !=null)
            {
                StopCoroutine(_damageCoroutine);
            }
        }
    }

    IEnumerator UnderwaterDamage()
    {
        var wait = new WaitForSeconds(2);
        while(true)
        {
            Damageable damageable = damageablePlayer.GetComponent<Damageable>();
            bool gotHit = damageable.Hit(waterDamage, knockback);
            yield return wait;
        }     
    }
}
