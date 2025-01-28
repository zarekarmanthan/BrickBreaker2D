using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [SerializeField] LevelManager levelManager;

    [Space]
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform paddleTransform; // Reference to the paddle
    [SerializeField] Transform ballParent; // Optional: To organize hierarchy in hierarchy view

    [SerializeField] GameObject startPanel;
    [SerializeField] TextMeshProUGUI gameOverText;

    [SerializeField] GameObject menupanel;

    [Space]
    [SerializeField] Button startBtn;

    private GameObject currentBall; // Keep track of the active ball

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        Time.timeScale = 0f;
    }

    void Start()
    {
        startBtn.onClick.AddListener(OnClickStart);
        levelManager.LoadLevelData();
        levelManager.GenerateLevel(levelManager.currentLevelIndex);
        ResetBall();


    }

    public void ResetBall()
    {
        if (currentBall != null)
        {
            Destroy(currentBall); // Destroy the previous ball if it exists
        }

        paddleTransform.gameObject.GetComponent<PaddleHandler>().slider.value = 0f;

        paddleTransform.position = new Vector2(0,-3.5f);

        // Instantiate the ball at the paddle's position
        Vector3 ballPosition = new Vector3(paddleTransform.position.x, paddleTransform.position.y + 0.2f, 0);
        currentBall = Instantiate(ballPrefab, ballPosition, Quaternion.identity, ballParent);
    }

    public void OnClickStart()
    {
        gameOverText?.gameObject.SetActive(false);
        startPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Resetting level...");

        // Show the start panel for restart
        startPanel.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;

        startBtn.GetComponentInChildren<TextMeshProUGUI>().text = "RE-START";
        gameOverText.gameObject.SetActive(true);

       

        // Clear existing bricks and reset the level
        levelManager.ClearExistingBricks();
        levelManager.GenerateLevel(levelManager.currentLevelIndex);

        Debug.Log("Paddle : " + paddleTransform.position);

        // Reset the ball
        ResetBall();
    }

    public void BallDestroyed()
    {
        // Trigger Game Over when the ball is destroyed
        GameOver();
    }

    public void OnPauseClick()
    {
        Time.timeScale = 0f;
        menupanel.SetActive(true);
    }


    public void OnContinueClick()
    {
        menupanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
