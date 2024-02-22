using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WouldYouRather : QuestionPack
{
    // Start is called before the first frame update
    void Start()
    {
        packName = "Would you rather?";
        setDrinks(false);
        setDifficulty(1);

        string[] lines = file.text.Split('\n');
        foreach (var x in lines)
        {
            questions.Add(x);
        }
    }
}
