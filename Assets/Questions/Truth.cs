using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truth : QuestionPack
{
    void Start()
    {
        packName = "Truth Pack";
        UIFormat = "truthordare";
        setDrinks(false);
        setDifficulty(2);

        string[] lines = file.text.Split('\n');
        foreach (var x in lines)
        {
            questions.Add(x);
        }
    }

}
