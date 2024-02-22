using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleQuestions : QuestionPack
{
    // Start is called before the first frame update
    void Start()
    {
        packName = "Battles";
        UIFormat = "battles";
        setDrinks(false);
        setDifficulty(1);

        string[] lines = file.text.Split('\n');
        foreach (var x in lines)
        {
            questions.Add(x);
        }
    }
}
