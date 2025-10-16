using UnityEngine;

public class AsteroidCollision : MonoBehaviour
{
    // Health points 
    private float hp;
    
    // Damage that the asteroid deals
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
        Debug.Log("Asteroid hit for " + damageAmount + " damage. Current HP: " + hp);
        if (hp <= 0)
        {
            DestroyAsteroid();
        }
    }
    
    private void DestroyAsteroid()
    {
        // Check if this asteroid has the Fracture component
        Fracture fractureScript = GetComponent<Fracture>();
        
        if (fractureScript != null)
        {
            // Use the fracture system to break the asteroid into pieces
            fractureScript.FractureObject();
        }
    }
}

