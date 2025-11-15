using UnityEngine;

public class AlienFighterCollision : MonoBehaviour
{
    [Header("HP and Damage Settings")]
    [SerializeField] private float hp = 12f;
    [SerializeField] private float damage = 5f;

    private AudioManager audioManager;

    void Awake()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
            audioManager = audioObject.GetComponent<AudioManager>();
    }

    void Start()
    {
        hp = Random.Range(10f, 12f);
        damage = Random.Range(5f, 10f);
    }

    public void TakeDamage(float incomingDamage)
    {
        hp -= incomingDamage;

        if (hp <= 0)
        {
            if (audioManager != null && audioManager.explosions != null && audioManager.explosions.Length > 0)
            {
                audioManager.PlaySFX(audioManager.explosions[Random.Range(0, audioManager.explosions.Length)]);
            }

            if (GameUI.Instance != null)
            {
                int scoreIncrease = Mathf.RoundToInt(damage * 20);
                GameUI.Instance.IncreaseScore(scoreIncrease);
            }

            Destroy(gameObject);
        } else {
            if (audioManager != null && audioManager.ememyImpact != null)
            {
                audioManager.PlaySFX(audioManager.ememyImpact);
            }
        }
    }

    public float GetDamage()
    {
        return damage;
    }
}

