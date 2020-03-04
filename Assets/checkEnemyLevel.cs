using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class checkEnemyLevel : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    public GameObject canvas;
    public TextMeshProUGUI text;


    private void Start()
    {
        canvas.SetActive(false);
    }
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "Enemy")
            {
                canvas.SetActive(true);
                int level = hit.collider.GetComponent<CharacterStats>().level;
                string name = hit.collider.name;

                text.text = name + " - Level " + level;
            }
            else
            {
                canvas.SetActive(false);
            }
            
        }
    }
}