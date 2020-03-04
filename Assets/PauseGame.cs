using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject resume, quit;
    public GameObject PauseMenuUI;
    public GameObject Player;
    public GameObject AudioManager;

    public PauseState pauseState;
    public enum PauseState
    {
        playing, resume, quit
    }

    private void Start()
    {
        pauseState = PauseState.playing;
    }

    void Update()
    {
        switch (pauseState)
        {
            case PauseState.resume:
                if (Input.GetKeyDown("s") || Input.GetKeyDown("down") || Input.GetKeyDown("w") || Input.GetKeyDown("up"))
                {
                    pauseState = PauseState.quit;
                    Play("Change");
                }
                if (Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
                {
                    Resume();
                }
                if (resume.activeInHierarchy == false)
                    resume.SetActive(true);
                if (quit.activeInHierarchy == true)
                    quit.SetActive(false);
                break;
            case PauseState.quit:
                if (Input.GetKeyDown("s") || Input.GetKeyDown("down") || Input.GetKeyDown("w") || Input.GetKeyDown("up"))
                {
                    pauseState = PauseState.resume;
                    Play("Change");
                }
                if (Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
                {
                    QuitGame();
                }
                if (resume.activeInHierarchy == true)
                    resume.SetActive(false);
                if (quit.activeInHierarchy == false)
                    quit.SetActive(true);
                break;
        }
        if (Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Play("Pause");
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Player.GetComponent<PlayerMovement>().pauseState(2);
        Play("Music");
        pauseState = PauseState.playing;
    }

    void Pause()
    {
        Play("Pause");
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Player.GetComponent<PlayerMovement>().pauseState(1);
        AudioManager.GetComponent<AudioManager>().Pause("Music");
        pauseState = PauseState.resume;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu 1");
    }
    public void Play(string sound)
    {
        AudioManager.GetComponent<AudioManager>().Play(sound);
    }

    public void MenuStateResume()
    {
        if (pauseState != PauseState.resume)
        {
            pauseState = PauseState.resume;
            Play("Change");
        }        
    }
    public void MenuStateQuit()
    {
        if (pauseState != PauseState.quit)
        {
            pauseState = PauseState.quit;
            Play("Change");
        }
    }
}
