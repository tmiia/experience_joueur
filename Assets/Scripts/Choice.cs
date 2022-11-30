using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour
{
    [SerializeField] private string text;
    //[SerializeField] private string type;
    private int score;
    private List<GameObject> listChoices = new List<GameObject>();
    [SerializeField] private int index = 0;
    private string goodChoice = "goodChoice";
    private string badChoice = "badChoice";
    private string neutralChoice = "neutralChoice";
    private int globalRate = GameManager.rate;
    private int maxScore = 15;
    private int minScore = 5;



    // Start is called before the first frame update
    void Start()
    {
        GameObject originalGameObject = GameObject.Find("Choices");
        for (int i = 0; i < originalGameObject.transform.childCount; i++)
        {
            GameObject child = originalGameObject.transform.GetChild(i).gameObject;
            listChoices.Add(child);
        }
        AnimationSelection(0);
    }

    // Update is called once per frame
    void Update()
    {
        SelectChoice();
      
    }

    void UpdateRate(string tag)
    {
        if(tag == goodChoice)
        {
            if(globalRate <= 0 && globalRate < (maxScore * GameManager.totalLevel))
            globalRate += maxScore;
        } else if(tag == neutralChoice)
        {
            globalRate += minScore;
        } else if (tag == badChoice)
        {
            globalRate += -maxScore;
        }
    }

    void AnimationSelection(int index)
    {
        for (int i = 0; i < listChoices.Count; i++)
        {
            if(i == index)
            {
                listChoices[i].GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
            }
            else
            {
                listChoices[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            }
        }
    }

    void SelectChoice()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (index >= 0 && index < listChoices.Count - 1)
            {
                index += 1;
                AnimationSelection(index);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (index <= listChoices.Count - 1 && index > 0)
            {
                index -= 1;
                AnimationSelection(index);
            }
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            UpdateRate(listChoices[index].tag);
        }
    }

}
