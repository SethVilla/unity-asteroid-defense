using UnityEngine;

public class MissileCollision : MonoBehaviour
{
    public float damage = 10f;

    void OnCollisionEnter(Collision collision)
    {
        // Check if we hit an asteroid
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            AsteroidCollision asteroidScript = collision.gameObject.GetComponent<AsteroidCollision>();
            if (asteroidScript != null)
            {
                Debug.Log("Missile hit asteroid for " + damage + " damage.");
                asteroidScript.TakeDamage(damage);
                Destroy(gameObject); // Destroy the missile after hitting
            }
        }
        // Check if we hit a space pod
        else if (collision.gameObject.CompareTag("Space Pod"))
        {
            SpacePodCollision spacePodScript = collision.gameObject.GetComponent<SpacePodCollision>();
            if (spacePodScript != null)
            {
                Debug.Log("Missile hit space pod for " + damage + " damage.");
                spacePodScript.TakeDamage(damage);
                Destroy(gameObject); // Destroy the missile after hitting
            }
        }
    }
}

