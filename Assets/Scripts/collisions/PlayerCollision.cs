using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private AudioManager audioManager;
    private float currentHP_ = 50f;
    private float maxHP_ = 50f;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        // Initialize HP bar display
        UpdateHPDisplay();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Get damage from asteroid
            AsteroidCollision asteroidScript = collision.gameObject.GetComponent<AsteroidCollision>();
            if (asteroidScript != null)
            {
                float damage = asteroidScript.damage;
                asteroidScript.TakeDamage(10f);
                TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("Space Pod"))
        {
            // Get damage from space pod
            SpacePodCollision spacePodScript = collision.gameObject.GetComponent<SpacePodCollision>();
            if (spacePodScript != null)
            {
                float damage = spacePodScript.damage;
                spacePodScript.TakeDamage(10f);
                TakeDamage(damage);
            }
        }

    }

    public void TakeDamage(float damage)
    {
        currentHP_ -= damage;
        if (currentHP_ <= 0 && GameUI.Instance.getLives() > 0) {
        
            GameUI.Instance.LoseLife();
            currentHP_ = maxHP_;
        }   else if (currentHP_ <= 0) {
            currentHP_ = 0;
            GameOver();
        }
        

        // Update HP bar display
        UpdateHPDisplay();
    }

    private void UpdateHPDisplay()
    {
        if (GameUI.Instance != null)
        {
            GameUI.Instance.UpdateHP(currentHP_, maxHP_);
        }
    }

    private void GameOver()
    {
        GameUI.Instance.ShowGameOver();
        Debug.Log("Game Over! HP reached 0.");
        Time.timeScale = 0.0f; // Pause the game
    }

    public float GetCurrentHP()
    {
        return currentHP_;
    }

    public float GetMaxHP()
    {
        return maxHP_;
    }
}

