using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public float damage = .1f;

    void Start()
    {
        damage = 1f;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Check if we hit an asteroid
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Try regular asteroid first
            AsteroidCollision asteroidScript = collision.gameObject.GetComponent<AsteroidCollision>();
            if (asteroidScript != null)
            {
                Debug.Log("Bullet hit asteroid for " + damage + " damage.");
                asteroidScript.TakeDamage(damage);
                Destroy(gameObject); // Destroy the bullet after hitting
                return;
            }
            
            // Try Level 2 asteroid
            AsteroidCollisionLevel2 asteroidLevel2Script = collision.gameObject.GetComponent<AsteroidCollisionLevel2>();
            if (asteroidLevel2Script != null)
            {
                Debug.Log("Bullet hit Level 2 asteroid for " + damage + " damage.");
                asteroidLevel2Script.TakeDamage(damage);
                Destroy(gameObject); // Destroy the bullet after hitting
                return;
            }
        }
        // Check if we hit a space pod
        else if (collision.gameObject.CompareTag("Space Pod"))
        {
            SpacePodCollision spacePodScript = collision.gameObject.GetComponent<SpacePodCollision>();
            if (spacePodScript != null)
            {
                Debug.Log("Bullet hit space pod for " + damage + " damage.");
                spacePodScript.TakeDamage(damage);
                Destroy(gameObject); // Destroy the bullet after hitting
            }
        }
        else if (collision.gameObject.CompareTag("Satellite"))
        {
            SatelliteCollision satelliteScript = collision.gameObject.GetComponent<SatelliteCollision>();
            if (satelliteScript != null)
            {
                Debug.Log("Bullet hit satellite for " + damage + " damage.");
                satelliteScript.TakeDamage(damage);
                Destroy(gameObject); // Destroy the bullet after hitting
            }
        }
    }
}