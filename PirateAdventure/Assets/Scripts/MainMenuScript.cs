using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    PauseMenu pauseMenuScript;
    

    private void Awake()
    {
        pauseMenuScript = GetComponent<PauseMenu>();
        PauseMenu.GameIsPaused = false;
        Time.timeScale = 1f;
    }

    // Function that will call whenever we press button
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ReturnToMain()
    {
        
        SceneManager.LoadScene("Start");
    }
}
