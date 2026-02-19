using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel;
    public AudioSource audioGame;
    public Text levelText, highScoreText, scoreText;
    public static int height = 20, width = 10;
    public int score = 0, level = 1, difficultyPoint;
    public float difficulty = 1;
    public bool pause = false;
    public static Transform[,] grid = new Transform[width, height];

    void Start()
    {
        levelText.text = "LEVEL\n" + level.ToString();
        highScoreText.text = "HIGH SCORE\n" + PlayerPrefs.GetInt("SavedHighScore").ToString();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (pause)
            {
                pause = false;
                pausePanel.SetActive(false);
                audioGame.Play();
            }
            else
            {
                pause = true;
                pausePanel.SetActive(true);
                audioGame.Pause();
            }
        }
        levelText.text = "LEVEL\n" +  level.ToString();
        scoreText.text = "SCORE\n" + score.ToString();
    }

    public bool InsideGrid(Vector2 position)
    {
        return ((int)position.x >= 0 && (int)position.x < width && (int)position.y >= 0);
    }

    public Vector2 Rounds(Vector2 numberRound)
    {
        return new Vector2(Mathf.Round(numberRound.x), Mathf.Round(numberRound.y));
    }

    public void UpdateGrid(TetrisMovement pieceTetris)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == pieceTetris.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }
        foreach (Transform piece in pieceTetris.transform)
        {
            Vector2 position = Rounds(piece.position);
            if (position.y < height)
            {
                grid[(int)position.x, (int)position.y] = piece;
            }
        }
    }

    public Transform PositionTransformGrid(Vector2 position)
    {
        if (position.y > height - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)position.x, (int)position.y];
        }
    }

    public bool CompleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void DeleteSquare(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveLineDown(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllLinesDown(int y)
    {
        for (int i = y; i < height; i++)
        {
            MoveLineDown(i);
        }
    }

    public void TurnOffLine()
    {
        for (int y = 0; y < height; y++)
        {
            if (CompleteLine(y))
            {
                DeleteSquare(y);
                MoveAllLinesDown(y + 1);
                y--;
                score += 100;
                difficultyPoint += 100;
            }
        }
    }

    public bool AboveGrid(TetrisMovement pieceTetris)
    {
        for (int x = 0; x < width; x++)
        {
            foreach (Transform square in pieceTetris.transform)
            {
                Vector2 position = Rounds(square.position);
                if (position.y > height -1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void GameOver()
    {
        HighScoreUpdate();
        SceneManager.LoadScene(1);
    }

    public void HighScoreUpdate()
    {
        if (PlayerPrefs.HasKey("SavedHighScore"))
        {
            if (score > PlayerPrefs.GetInt("SavedHighScore"))
            {
                PlayerPrefs.SetInt("SavedHighScore", score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("SavedHighScore", score);
        }
        highScoreText.text = "HIGH SCORE\n" + PlayerPrefs.GetInt("SavedHighScore").ToString();
    }
}