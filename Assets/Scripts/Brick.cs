using UnityEngine;

public class Brick : MonoBehaviour
{
    void OnDestroy()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.OnBrickDestroyed();
        }
    }
}
