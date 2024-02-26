using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    // Game Scenes
    public GameObject MainMenu;
    public GameObject PackSelection;
    public GameObject PlayGame;
    public GameObject PlayerManage;
    public GameObject ScoreBoard;

    // List of question packs for game
    private List<QuestionPack> packs = new List<QuestionPack>();

    // List of players in game
    private List<string> players = new List<string>();

    // List of all questions from selected pack
    private List<(string, string, int, bool)> gameQuestions = new List<(string, string, int, bool)>();

    // Text for displaying list of selected packs
    public TextMeshProUGUI packList;

    // Text to display questions, players involved and drinks
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;
    public TextMeshProUGUI drinksText;
    public TextMeshProUGUI vsText;
    public TextMeshProUGUI startingWithText;
    public GameObject displayPlayers;
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI playerNumberErrorMessage;
    public Image drinks;

    // Button to start game
    public Button startGame;
    public Button touchToStartButton;

    // Buttons for respective packs
    public Button baseButton;
    public Button truthButton;
    public Button dareButton;
    public Button battleButton;
    public Button categoriesButton;
    public Button everyoneButton;
    public Button targetedButton;
    public Button rulesButton;
    public Button wouldButton;

    // References to class for each pack
    public QuestionPack basePack;
    public QuestionPack truthPack;
    public QuestionPack darePack;
    public QuestionPack battlePack;
    public QuestionPack categoriesPack;
    public QuestionPack everyonePack;
    public QuestionPack targetedPack;
    public QuestionPack rulesPack;
    public QuestionPack wouldPack;

    // Buttons for playing game
    public Button nextQuestionButton;
    public Button settingsButton;
    public Button managePlayersButton;
    public Button acceptPlayersButton;
    public Button addPlayerButton;
    public Button editGameButton;

    // Scoreboard scene buttons
    public Button scoreBackButton;
    public Button scoreBoardButton;

    // Variables tracking number of questions in games
    private int totalQuestions;

    // Prefab of scroll grid object
    public GameObject prefab;

    // Create list of size players * 10, giving each player an even start
    private bool playerNoSatsified;
    private int sizeOfTracker;
    //private int lastPlayer;
    private List<Tuple<string, int>> drinkTracker;
    private int tracker = 0;

    // Handling the warning messages and buttons
    public Button warningMessageButton;

    // Create a new scoreboard
    public ScoreManager scoreManager;

    void Start()
    {
        // Set up buttons in Pack Selection
        startGame.onClick.AddListener(StartTaskOnClick);
        touchToStartButton.onClick.AddListener(TouchToStartTaskOnClick);

        truthButton.onClick.AddListener(TruthTaskOnClick);
        dareButton.onClick.AddListener(DareTaskOnClick);
        battleButton.onClick.AddListener(BattleTaskOnClick);
        categoriesButton.onClick.AddListener(CategoriesTaskOnClick);
        everyoneButton.onClick.AddListener(EveryoneTaskOnClick);
        targetedButton.onClick.AddListener(TargetedTaskOnClick);
        rulesButton.onClick.AddListener(RulesTaskOnClick);
        wouldButton.onClick.AddListener(WouldTaskOnClick);

        // Set up buttons in Play Game
        nextQuestionButton.onClick.AddListener(NextQuestionTaskOnClick);
        managePlayersButton.onClick.AddListener(ManagePlayerTaskOnClick);
        acceptPlayersButton.onClick.AddListener(AcceptPlayersButtonTaskOnClick);
        addPlayerButton.onClick.AddListener(AddPlayersButtonTaskOnClick);
        editGameButton.onClick.AddListener(EditGameTaskOnClick);

        // Set up buttons in Scoreboard
        scoreBoardButton.onClick.AddListener(ScoreBoardTaskOnClick);
        scoreBackButton.onClick.AddListener(BackTaskOnClick);

        // Display warning message on start
        warningMessageButton.onClick.AddListener(WarningMessageButtonTaskOnClick);
    }

    void Update()
    {
        if (players.Count <= 1)
        {
            playerNumberErrorMessage.enabled = true;
            playerNoSatsified = false;
        }
        else
        {
            playerNumberErrorMessage.enabled = false;
            playerNoSatsified = true;
        }
    }

    // Start Game
    void StartTaskOnClick() {
        // Build Questions from selected packs
        // Enumerate through all selected packs
        List<string> packQuestions = new List<string>();
        string packFormat;
        int packDifficulty;
        bool packRequiresDrinks;

        foreach (var x in packs) 
        {
            
            // Create individual collection of the questions from each pack
            packQuestions = x.getQuestions();
            packFormat = x.getFormat();
            packDifficulty = x.getDifficulty();
            packRequiresDrinks = x.requiresDrinks();
            
            // Add them all to global game questions
            foreach (var y in packQuestions)
            {
                gameQuestions.Add((y, packFormat, packDifficulty, packRequiresDrinks));
            }
        }

        // Set number of questions
        totalQuestions = gameQuestions.Count;
        Debug.Log(totalQuestions);

        // Set up Drink Tracker
        drinkTracker = new List<Tuple<string, int>>();

        // Play game
        PackSelection.SetActive(false);
        PlayGame.SetActive(true);
        RunGame();

    }

    void RunGame() {
        // Play the game, each time the button is clicked, showing a different question.
        // Also handles the tracking of players and balancing of drinks.

        // Set up drink tracker and randomising by initialising the tracking list at 10 * each player, giving each player an even chance of being selected at start.
        if (playerNoSatsified == false)
        {
            // Error handling: Make sure at least one player is playing
            PackSelection.SetActive(true);
            PlayGame.SetActive(false);
        }
        else
        {
            
            // Initialise list, with i = 0 being player 0 (first player added), i = n being the nth player added
            for (int i = 0; i < players.Count; i++)
            {
                drinkTracker.Add(Tuple.Create(players[i], 10));
                sizeOfTracker += 10;
            }

            
            for (int i = 0; i < players.Count; i++)
            {
                tracker += drinkTracker[i].Item2;
            }

            string questionPlayer = DeterminePlayers(drinkTracker);
            NextQuestion(questionPlayer);
        }
    }

    void NextQuestion(string player) {
        // Uses the randomiser to display questions

        // Choose random question, without repicking a question
        // Non-repetition acheived by noting random number checked and if already used, a new number is picked.
        bool newNumber = false;
        var randNum = UnityEngine.Random.Range(0, totalQuestions + 1);
        List<int> usedNum = new List<int>(totalQuestions);
        string currentFormat;

        player1Text.text = player;

        while (newNumber == false){
            if (usedNum.Count == totalQuestions)
            {
                // If we have used all questions, start again by clearing usedNum
                usedNum.Clear();
            }
            else if (usedNum.Contains(randNum))
            {
                // If we have used it before, re random until we get one we haven't used
                randNum = UnityEngine.Random.Range(0, totalQuestions + 1);
            }
            else 
            {
                // Set the display format based on the question
                currentFormat = gameQuestions[randNum].Item2;
                if (currentFormat == "truthordare")
                {
                    // Display the choice, and then the question
                    // TODO: Display player to make choice
                    // TODO: Create overlay for truthdaredrink to cover actual question
                    questionText.text = "Truth, dare or drink?";
                }
                AlterUIFormat(currentFormat);

                // Display question, add it to already used questions and wait for next button click
                // TODO: Display player to make choice
                questionText.text = gameQuestions[randNum].Item1;
                
                if (gameQuestions[randNum].Item4 == false)
                {
                    drinks.enabled = false;
                    drinksText.enabled = false;
                } 
                else
                {
                    drinks.enabled = true;
                    drinksText.enabled = true;
                    drinksText.text = (UnityEngine.Random.Range(1, 3) + gameQuestions[randNum].Item3).ToString();
                }

                if (gameQuestions[randNum].Item2 == "battles")
                {
                    player2Text.text = DeterminePlayers(drinkTracker);
                    while (player1Text.text == player2Text.text) 
                    {
                        player2Text.text = DeterminePlayers(drinkTracker);
                    }
                }

                usedNum.Add(randNum);
                newNumber = true;
            }
        }
    }
    
    string DeterminePlayers(List<Tuple<string, int>> drinkTracker)
    {
        // Uses previous rounds and the amount of drinks given to determine future players, in an attempt to even out the drinks.
        // Check values equal this number before randomising, otherwise, add one to another player that didn't take a turn (player with the least drinks) until we get to size again

        /*
        while (tracker != sizeOfTracker)
        {
            int addPlayerRandomly;
            do { addPlayerRandomly = UnityEngine.Random.Range(0, players.Count + 1); }
            while (addPlayerRandomly == lastPlayer);

            drinkTracker[addPlayerRandomly] = new Tuple<string, int>(drinkTracker[addPlayerRandomly].Item1, (drinkTracker[addPlayerRandomly].Item2 + 1));
            tracker++;
        }
        
        // Select random player
        int[] randPlayerList = new int[tracker];
        int count = 0;

        for (int i = 0; i < players.Count; i++)
        {
            for (int j = 0; j < drinkTracker[i].Item2; j++)
            {
                randPlayerList[count] = i;
                //count++;
            }
        }
        */
        System.Random rnd = new System.Random();

        int selected = rnd.Next(players.Count);

        string location = players[selected];
        //string nextPlayer = drinkTracker[location].Item1;

        //drinkTracker[location] = new Tuple<string, int>(drinkTracker[location].Item1, (drinkTracker[location].Item2 - 1));

        // Set last player to next player for next question
        //lastPlayer = location;

        // Take away the number of chances for player to get selected
        
        
        return location;

        // Pick random player, take number of drinks off this player in list.
        // Run once per question.
    }

    void AlterUIFormat(string format) {
        // Changes the position of text based on the format of the question
        if (format == "default")
        {
            questionText.transform.position = new Vector2(0, 0);
            questionText.fontSize = 54;
            player1Text.transform.position = new Vector2(0, 40);
            player1Text.fontSize = 72;
            player1Text.enabled = true;
            player2Text.enabled = false;
            vsText.enabled = false;
            startingWithText.enabled = false;
        }
        else if (format == "noname") 
        {
            questionText.transform.position = new Vector2(0, 0);
            questionText.fontSize = 72;
            player1Text.enabled = false;
            player2Text.enabled = false;
            vsText.enabled = false;
            startingWithText.enabled = false;
        }
        else if (format == "categories")
        {
            questionText.transform.position = new Vector2(0, 40);
            questionText.fontSize = 54;
            player1Text.fontSize = 72;
            player1Text.enabled = true;
            player1Text.transform.position = new Vector2(0, -40);
            player2Text.enabled = false;
            vsText.enabled = false;
            startingWithText.enabled = true;
        }
        else if (format == "battles")
        {
            questionText.transform.position = new Vector2(0, 40);
            questionText.fontSize = 54;
            player1Text.enabled = true;
            player1Text.transform.position = new Vector2(0, 0);
            player1Text.fontSize = 72;
            player2Text.enabled = true;
            player2Text.transform.position = new Vector2(0, -40);
            player2Text.fontSize = 72;
            vsText.enabled = true;
            vsText.fontSize = 54;
            startingWithText.enabled = false;
        }
        else if (format == "truthordare")
        {
            questionText.transform.position = new Vector2(0, -20);
            questionText.fontSize = 54;
            player1Text.enabled = true;
            player1Text.transform.position = new Vector2(0, 20);
            player1Text.fontSize = 72;
            player2Text.enabled = false;
            vsText.enabled = false;
            startingWithText.enabled = false;
        }
    }

    // Print pack list
    void printPackList() {
        // Print pack list
        packList.text = null;
        if (packs.Count == 0)
        {
            packList.text = "No Packs Selected";
        }
        else
        {
            foreach (var x in packs)
            {
                packList.text += "\u2022 " + x.getName() + '\n';
            }
        }
        
    }

    // Add selected pack
    public void AddPack(QuestionPack pack) { 
        // Add new pack to list
        packs.Add(pack);

        // Update shown pack list
        printPackList();
    }

    // Remove selected pack
    public void RemovePack(QuestionPack pack) { 
        // Remove pack from pack list
        packs.Remove(pack);

        // Update shown pack list
        printPackList();
    }
    void Activation(QuestionPack pack)
    {
        // Handle the activation of a pack
        if (pack.getSelected() == false)
        {
            AddPack(pack);
            pack.setSelected(true);
        }
        else
        {
            RemovePack(pack);
            pack.setSelected(false);
        }
    }

    public void RemovePlayer(int index)
    {
        string playerToRemove = players[index];
        players.Remove(playerToRemove);
    }

    void BaseTaskOnClick() {
        // Handle base pack
        Activation(basePack);
    }
    void TruthTaskOnClick()
    {
        // Handle truth pack
        Activation(truthPack);
    }
    void DareTaskOnClick()
    {
        // Handle truth pack
        Activation(darePack);
    }
    void BattleTaskOnClick()
    {
        // Handle truth pack
        Activation(battlePack);
    }
    void CategoriesTaskOnClick()
    {
        // Handle truth pack
        Activation(categoriesPack);
    }
    void EveryoneTaskOnClick()
    {
        // Handle truth pack
        Activation(everyonePack);
    }
    void TargetedTaskOnClick()
    {
        // Handle truth pack
        Activation(targetedPack);
    }
    void RulesTaskOnClick()
    {
        // Handle truth pack
        Activation(rulesPack);
    }
    void WouldTaskOnClick()
    {
        // Handle truth pack
        Activation(wouldPack);
    }
    void NextQuestionTaskOnClick()
    {
        // Handle displaying next question
        string nextPlayer = DeterminePlayers(drinkTracker);
        NextQuestion(nextPlayer);
    }
    void TouchToStartTaskOnClick() {
        MainMenu.SetActive(false);
        PackSelection.SetActive(true);
    }

    void ManagePlayerTaskOnClick()
    {
        ScoreBoard.SetActive(false);
        PackSelection.SetActive(false);
        PlayGame.SetActive(false);
        PlayerManage.SetActive(true);
    }

    void EditGameTaskOnClick()
    {
        ScoreBoard.SetActive(false);
        PackSelection.SetActive(true);
        PlayGame.SetActive(false);
        PlayerManage.SetActive(false);
    }

    void ScoreBoardTaskOnClick()
    {
        ScoreBoard.SetActive(true);
        PlayGame.SetActive(false);
        PlayerManage.SetActive(false);
        PackSelection.SetActive(false);
        scoreManager.displayScore();
    }

    void BackTaskOnClick()
    {
        ScoreBoard.SetActive(false);
        PlayGame.SetActive(true);
        PlayerManage.SetActive(false);
        PackSelection.SetActive(false);
    }

    void AcceptPlayersButtonTaskOnClick()
    {
        PlayerManage.SetActive(false);
        PackSelection.SetActive(true);

        foreach (Transform child in displayPlayers.transform)
        {
            if (!players.Contains(child.GetComponentInChildren<TextMeshProUGUI>().text))
            {
                players.Remove(child.GetComponentInChildren<TextMeshProUGUI>().text);
            }
        }
    }
    
    void AddPlayersButtonTaskOnClick()
    {
        if (!string.IsNullOrEmpty(playerNameInput.text))
        {
            GameObject g = GameObject.Instantiate(prefab, displayPlayers.transform);

            g.GetComponentInChildren<TextMeshProUGUI>().text = playerNameInput.text;
            players.Add(playerNameInput.text);

            // Add player to the scoreboard
            scoreManager.addPlayer(playerNameInput.text);
        }

        playerNameInput.text = "";

    }

    void WarningMessageButtonTaskOnClick()
    {
        // Handle players acceptance of warnings and continue the game
        warningMessageButton.gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }
}
