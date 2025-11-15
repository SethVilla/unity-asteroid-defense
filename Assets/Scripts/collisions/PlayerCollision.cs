using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private AudioManager audioManager;
    private float currentHP = 50f;
    private float maxHP = 50f;

    void Awake()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
            audioManager = audioObject.GetComponent<AudioManager>();
    }

    void Start()
    {
        UpdateHPDisplay();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            HandleAsteroidCollision(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Space Pod"))
        {
            SpacePodCollision spacePodScript = collision.gameObject.GetComponent<SpacePodCollision>();
            if (spacePodScript != null)
            {
                spacePodScript.TakeDamage(10f);
                TakeDamage(spacePodScript.damage);
            }
        }
        else if (collision.gameObject.CompareTag("Satellite"))
        {
            SatelliteCollision satelliteScript = collision.gameObject.GetComponent<SatelliteCollision>();
            if (satelliteScript != null)
            {
                satelliteScript.TakeDamage(10f);
                TakeDamage(satelliteScript.damage);
            }
        }
        else if (collision.gameObject.CompareTag("Beam"))
        {
            BeamCollision beamScript = collision.gameObject.GetComponent<BeamCollision>();
            if (beamScript != null)
            {
                TakeDamage(beamScript.GetDamage());
                // Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Alien Missile"))
        {
            AlienMissileCollision alienMissileScript = collision.gameObject.GetComponent<AlienMissileCollision>();
            if (alienMissileScript != null)
            {
                alienMissileScript.TakeDamage(10f);
                TakeDamage(alienMissileScript.GetDamage());
            }
        }
        else if (collision.gameObject.CompareTag("Alien Fighter"))
        {
            AlienFighterCollision alienFighterScript = collision.gameObject.GetComponent<AlienFighterCollision>();
            if (alienFighterScript != null)
            {
                alienFighterScript.TakeDamage(10f);
                TakeDamage(alienFighterScript.GetDamage());
            }
        }
        else if (collision.gameObject.CompareTag("Alien Destroyer"))
        {
            AlienDestroyerCollision alienDestroyerScript = collision.gameObject.GetComponent<AlienDestroyerCollision>();
            if (alienDestroyerScript != null)
            {
                alienDestroyerScript.TakeDamage(10f);
                TakeDamage(20f);
            }
        }
    }


    private void HandleAsteroidCollision(GameObject asteroid)
    {
        AsteroidCollision asteroidScript = asteroid.GetComponent<AsteroidCollision>();
        if (asteroidScript != null)
        {
            asteroidScript.TakeDamage(10f);
            TakeDamage(asteroidScript.damage);
            return;
        }
        
        AsteroidCollisionLevel2 asteroidLevel2Script = asteroid.GetComponent<AsteroidCollisionLevel2>();
        if (asteroidLevel2Script != null)
        {
            asteroidLevel2Script.TakeDamage(10f);
            TakeDamage(asteroidLevel2Script.damage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        
        if (audioManager != null && audioManager.impact != null)
            audioManager.PlaySFX(audioManager.impact);
        
        if (currentHP <= 0 && GameUI.Instance.getLives() > 0)
        {
            if (audioManager != null && audioManager.explosions != null && audioManager.explosions.Length > 0)
                audioManager.PlaySFX(audioManager.explosions[Random.Range(0, audioManager.explosions.Length)]);
            
            GameUI.Instance.LoseLife();
            currentHP = maxHP;
            
            // Reset destroyer count if in boss battle
            if (LevelManager.Instance != null)
                LevelManager.Instance.OnLifeLost();
            
            SceneController sceneController = FindObjectOfType<SceneController>();
            if (sceneController != null)
                sceneController.ResetScene();
        }
        else if (currentHP <= 0)
        {
            currentHP = 0;
            GameOver();
        }

        UpdateHPDisplay();
    }

    private void UpdateHPDisplay()
    {
        if (GameUI.Instance != null)
            GameUI.Instance.UpdateHP(currentHP, maxHP);
    }

    private void GameOver()
    {
        GameUI.Instance.ShowGameOver();
        Time.timeScale = 0.0f;
    }

    public float GetCurrentHP() => currentHP;

    public float GetMaxHP() => maxHP;
}
