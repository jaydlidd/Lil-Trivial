using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Text UI object to show the scoreboard
    public GameObject PlayerScoreBoardObject;

    // Dictionary to track players
    private Dictionary<string, int> scoreboard = new Dictionary<string, int>();

    // Return the current scoreboard
    public Dictionary<string, int> getScoreboard() { return scoreboard; }

    // Add player to scoreboard with default score of 0
    public void addPlayer(string player, int score = 0 ) { scoreboard.Add(player, score); }

    // Remove player to scoreboard with default score of 0
    public void removePlayer(string player) { scoreboard.Remove(player); }

    // Give a player some points
    public void addPoints(string player, int points) { scoreboard[player] += points; }

    // Take some of a player's points
    public void removePoints(string player, int points) { scoreboard[player] -= points; }

    // Function to display of the scoreboard
    public void displayScore()
    {
        int count = 0;
        var sortedPlayers = from entry in scoreboard orderby entry.Value descending select entry;

        foreach (var player in sortedPlayers)
        {
            // Create a new prefab
            var playerScoreboardObj = Instantiate(PlayerScoreBoardObject);

            // Set the parent and location of the new prefab
            playerScoreboardObj.transform.SetParent(this.transform);
            playerScoreboardObj.transform.localPosition = new Vector3(0, 25 - (count * 50), 0);
            playerScoreboardObj.transform.localScale = new Vector3(1, 1, 1);

            // Find and update the text for the current prefab based on which player is winning
            var placeText = playerScoreboardObj.GetComponentsInChildren<TextMeshProUGUI>().ToList().Find(x => x.name.Contains("PlaceText"));
            var scoreText = playerScoreboardObj.GetComponentsInChildren<TextMeshProUGUI>().ToList().Find(x => x.name.Contains("ScoreText"));
            var playerText = playerScoreboardObj.GetComponentsInChildren<TextMeshProUGUI>().ToList().Find(x => x.name.Contains("PlayerText"));
            count += 1;
            placeText.text = count.ToString() + ".";
            scoreText.text = player.Value.ToString();
            playerText.text = player.Key;
        }
    }


}
