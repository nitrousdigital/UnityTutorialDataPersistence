using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class HighScore
{
    public string name;
    public int score;
    public HighScore(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

[System.Serializable]
class SaveData
{
    public HighScore[] scores;
}

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance { get; private set; }
    private const string SAVE_FILE = "/scores.json";

    /**
     * High Scores in order from highest to lowest
     */
    public HighScore[] scores = new HighScore[0];


    /** The current score for which a high score entry may be created */
    private int currentScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadScores();
    }

    /**
     * @return The current high score
     */
    public HighScore GetHighScore()
    {
        return scores[0];
    }

    /**
     * Save the most recent score to the high score table
     * along with the specified player name.
     * 
     * @return True if the score was inserted and saved. False if
     * either a name was not specified or the score is not higher than
     * any current entries in the high score table.
     */
    public bool SaveCurrentScore(string playerName)
    {
        string name = playerName.Trim();
        if (name.Length == 0)
        {
            // require a name
            return false;
        }

        Debug.Log("Saving high score " + currentScore + " for player " + playerName);
        for (int i = 0; i < scores.Length; i++)
        {
            if (currentScore > scores[i].score)
            {
                InsertScore(new HighScore(playerName, currentScore), i);
                currentScore = 0;
                return true;
            }
        }
        return false;
    }

    /**
     * Insert a score into the high score table and save
     */
    private void InsertScore(HighScore score, int position)
    {
        // shift scores down before insertion
        for (int i = scores.Length - 1; i > position; i--)
        {
            scores[i] = scores[i - 1];
        }

        // insert the new score
        scores[position] = score;
        SaveScores();
    }

    /**
     * Set the current score, which may be used to create a new high score entry
     */
    public void SetCurrentScore(int score)
    {
        currentScore = score;
    }

    /**
     * @return The score awarded from the most recent gameplay
     */
    public int GetCurrentScore()
    {
        return currentScore;
    }

    /**
     * Determine whether the specified score is a new high score
     */
    public bool IsHighScore(int score)
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if (score > scores[i].score)
            {
                return true;
            }
        }
        return false;
    }

    public void LoadScores()
    {
        string path = Application.persistentDataPath + SAVE_FILE;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            scores = data.scores;
        } else
        {
            // default initial score table
            scores = new HighScore[4];
            for (int i = 0; i < 4; i++)
            {
                scores[i] = new HighScore("Player " + (i + 1), 100 - (25 * i));
            }
        }
    }

    public void SaveScores()
    {
        SaveData data = new SaveData();
        data.scores = scores;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + SAVE_FILE, json);
    }
}
