using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechScript : MonoBehaviour
{
    public GameObject speechPrefab;
    public TMP_Text speechText;
    public int wait =3;
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if(gameObject.CompareTag("QM1"))
            {
                StartCoroutine("EntryText");
            }
            else if(gameObject.CompareTag("QM2"))
            {
                StartCoroutine("ExitText");
            }
            else if (gameObject.CompareTag("Nav"))
            {
                StartCoroutine("NavText");
            }
        }
    }

    IEnumerator EntryText()
    {
        speechText.text = "Cap'n! Our Navigator's gone and gotten 'imself lost! Can ye find 'im and bring him back to the ship? I'll meet you on the other side of the island!";
        yield return new WaitForSeconds(wait);
        speechText.text = "Careful! The pirates of this island are mean, but if ye can beat 'em they carry a lot of gold!";
        yield return new WaitForSeconds(wait);
        Destroy(gameObject);
        
    }

    IEnumerator ExitText()
    {
        speechText.text = "Cap'n! Well done! Navigator's on board already! Jump on the boat and lets go!";
        yield return new WaitForSeconds(wait);
        Destroy(speechText);
    }

    IEnumerator NavText()
    {
        speechText.text = "Ahoy Cap'n! There's a real nasty one ahead, I ain't got the skill to fight 'em! I'll sneak out while you distract 'em!";
        yield return new WaitForSeconds(wait);
        Destroy(speechText);
    }
}
