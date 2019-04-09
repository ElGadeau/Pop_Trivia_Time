using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayLevel(string lvl)
    {
        SceneManager.LoadScene(lvl);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}