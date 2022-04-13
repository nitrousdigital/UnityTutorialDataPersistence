using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HighScoreEntryManager : MonoBehaviour
{
    /**
     * The textmesh field in which the current score should be displayed
     */
    public TextMeshProUGUI scoreText;
    public TMP_InputField nameInputField;

    // Start is called before the first frame update
    void Start()
    {
        int score = HighScoreManager.Instance.GetCurrentScore();
        scoreText.text = "" + score;

        nameInputField.Select();
        nameInputField.ActivateInputField();
    }

    /**
     * Save the specified score to the high score table
     */
    public void SaveHighScore(TextMeshProUGUI playerNameText)
    {
        string name = playerNameText.text.Trim();
        if (name.Length == 0)
        {
            // require a name
            return;
        }

        // save the score to the highscore table
        HighScoreManager.Instance.SaveCurrentScore(name);

        // load main menu screen
        SceneManager.LoadScene(0);
    }
}
