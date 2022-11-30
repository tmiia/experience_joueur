using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    // Variables globales

    public static int choice = 0;
    public static int level = 0;
    public static int rate = 30;
    public static int totalLevel = 3;

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
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ChangeScene (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
