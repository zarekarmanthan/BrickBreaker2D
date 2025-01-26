using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject paddle;
  
    [Space]
    [SerializeField] TextMeshProUGUI scoreText;
    
    [Space]
    [SerializeField] Button startBtn;
    private int score;

    private void Awake()
    {
        Time.timeScale = 0;
        
    }

    void Start()
    {
        startBtn.onClick.AddListener(OnClickStart);
        ResetBall();
    }

    public void ResetBall()
    {
        GameObject ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void OnClickStart()
    {
        startBtn.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

}
