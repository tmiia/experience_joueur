using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    // Variables globales

    public static int choice = 0;
    public static int currentLevel = 0;
    public static int achivedLevel = 0;
    public static int totalLevel = 3;

    // Gestion note
    public int rate = 15;
    public static int maxScore = 15;
    public static int minScore = 5;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public static void ChangeScene (string sceneName)
    {
        
            SceneManager.LoadScene(sceneName);
    }
    public static void Ending()
    {
        if (GameManager.Instance.rate < (maxScore * 2))
        {
            Debug.Log("bad end");
        } else if (GameManager.Instance.rate >= (maxScore * 2) && GameManager.Instance.rate < ((maxScore * 2) + maxScore))
        {
            Debug.Log("neutral end");
        } else if (GameManager.Instance.rate >= ((maxScore * 2) + maxScore))
        {
            Debug.Log("good end");
        }
    }

    public static void ChangeLevel()
    {
        currentLevel += 1;
        achivedLevel += 1;
        choice += 1;

        string nextLevel = "level-" + currentLevel.ToString();
        ChangeScene(nextLevel);
    }
}
