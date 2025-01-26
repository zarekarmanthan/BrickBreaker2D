using UnityEngine;

public class BallController : MonoBehaviour
{
    public Vector2 initialDirection = Vector2.up + Vector2.right; // Diagonal movement
    public float speed = 5f;

    private Vector2 direction;

    void Start()
    {
        direction = initialDirection.normalized; // Normalize direction
    }

    void Update()
    {
        MoveBall();
        CheckCollisions();
    }

    void MoveBall()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Screen boundaries
        if (transform.position.x > 2.5f || transform.position.x < -2.5f) // Left/Right walls
        {
            direction.x = -direction.x; // Reflect X direction
        }
        if (transform.position.y > 5f) // Top wall
        {
            direction.y = -direction.y; // Reflect Y direction
        }
        if (transform.position.y < -5f) // Bottom wall
        {
            // Handle game over logic
            FindObjectOfType<GameManager>().scoreText.text = "GameOver" ;


        }
    }

    void CheckCollisions()
    {
        // Check for paddle collision
        GameObject paddle = GameObject.FindWithTag("Paddle");
        if (IsColliding(paddle))
        {
            HandlePaddleCollision(paddle);
        }

        // Check for brick collisions
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        foreach (GameObject brick in bricks)
        {
            if (IsColliding(brick))
            {
                HandleBrickCollision(brick);
                break; // Only handle one collision per frame
            }
        }
    }

    bool IsColliding(GameObject obj)
    {
        if (obj == null) return false;

        // Get the bounding boxes of the ball and the object
        Bounds ballBounds = new Bounds(transform.position, transform.localScale);
        Bounds objBounds = new Bounds(obj.transform.position, obj.transform.localScale);

        // Check for overlap
        return ballBounds.Intersects(objBounds);
    }

    void HandlePaddleCollision(GameObject paddle)
    {
        float paddleCenter = paddle.transform.position.x;
        float hitPoint = transform.position.x - paddleCenter;

        // Reflect the ball's direction based on the hit point
        direction = new Vector2(hitPoint, 1).normalized;
    }

    void HandleBrickCollision(GameObject brick)
    {
        // Reflect Y direction
        direction.y = -direction.y;

        // Destroy the brick
        Destroy(brick);

        // Update the score
        FindObjectOfType<GameManager>().AddScore(10);
    }
}
