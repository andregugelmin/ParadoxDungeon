using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseMenuControls : MonoBehaviour
{
    public GameObject levelChanger;
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
            levelChanger.GetComponent<LevelChanger>().MenuStateControls();
            if (Input.GetMouseButtonDown(0))
            {
                levelChanger.GetComponent<LevelChanger>().gotoControls();
            }
        }
    }
    
}
