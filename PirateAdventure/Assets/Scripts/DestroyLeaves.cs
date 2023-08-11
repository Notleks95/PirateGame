using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLeaves : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //
        if (collision.gameObject.CompareTag("Leaves"))
        {
            Debug.Log("Leaves destroyed");
            Destroy(collision.gameObject, 1f);
        }
        
    }


}
