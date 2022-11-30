using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    //[SerializeField] private string levelName = "";
    [SerializeField] private int levelId = 0;



    // Start is called before the first frame update
    void Start()
    {
        ChangeLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeLevel()
    {
        GameManager.level += 1;
        levelId += 1;

        string nextLevel = "level-" + levelId.ToString();
        GameManager.ChangeScene(nextLevel);
    }


}
