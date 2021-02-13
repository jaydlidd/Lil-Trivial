using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestionPack : MonoBehaviour
{
    // Abstract class for question packs
    public List<string> questions;
    protected string packName;
    public TextAsset file;
    protected string UIFormat = "default";

    // Difficulty of question (essentially adjusts the number of drinks)
    // 0 = Easy (1-2 Drinks)
    // 1 = Medium (3-4 Drinks)
    // 2 = Hard (5-6 Drinks)
    private int difficultyLevel = 0;

    private bool isSelected = false;

    private void Start()
    {
        string[] lines = file.text.Split('\n');
        questions.Add(file.text);
    }

    // Returns questions in pack
    public List<string> getQuestions() { return questions; }

    // Set if selected in current game
    public void setSelected (bool update) { isSelected = update; }

    // Check if selected
    public bool getSelected() { return isSelected; }

    // Return name of pack
    public string getName() { return packName; }

    // Return the UIFormat of the pack
    public string getFormat() { return UIFormat; }

    public int getDifficulty() { return difficultyLevel; }

    public void setDifficulty(int level) { difficultyLevel = level; }

}
