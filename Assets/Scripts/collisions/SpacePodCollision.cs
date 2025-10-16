using UnityEngine;

public class SpacePodCollision : MonoBehaviour
{
    // Health points 
    private float hp;
    
    // Damage that the space pod deals
    public float damage;
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
            Destroy(gameObject);
        }
    }
}

