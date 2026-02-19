using UnityEngine;

public class TetrisMovement : MonoBehaviour
{
    GameManager gameManager;
    SpawnTetris spawnTetris;
    public float fall, speeds, timer;
    public bool canRotate, rotate360;
    

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        spawnTetris = GameObject.FindObjectOfType<SpawnTetris>();
        timer = speeds;
    }

    void Update()
    {
        if (!gameManager.pause)
        {
            if (gameManager.difficultyPoint > 1000)
            {
                gameManager.level++;
                gameManager.difficultyPoint -= 1000;
                gameManager.difficulty += 0.1f;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                timer = speeds;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                timer += Time.deltaTime;
                if (timer > speeds)
                {
                    transform.position += new Vector3(1, 0, 0);
                    timer = 0;
                }
                if (ValidPosition())
                {
                    gameManager.UpdateGrid(this);
                }
                else
                {
                    transform.position += new Vector3(-1, 0, 0);
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                timer += Time.deltaTime;
                if (timer > speeds)
                {
                    transform.position += new Vector3(-1, 0, 0);
                    timer = 0;
                }
                if (ValidPosition())
                {
                    gameManager.UpdateGrid(this);
                }
                else
                {
                    transform.position += new Vector3(1, 0, 0);
                }
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                timer += Time.deltaTime;
                if (timer > speeds)
                {
                    transform.position += new Vector3(0, -1, 0);
                    timer = 0;
                }
                if (ValidPosition())
                {
                    gameManager.UpdateGrid(this);
                }
                else
                {
                    transform.position += new Vector3(0, 1, 0);
                    gameManager.TurnOffLine();
                    if (gameManager.AboveGrid(this))
                    {
                        gameManager.GameOver();
                    }
                    gameManager.score += 10;
                    gameManager.difficultyPoint += 10;
                    enabled = false;
                    spawnTetris.NextPiece();
                }
            }
            if (Time.time - fall >= (1 / gameManager.difficulty) && !Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += new Vector3(0, -1, 0);
                if (ValidPosition())
                {
                    gameManager.UpdateGrid(this);
                }
                else
                {
                    transform.position += new Vector3(0, 1, 0);
                    gameManager.TurnOffLine();
                    if (gameManager.AboveGrid(this))
                    {
                        gameManager.GameOver();
                    }
                    gameManager.score += 10;
                    gameManager.difficultyPoint += 10;
                    enabled = false;
                    spawnTetris.NextPiece();
                }
                fall = Time.time;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                CheckRotation();
            }
        }
    }

    void CheckRotation()
    {
        if (canRotate)
        {
            if (!rotate360)
            {
                if (transform.rotation.z < 0)
                {
                    transform.Rotate(0, 0, 90);
                    if (ValidPosition())
                    {
                        gameManager.UpdateGrid(this);
                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, -90);
                    if (ValidPosition())
                    {
                        gameManager.UpdateGrid(this);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
            }
            else
            {
                transform.Rotate(0, 0, -90);
                if (ValidPosition())
                {
                    gameManager.UpdateGrid(this);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
        }
    }

    bool ValidPosition()
    {
        foreach (Transform child in transform)
        {
            Vector2 lockPosition = gameManager.Rounds(child.transform.position);
            if (gameManager.InsideGrid(lockPosition) == false)
            {
                return false;
            }
            if (gameManager.PositionTransformGrid(lockPosition) != null && gameManager.PositionTransformGrid(lockPosition).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}