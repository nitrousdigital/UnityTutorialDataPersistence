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
     * Save the current score to the high score table
     */
    public void SaveHighScore(TextMeshProUGUI scoreText)
    {
        string name = scoreText.text.Trim();
        if (name.Length > 0)
        {
            Debug.Log("Saving high score " + currentScore + " for user " + scoreText.text);
            SceneManager.LoadScene(0); // main menu scene
        }
    }

    /**
     * Set the current score, which may be used to create a new high score entry
     */
    public void SetCurrentScore(int score)
    {
        this.currentScore = score;
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
                scores[i] = new HighScore("Player " + (i + 1), 100);
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
