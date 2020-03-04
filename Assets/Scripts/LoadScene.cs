
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public GameObject controlMenu;
    public void load(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void ControlMenu()
    {
        controlMenu.SetActive(!controlMenu.activeSelf);
    }
}
