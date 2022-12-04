using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    // Variables globales

    public static int choice = 1;
    public static int currentLevel = 0;
    public static int achivedLevel = 0;
    public static int totalLevel = 2;
    public bool choiceDone = false;

    // Gestion note
    [SerializeField]  public int rate = 15;
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
        Debug.Log("change scene");
        SceneManager.LoadScene(sceneName);
    }
    public static void Ending()
    {
        if (GameManager.Instance.rate < maxScore)
        {
            Debug.Log("bad end");
            GameManager.ChangeScene("ending-bad");
        } else if (GameManager.Instance.rate >= maxScore && GameManager.Instance.rate < (maxScore * 2))
        {
            Debug.Log("neutral end");
            GameManager.ChangeScene("ending-neutral");
        } else if (GameManager.Instance.rate >= (maxScore * 2))
        {
            Debug.Log("good end");
            GameManager.ChangeScene("ending-good");
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
