using UnityEngine;

public class MissileCollision : MonoBehaviour
{
    public float damage = 10f;

    void OnCollisionEnter(Collision collision)
    {
        // Check if we hit an asteroid
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Try regular asteroid first
            AsteroidCollision asteroidScript = collision.gameObject.GetComponent<AsteroidCollision>();
            if (asteroidScript != null)
            {
                Debug.Log("Missile hit asteroid for " + damage + " damage.");
                asteroidScript.TakeDamage(damage);
                Destroy(gameObject); // Destroy the missile after hitting
                return;
            }
            
            // Try Level 2 asteroid
            AsteroidCollisionLevel2 asteroidLevel2Script = collision.gameObject.GetComponent<AsteroidCollisionLevel2>();
            if (asteroidLevel2Script != null)
            {
                Debug.Log("Missile hit Level 2 asteroid for " + damage + " damage.");
                asteroidLevel2Script.TakeDamage(damage);
                Destroy(gameObject); // Destroy the missile after hitting
                return;
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
        // Check if we hit a satellite
        else if (collision.gameObject.CompareTag("Satellite"))
        {
            SatelliteCollision satelliteScript = collision.gameObject.GetComponent<SatelliteCollision>();
            if (satelliteScript != null)
            {
                Debug.Log("Missile hit satellite for " + damage + " damage.");
                satelliteScript.TakeDamage(damage);
                Destroy(gameObject); // Destroy the missile after hitting
            }
        }
        // Check if we hit an alien missile
        else if (collision.gameObject.CompareTag("Alien Missile"))
        {
            AlienMissileCollision alienMissileScript = collision.gameObject.GetComponent<AlienMissileCollision>();
            if (alienMissileScript != null)
            {
                Debug.Log("Missile hit alien missile for " + damage + " damage.");
                alienMissileScript.TakeDamage(damage);
                Destroy(gameObject); // Destroy the missile after hitting
            }
        }
        // Check if we hit an alien fighter
        else if (collision.gameObject.CompareTag("Alien Fighter"))
        {
            AlienFighterCollision alienFighterScript = collision.gameObject.GetComponent<AlienFighterCollision>();
            if (alienFighterScript != null)
            {
                Debug.Log("Missile hit alien fighter for " + damage + " damage.");
                alienFighterScript.TakeDamage(damage);
                Destroy(gameObject); // Destroy the missile after hitting
            }
        }
        // Check if we hit an alien destroyer
        else if (collision.gameObject.CompareTag("Alien Destroyer"))
        {
            AlienDestroyerCollision alienDestroyerScript = collision.gameObject.GetComponent<AlienDestroyerCollision>();
            if (alienDestroyerScript != null)
            {
                Debug.Log("Missile hit alien destroyer for " + damage + " damage.");
                alienDestroyerScript.TakeDamage(damage);
                Destroy(gameObject); // Destroy the missile after hitting
            }
        }
    }
}

