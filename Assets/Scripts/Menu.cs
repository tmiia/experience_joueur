using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
   public void PlayGame()
    {

        GameManager.ChangeScene("level-" + GameManager.currentLevel);
    }
    public void QuitGame()
    {
        Debug.Log("On quitte le jeu");
        Application.Quit();
    }
    public void ReturnMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
