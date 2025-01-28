using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    [Space]
    [SerializeField] TextAsset levelFile; 
    [SerializeField] GameObject[] brickPrefabs; 

    [Space]
    [SerializeField] Vector2 gridOffset = new Vector2(-7.5f, 3.5f); // Top-left corner for the grid
    [SerializeField] Vector2 brickSize = new Vector2(1.5f, 0.7f); // Size of each brick with spacing

    [Space]
    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] Button nextLvlBtn;

    public int currentLevelIndex = 0; 
    private LevelData levelData;
    private int remainingBricks;

    void Start()
    {
        nextLvlBtn.onClick.AddListener(OnNextButtonClick);
        
    }

    public void LoadLevelData()
    {
        if (levelFile != null)
        {
            levelData = JsonConvert.DeserializeObject<LevelData>(levelFile.text);
        }
        else
        {
            Debug.LogError("Level file not assigned!");
        }
    }

    public void GenerateLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelData.levels.Length)
        {
            Debug.LogError("Invalid level index!");
            return;
        }

        ClearExistingBricks(); // Clear bricks from any previous level

        Level level = levelData.levels[levelIndex];
        int rows = level.rows;
        int columns = level.columns;

        // Get the screen boundaries based on the camera
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2; // Horizontal screen size
        float screenHeight = Camera.main.orthographicSize * 2; // Vertical screen size

        // Calculate available space per brick
        float maxBrickWidth = screenWidth / columns;
        float maxBrickHeight = screenHeight / rows;

        // Use the smaller of the two dimensions to fit the bricks
        Vector2 adjustedBrickSize = new Vector2(
            Mathf.Min(brickSize.x, maxBrickWidth),
            Mathf.Min(brickSize.y, maxBrickHeight)
        );

        // Calculate grid offset to center the layout
        Vector2 startOffset = new Vector2(
            -screenWidth / 2 + (adjustedBrickSize.x / 2),
            screenHeight / 2 - (adjustedBrickSize.y / 2)
        );

        // Instantiate bricks
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int brickType = level.data[row][col];
                if (brickType > 0 && brickType <= brickPrefabs.Length)
                {
                    // Calculate the position of each brick
                    Vector2 position = new Vector2(
                        startOffset.x + col * adjustedBrickSize.x,
                        startOffset.y - row * adjustedBrickSize.y
                    );

                    // Instantiate the brick at the calculated position
                    Instantiate(brickPrefabs[brickType - 1], position, Quaternion.identity);

                    remainingBricks++;
                }
            }
        }
    }


    public void ClearExistingBricks()
    {
        foreach (GameObject brick in GameObject.FindGameObjectsWithTag("Brick"))
        {
            Destroy(brick);
        }
    }


    public void OnBrickDestroyed()
    {
        remainingBricks--;

        if (remainingBricks <= 0)
        {
            Debug.Log("Level Complete!");
            GameManager.Instance.ResetBall();
            Time.timeScale = 0f;
            ShowLevelCompleteUI();
        }
    }

    void ShowLevelCompleteUI()
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            LoadNextLevel();
        }
    }



    public void LoadNextLevel()
    {

        currentLevelIndex++;
        if (currentLevelIndex < levelData.levels.Length)
        {
            GenerateLevel(currentLevelIndex);
        }
        else
        {
            currentLevelIndex = 0;
            GenerateLevel(currentLevelIndex);
        }

    }

    void OnNextButtonClick()
    {
        levelCompletePanel.SetActive(false);
        Time.timeScale = 1f;
    }


    public int GetTotalLevels()
    {
        return levelData.levels.Length;
    }
}

[System.Serializable]
public class LevelData
{
    public Level[] levels;
}

[System.Serializable]
public class Level
{
    public int rows;
    public int columns;
    public int[][] data;
}
