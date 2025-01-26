using UnityEngine;

public class BallHandler : MonoBehaviour
{
    public Vector2 offset = new Vector2(0, 0.5f); // Offset from the top of the paddle
    public Vector2 initialDirection = Vector2.up + Vector2.right; // Initial direction
    public float speed = 5f;

    private Vector2 direction;

    private float leftBoundary;
    private float rightBoundary;
    private float topBoundary;
    private float bottomBoundary;


    void Start()
    {
        CalculateScreenBoundaries();

        // Start the ball at the top of the paddle with the specified offset
        Vector2 paddlePosition = new Vector2(0,-3.5f);
        transform.position = new Vector3(paddlePosition.x + offset.x, paddlePosition.y + offset.y, 0);

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
        if (transform.position.x > rightBoundary || transform.position.x < leftBoundary) // Left/Right walls
        {
            direction.x = -direction.x; // Reflect X direction
        }
        if (transform.position.y > topBoundary) // Top wall
        {
            direction.y = -direction.y; // Reflect Y direction
        }
        if (transform.position.y < bottomBoundary) // Bottom wall
        {
            // Handle game over logic
           // FindObjectOfType<GameManager>().scoreText.text = "GameOver" ;


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
      //  FindObjectOfType<GameManager>().AddScore(10);
    }


    void CalculateScreenBoundaries()
    {
        float offset = 0.2f;

        // Get screen boundaries in world coordinates
        Camera mainCamera = Camera.main;
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        // Set boundaries
        leftBoundary = bottomLeft.x + offset;
        rightBoundary = topRight.x - offset;
        bottomBoundary = bottomLeft.y + offset;
        topBoundary = topRight.y - offset;

        Debug.Log($"Boundaries: Left {leftBoundary}, Right {rightBoundary}, Bottom {bottomBoundary}, Top {topBoundary}");
    }
}
