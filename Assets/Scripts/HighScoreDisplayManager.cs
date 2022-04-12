using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * Updates the UI high score table with the most recent high scores
 */
public class HighScoreDisplayManager : MonoBehaviour
{
    /**
     * The text mesh pro text objects used to display the high scores
     */
    public List<TextMeshProUGUI> topScores;

    /** The fixed length (characters) to be displayed for each line of text in the high score table */
    public int maxLineLength = 20;

    /** Maximum number of characters to display for player name */
    public int maxNameLength = 10;

    // Start is called before the first frame update
    void Start()
    {
        HighScore[] scores = HighScoreManager.Instance.scores;
        for (int i = 0; i < scores.Length && i < topScores.Count; i++)
        {
            // trim play name to max allowed length
            string name = scores[i].name;
            if (name.Length > maxNameLength)
            {
                name = name.Substring(0, maxNameLength);
            }

            // generate filler between player name and high score value
            // so that all lines are of the same (character) length
            string score = "" + scores[i].score;
            int displayLength = (name + score).Length;
            string middle = "";
            while (displayLength + middle.Length < maxLineLength)
            {
                middle = middle + ".";
            }

            topScores[i].text = name + middle + score;
        }
    }

}
