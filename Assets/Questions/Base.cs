using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class Base : QuestionPack
{
    void Start()
    {
        packName = "Base Pack";

        string[] lines = file.text.Split('\n');
        foreach (var x in lines) {
            questions.Add(x);
        }
        
    }

}
