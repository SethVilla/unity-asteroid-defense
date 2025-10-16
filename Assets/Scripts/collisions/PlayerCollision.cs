using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid") || collision.gameObject.CompareTag("Space Pod"))
        {
            Debug.Log("Player hit an asteroid or space pod! Game Paused.");
            
            // Pause the game
            Time.timeScale = 0.0f;
        }
    }
}

