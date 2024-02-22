using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Everyone : QuestionPack
{
    // Start is called before the first frame update
    void Start()
    {
        packName = "Everyone's Involved";
        UIFormat = "noname";
        setDifficulty(0);
        setDrinks(false);

        string[] lines = file.text.Split('\n');
        foreach (var x in lines)
        {
            questions.Add(x);
        }
    }
}
