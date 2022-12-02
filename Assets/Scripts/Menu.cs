using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadScene("level-" + GameManager.currentLevel);
    }
    public void QuitGame()
    {
        Debug.Log("On quitte le jeu");
        Application.Quit();
    }
    public void returnMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
