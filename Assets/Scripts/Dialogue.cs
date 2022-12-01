using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private string text;
    private List<GameObject> listDialogues = new List<GameObject>();
    private int dialogueIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        GameObject originalGameObjectDialogue = GameObject.Find("DialoguesContainer");
        for (int i = 0; i < originalGameObjectDialogue.transform.childCount; i++)
        {
            GameObject dialogue = originalGameObjectDialogue.transform.GetChild(i).gameObject;
            listDialogues.Add(dialogue);
        }

        PlayDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        PlayNextDialogue();
    }


    public void PlayNextDialogue()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogueIndex <= listDialogues.Count)
        {
            Debug.Log("inna di E press key");
            for (int j = 0; j < listDialogues.Count; j++)
            {
                if (j == dialogueIndex)
                {
                    Debug.Log("inna di true");
                    listDialogues[j].SetActive(true);
                }
                else
                {
                    Debug.Log("inna di false");
                    listDialogues[j].SetActive(false);
                }
            }
            dialogueIndex++;
        }
    }

    public void PlayDialogue()
    {
        listDialogues[0].SetActive(true);
    }
}