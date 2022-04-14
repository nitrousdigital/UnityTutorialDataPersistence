using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    private enum GameState
    {
        LEVEL_ANNOUNCE,
        PLAYING,
        GAME_OVER_ANNOUNCE
    }

    public Brick BrickPrefab;
    public Rigidbody ballRb;
    public int LineCount = 6;
    public Text ScoreText;
    public Text HighScoreText;
    public Text LevelAnnounceText;
    public GameObject LevelAnnounceTextContainer;
    public GameObject gameOverText;

    private int levelAnnounceDuration = 3;
    private int gameOverAnnounceDuration = 4;

    private GameState state;
    private Paddle paddle;
    private Ball ball;
    private bool ballLaunched = false;
    private int m_Points;
    
    private int level = 1;
    private int bricksPerLine;
    private List<Brick> bricks;
    private float ballSpeed;

    // Start is called before the first frame update
    void Start()
    {
        paddle = FindObjectOfType<Paddle>();
        ball = FindObjectOfType<Ball>();
        HighScore highScore = HighScoreManager.Instance.GetHighScore();
        HighScoreText.text = ("BEST SCORE: " + highScore.name + " : " + highScore.score).ToUpper();
        BuildBricks();
        GoToLevel(1);
    }

    /**
     * Announce the specified level and schedule play
     */
    private void GoToLevel(int level)
    {
        this.level = level;
        LevelAnnounceText.text = "LEVEL " + level;
        state = GameState.LEVEL_ANNOUNCE;
        ShowUiForState();
        PrepareLevel(level);
        StartCoroutine(ScheduleStartLevel());
    }

    /**
     * Wait for a short period of time before hiding the level announcement
     */
    private IEnumerator ScheduleStartLevel()
    {
        yield return new WaitForSeconds(levelAnnounceDuration);
        state = GameState.PLAYING;
        ShowUiForState();
    }

    /**
     * Toggle visibility of UI elements depending on the current GameState
     */
    private void ShowUiForState()
    {
        gameOverText.SetActive(state == GameState.GAME_OVER_ANNOUNCE);
        LevelAnnounceTextContainer.SetActive(state == GameState.LEVEL_ANNOUNCE);
    }

    /**
     * (Re)Initialize the bricks 
     */
    private void PrepareLevel(int level)
    {
        this.level = level;
        paddle.Speed = this.level + 2f;
        paddle.ResetPosition();
        ballLaunched = false;

        ballSpeed = this.level + 1f;
        ball.ResetPosition();

        // prepare scores for the level
        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < pointCountArray.Length; i++)
        {
            pointCountArray[i] *= level;
        }

        // assign point values to the bricks and ensure all bricks are visible
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < bricksPerLine; ++x)
            {
                int brickIdx = (i * bricksPerLine) + x;
                bricks[brickIdx].SetPointValue(pointCountArray[i]);
                bricks[brickIdx].gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (state == GameState.PLAYING && !ballLaunched)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ballLaunched = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                ballRb.transform.SetParent(null);
                ballRb.AddForce(forceDir * ballSpeed, ForceMode.VelocityChange);
            }
        }
    }

    public bool IsPlaying()
    {
        return state == GameState.PLAYING;
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"SCORE : {m_Points}";
        CheckLevelComplete();
    }

    /**
     * Check whether all bricks have been destroyed and if so, advance to the next level
     */
    private void CheckLevelComplete()
    {
        if (VisibleBrickCount() == 0)
        {
            GoToLevel(level + 1);
        }
    }

    /**
     * Count the number of remaining visible bricks
     */
    private int VisibleBrickCount()
    {
        int count = 0;
        for (int i = 0; i < bricks.Count; i++)
        {
            if (bricks[i].gameObject.activeSelf)
            {
                count++;
            }
        }
        Debug.Log(count + " visible bricks");
        return count;
    }

    /**
     * Announce the end of the game
     */
    public void GameOver()
    {
        state = GameState.GAME_OVER_ANNOUNCE;
        ShowUiForState();        
        StartCoroutine(ScheduleHighScoreCheck());
    }

    /**
     * Wait for a short period of time before hiding the game over screen
     * and then check whether the user obtained a high score.
     * If so, go to the name entry scene, otherwise return to the main menu.
     */
    private IEnumerator ScheduleHighScoreCheck()
    {
        yield return new WaitForSeconds(gameOverAnnounceDuration);
        CheckHighScore();
    }

    private void CheckHighScore()
    {
        if (HighScoreManager.Instance.IsHighScore(m_Points))
        {
            HighScoreManager.Instance.SetCurrentScore(m_Points);
            SceneManager.LoadScene(2); // NewHighScore scene
        } else
        {
            SceneManager.LoadScene(0); // main menu scene
        }
    }

    /**
     * Construct the bricks
     */
    private void BuildBricks()
    {
        const float step = 0.6f;
        bricksPerLine = Mathf.FloorToInt(4.0f / step);

        bricks = new List<Brick>();
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < bricksPerLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                Brick brick = Instantiate(BrickPrefab, position, Quaternion.identity);

                brick.onDestroyed.AddListener(AddPoint);
                bricks.Add(brick);
            }
        }
    }
}
