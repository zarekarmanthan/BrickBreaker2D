using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject paddle;
    public TextMeshProUGUI scoreText;

    private int score;

    void Start()
    {
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
}
