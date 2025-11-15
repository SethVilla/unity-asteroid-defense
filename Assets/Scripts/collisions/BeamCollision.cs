using UnityEngine;

public class BeamCollision : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damage = 15f;

    private AudioManager audioManager;

    void Awake()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
            audioManager = audioObject.GetComponent<AudioManager>();
    }

    void Start()
    {
        damage = Random.Range(15f, 25f);
    }

    public float GetDamage()
    {
        return damage;
    }
}

