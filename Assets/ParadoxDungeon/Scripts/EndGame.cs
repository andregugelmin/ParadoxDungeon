using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGame : MonoBehaviour
{
    private float startTime, timeCooldown = 5.0f;

    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime > timeCooldown)
        {
            SceneManager.LoadScene("Menu 1");
        }
    }
}
