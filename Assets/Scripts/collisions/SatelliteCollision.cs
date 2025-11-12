using UnityEngine;

public class SatelliteCollision : MonoBehaviour
{
    // Health points 
    private float hp;
    
    // Damage that the satellite deals
    public float damage;

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        // Initialize hp to a random value between 10 and 15 (satellites are more durable)
        hp = Random.Range(10f, 15f);
        damage = Random.Range(5f, 15f);
    }
    
    // Method to take damage from collisions
    public void TakeDamage(float damageAmount)
    {
        hp -= damageAmount;
        Debug.Log("Satellite hit for " + damageAmount + " damage. Current HP: " + hp);
        if (hp <= 0)
        {
            audioManager.PlaySFX(audioManager.explosions[Random.Range(0, audioManager.explosions.Length)]);
            GameUI.Instance.IncreaseScore(15 * (int)damage);
            Destroy(gameObject);
        }
    }
}

