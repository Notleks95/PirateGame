using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InteractionScript : MonoBehaviour
{
    public static Vector3 respawnPoint;
    public GameObject playerPosition;
    MainMenuScript mainMenuScript;
    public TMP_Text speechText;

    public int wait = 5;
    public Canvas gameCanvas;
    private string speechBubble;

    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();

    }

    private void Start()
    {
        respawnPoint = playerPosition.transform.position;
        mainMenuScript = GetComponent<MainMenuScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            Debug.Log("Interacted!");
            
        }
        else if (collision.gameObject.CompareTag("Checkpoint"))
        {
            Debug.Log("CheckpointReached!");
            respawnPoint = playerPosition.transform.position;
            Destroy(collision.gameObject, 2f);
        }

        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("Underwater!");

        }

        if (collision.gameObject.CompareTag("OutOfMap"))
        {
            playerPosition.transform.position = respawnPoint;
        }

        if (collision.gameObject.CompareTag("EndOfMap"))
        {
            StartCoroutine(FinishGame());
        }

        if (collision.CompareTag("QM1"))
        {
            StartCoroutine("EntryText1");
            Destroy(collision);
        } 

        if (collision.CompareTag("QM2"))
        {
            StartCoroutine("ExitText");
        }

        if (collision.CompareTag("Nav"))
        {
            StartCoroutine("NavText");
        }
    }

    private IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator EntryText1()
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(playerPosition.transform.position);
        speechText.text = "Cap'n! Our Navigator's gone and gotten 'imself lost! Can ye find 'im and bring him back to the ship? I'll meet you on the other side of the island!";
        TMP_Text tmpText = Instantiate(speechText, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        yield return new WaitForSeconds(wait);
        Destroy(tmpText);

        speechText.text = "Careful! The pirates of this island are mean, but if ye can beat 'em, they carry a lot of gold!";
        TMP_Text tmpText2 = Instantiate(speechText, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        yield return new WaitForSeconds(wait);
        Destroy(tmpText2);
    }


    IEnumerator ExitText()
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(playerPosition.transform.position);
        speechText.text = "Cap'n! Well done! Navigator's on board already! Jump on the boat and lets go!";
        TMP_Text tmpText = Instantiate(speechText, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        yield return new WaitForSeconds(wait);
        Destroy(tmpText);

    }

    IEnumerator NavText()
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(playerPosition.transform.position);
        speechText.text = "Ahoy Cap'n! There's a real nasty one ahead, I ain't got the skill to fight 'em! I'll sneak out while you distract 'em!";
        TMP_Text tmpText = Instantiate(speechText, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        yield return new WaitForSeconds(wait);
        Destroy(tmpText);
    }
}
