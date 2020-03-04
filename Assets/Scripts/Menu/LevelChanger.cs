using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{

    public GameObject start, menuControls, quit, controls;
    public GameObject mouseMenu;
    public GameObject AudioManager;
    private Animator animator;

    public MenuState menuState;
    public enum MenuState
    {
        start, menuControls, quit, controls
    }
    // Start is called before the first frame update
    void Start()
    {
        menuState = MenuState.start;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (menuState)
        {
            case MenuState.start:
                if (Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
                    StartGame();
                if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
                {
                    menuState = MenuState.menuControls;
                    Play("Change");
                }
                    
                    
                if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
                {
                   Play("Change");
                    menuState = MenuState.quit;
                }
                    
                if (start.activeInHierarchy == false)
                    start.SetActive(true);
                if (menuControls.activeInHierarchy == true)
                    menuControls.SetActive(false);
                if (quit.activeInHierarchy == true)
                    quit.SetActive(false);
                if (controls.activeInHierarchy == true)
                    controls.SetActive(false);
                break;
            case MenuState.menuControls:
                if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
                {
                    Play("Change");
                    menuState = MenuState.start;
                }
                    
                if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
                {
                    Play("Change");
                    menuState = MenuState.quit;
                }
                    
                if (Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
                {
                    Play("Change2");
                    menuState = MenuState.controls;
                }
                    
                if (start.activeInHierarchy == true)
                    start.SetActive(false);
                if (menuControls.activeInHierarchy == false)
                    menuControls.SetActive(true);
                if (quit.activeInHierarchy == true)
                    quit.SetActive(false);
                if (controls.activeInHierarchy == true)
                    controls.SetActive(false);
                break;
            case MenuState.quit:
                if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
                {
                    Play("Change");
                    menuState = MenuState.menuControls;
                }
                   
                if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
                {
                    Play("Change");
                    menuState = MenuState.start;
                }
                    
                if (Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
                    Quit();
                if (start.activeInHierarchy == true)
                    start.SetActive(false);
                if (menuControls.activeInHierarchy == true)
                    menuControls.SetActive(false);
                if (quit.activeInHierarchy == false)
                    quit.SetActive(true);
                if (controls.activeInHierarchy == true)
                    controls.SetActive(false);
                break;
            case MenuState.controls:
                if (Input.GetKeyDown("return") || Input.GetKeyDown("escape") || Input.GetKeyDown("enter"))
                {
                    Play("Change2");
                    menuState = MenuState.menuControls;
                }
                    
                if (start.activeInHierarchy == true)
                    start.SetActive(false);
                if (menuControls.activeInHierarchy == true)
                    menuControls.SetActive(false);
                if (quit.activeInHierarchy == true)
                    quit.SetActive(false);
                if (controls.activeInHierarchy == false)
                    controls.SetActive(true);
                break;
        }
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        animator.SetTrigger("play");
    }

    public void Play(string sound)
    {
        AudioManager.GetComponent<AudioManager>().Play(sound);
    }

    public void MenuStateStart()
    {
        if(menuState != MenuState.start)
        {
            Play("Change");
            menuState = MenuState.start;
        }
        
    }
    public void MenuStateControls()
    {
        if(menuState != MenuState.menuControls)
        {
            Play("Change");
            menuState = MenuState.menuControls;
        }
        
    }
    public void MenuStateQuit()
    {
        if(menuState != MenuState.quit)
        {
            Play("Change");
            menuState = MenuState.quit;
        }
        
    }

    public void gotoMenu()
    {
        Play("Change2");
        menuState = MenuState.menuControls;
        mouseMenu.SetActive(true);
    }

    public void gotoControls()
    {
        Play("Change2");
        menuState = MenuState.controls;
        mouseMenu.SetActive(false);
    }
}
