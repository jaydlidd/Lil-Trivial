using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dare : QuestionPack
{
    // Start is called before the first frame update
    void Start()
    {
        packName = "Dare";
        UIFormat = "truthordare";
        setDrinks(true);
        setDifficulty(2);

        string[] lines = file.text.Split('\n');
        foreach (var x in lines)
        {
            questions.Add(x);
        }
    }
}
