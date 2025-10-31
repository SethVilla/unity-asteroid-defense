using UnityEngine;

public class SpacePodCollision : MonoBehaviour
{
    // Health points 
    private float hp;
    
    // Damage that the space pod deals
    public float damage;

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        // Initialize hp to a random value between 5 and 10
        hp = Random.Range(5f, 10f);
        damage = Random.Range(1f, 10f);
    }
    
    // Method to take damage from collisions
    public void TakeDamage(float damageAmount)
    {
        hp -= damageAmount;
        Debug.Log("SpacePod hit for " + damageAmount + " damage. Current HP: " + hp);
        if (hp <= 0)
        {
            audioManager.PlaySFX(audioManager.explosions[Random.Range(0, audioManager.explosions.Length)]);
            GameUI.Instance.IncreaseScore(10 * (int)damage);
            Destroy(gameObject);
        }
    }
}

