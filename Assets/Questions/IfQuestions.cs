using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfQuestions : QuestionPack
{
    // Start is called before the first frame update
    void Start()
    {
        packName = "Targeted";
        setDifficulty(1);
        setDrinks(false);

        string[] lines = file.text.Split('\n');
        foreach (var x in lines)
        {
            questions.Add(x);
        }
    }
}
