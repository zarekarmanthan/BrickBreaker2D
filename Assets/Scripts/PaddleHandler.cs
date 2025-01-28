using UnityEngine;
using UnityEngine.UI;

public class PaddleHandler : MonoBehaviour
{
    public Slider slider; // Assign the slider in the Inspector
    public float boundary = 2.5f; // Horizontal boundary for the paddle
    
    float sliderSpeed = 0.1f;

    public Vector2 starPos;
    void Start()
    {
        // Initialize slider values
        slider.minValue = -boundary;
        slider.maxValue = boundary;
        slider.value = 0; // Start in the center

        starPos = transform.position;
    }

    void Update()
    {
        // Move the paddle based on the slider's value
        Vector3 position = transform.position;
        position.x = slider.value; // Set the paddle's x-position to match the slider
        transform.position = position;

        HandleKeyboardInput();
    }


    void HandleKeyboardInput()
    {
        // Check for left and right arrow keys
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            slider.value -= sliderSpeed * Time.deltaTime * 70; // Move slider to the left
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            slider.value += sliderSpeed * Time.deltaTime * 70; // Move slider to the right
        }

        slider.value = Mathf.Clamp(slider.value, slider.minValue, slider.maxValue);
    }
}
