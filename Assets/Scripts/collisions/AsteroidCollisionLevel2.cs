using UnityEngine;

public class AsteroidCollisionLevel2 : MonoBehaviour
{
    // Health points 
    private float hp;
    
    // Damage that the asteroid deals
    public float damage;

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        // Initialize hp to a random value between 10 and 30 (Level 2 - harder)
        hp = Random.Range(10f, 30f);
        damage = Random.Range(1f, 10f);
    }
    
    // Method to take damage from collisions
    public void TakeDamage(float damageAmount)
    {
        hp -= damageAmount;
        Debug.Log("Level 2 Asteroid hit for " + damageAmount + " damage. Current HP: " + hp);
        if (hp <= 0)
        {
            audioManager.PlaySFX(audioManager.explosions[Random.Range(0, audioManager.explosions.Length)]);
            DestroyAsteroid();
        } else {
            if (audioManager != null && audioManager.ememyImpact != null)
            {
                audioManager.PlaySFX(audioManager.ememyImpact);
            }
        }
    }
    
    private void DestroyAsteroid()
    {
        if (GameUI.Instance != null)
        {
            GameUI.Instance.IncreaseScore(10 * (int)damage);
        }
        
        // Check if this asteroid has the Fracture component
        Fracture fractureScript = GetComponent<Fracture>();
        
        if (fractureScript != null)
        {
            // Use the fracture system to break the asteroid into pieces
            fractureScript.FractureObject();
        }
    }
}

