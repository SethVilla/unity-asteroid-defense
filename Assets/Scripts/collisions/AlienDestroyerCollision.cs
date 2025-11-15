using UnityEngine;

public class AlienDestroyerCollision : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] private float hp = 100f;
    [SerializeField] private float maxHP = 100f;

    private AudioManager audioManager;
    private bool isDestroyed = false;

    void Awake()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
            audioManager = audioObject.GetComponent<AudioManager>();
    }

    void Start()
    {
        hp = Random.Range(500f, 1000f);
        maxHP = hp;
    }

    public void TakeDamage(float incomingDamage)
    {
        // Prevent taking damage if already destroyed
        if (isDestroyed) return;
        
        hp -= incomingDamage;
        Debug.Log("Alien Destroyer HP: " + hp + "/" + maxHP);

        if (hp <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            
            if (audioManager != null && audioManager.explosions != null && audioManager.explosions.Length > 0)
            {
                audioManager.PlaySFX(audioManager.explosions[Random.Range(0, audioManager.explosions.Length)]);
            }

            if (GameUI.Instance != null)
            {
                int scoreIncrease = 500;
                GameUI.Instance.IncreaseScore(scoreIncrease);
            }
            
            // Notify LevelManager that an Alien Destroyer was destroyed (only once!)
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnAlienDestroyerDestroyed();
                Debug.Log($"Destroyer {gameObject.name} notified LevelManager of destruction");
            }

            Destroy(gameObject);
        } else {
            if (audioManager != null && audioManager.ememyImpact != null)
            {
                audioManager.PlaySFX(audioManager.ememyImpact);
            }
        }
    }

    public float GetHP()
    {
        return hp;
    }

    public float GetMaxHP()
    {
        return maxHP;
    }
}

