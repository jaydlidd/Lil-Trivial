using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ChangeQuestion : MonoBehaviour
{
    // Assign screen wide button, and screen text
    public Button changeQuestionButton;
    public TextMeshProUGUI question;

    void Start()
    {
        // Allow button to be clicked
        changeQuestionButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        // Set question text on click
        question.text = Application.streamingAssetsPath + "/" + "Base.txt";

        if (File.Exists(Application.streamingAssetsPath + "/" + "Base.txt"))
        {

            Debug.Log("File found.");

        }
    }
}
