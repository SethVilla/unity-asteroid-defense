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
            AsteroidCollision asteroidScript = collision.gameObject.GetComponent<AsteroidCollision>();
            if (asteroidScript != null)
            {
                Debug.Log("Bullet hit asteroid for " + damage + " damage.");
                asteroidScript.TakeDamage(damage);
                Destroy(gameObject); // Destroy the bullet after hitting
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
    }
}