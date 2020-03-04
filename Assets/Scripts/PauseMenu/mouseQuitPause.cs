using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseQuitPause : MonoBehaviour
{
    public GameObject pauseMenu;
    private RectTransform imgRectTransform;

    private void Start()
    {
        imgRectTransform = gameObject.GetComponent<RectTransform>();
    }
    private void Update()
    {
        Vector2 localMousePosition = imgRectTransform.InverseTransformPoint(Input.mousePosition);
        if (imgRectTransform.rect.Contains(localMousePosition))
        {
            pauseMenu.GetComponent<PauseGame>().MenuStateQuit();
            if (Input.GetMouseButtonDown(0))
            {
                pauseMenu.GetComponent<PauseGame>().QuitGame();
            }
        }
    }
    
}
