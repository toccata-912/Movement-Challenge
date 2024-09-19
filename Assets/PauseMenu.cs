using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject playingUI;
    [SerializeField] InputManager inputManager;
    [SerializeField] MouseLook mouseLook;

    bool paused = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
        playingUI.SetActive(true);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("pressed");
            if (paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        mouseLook.enabled = false;
        inputManager.controls.Disable();
        Debug.Log("pausing");
        paused = true;

        pauseMenu.SetActive(true);
        playingUI.SetActive(false);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        mouseLook.enabled = true;
        inputManager.controls.Enable();
        Debug.Log("resuming");

        paused = false;

        pauseMenu.SetActive(false);
        playingUI.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

}
