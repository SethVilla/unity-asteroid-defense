using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid") || collision.gameObject.CompareTag("Space Pod"))
        {
            Debug.Log("Player hit an asteroid or space pod! Game Paused.");
            // Pause the game
            audioManager.PlaySFX(audioManager.explosions[Random.Range(0, audioManager.explosions.Length)]);
            Time.timeScale = 0.0f;
        }
    }
}

