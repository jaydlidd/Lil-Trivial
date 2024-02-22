using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OngoingRules : QuestionPack
{
    // Start is called before the first frame update
    void Start()
    {
        packName = "Games";
        UIFormat = "games";
        setDrinks(false);
        setDifficulty(2);

        string[] lines = file.text.Split('\n');
        foreach (var x in lines)
        {
            questions.Add(x);
        }
    }
}
