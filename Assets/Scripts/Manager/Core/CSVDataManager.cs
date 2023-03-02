using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVDataManager : SingletomManager<CSVDataManager>
{
    //[SerializeField]
    //string csv_FileName;

    //Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();

    //public static bool isFinish = false;

    //void Awake()
    //{
    //    DialogueParser theParser = GetComponent<DialogueParser>();
    //    Dialogue[] dialogues = theParser.Parse(csv_FileName);

    //    for (int i = 0; i < dialogues.Length; i++)
    //    {
    //        dialogueDic.Add(i + 1, dialogues[i]);
    //    }

    //    isFinish = true;
    //}

    //public Dialogue[] GetDialogue(int startNum, int endNum)
    //{
    //    List<Dialogue> dialogueList = new List<Dialogue>();

    //    for (int i = 0; i <= endNum - startNum; i++)
    //    {
    //        dialogueList.Add(dialogueDic[startNum + i]);
    //    }

    //    return dialogueList.ToArray();
    //}
}
