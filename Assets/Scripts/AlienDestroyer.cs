using UnityEngine;

public class AlienDestroyer : MonoBehaviour
{
    [Header("Hardpoints")]
    [SerializeField] private Transform hardpoint;
    [SerializeField] private Transform hardpointLeft;
    [SerializeField] private Transform hardpointRight;

    [Header("Weapon Prefabs")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject beamPrefab;

    [Header("Fire Settings")]
    [SerializeField] private float beamFireRate = 2f;
    [SerializeField] private float missileFireRate = 0.5f;

    private float beamTimer = 0f;
    private float missileTimer = 0f;
    private bool useLeftHardpoint = true;
    private bool isFiringBeam = false;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        beamTimer += Time.deltaTime;
        missileTimer += Time.deltaTime;

        // Fire beam every beamFireRate seconds
        if (beamTimer >= beamFireRate)
        {
            FireBeam();
            beamTimer = 0f;
            isFiringBeam = true;
            missileTimer = 0f; // Reset missile timer when firing beam
        }
        // Fire missiles when not firing beam
        else if (missileTimer >= missileFireRate && !isFiringBeam)
        {
            FireMissile();
            missileTimer = 0f;
        }

        // Reset beam firing flag after a short delay
        if (isFiringBeam && missileTimer >= 0.1f)
        {
            isFiringBeam = false;
        }
    }

    private void FireBeam()
    {
        if (beamPrefab == null || hardpoint == null)
        {
            Debug.LogWarning("BeamPrefab or Hardpoint not assigned!");
            return;
        }

        // Adjust beam position with Z offset of 2
        Vector3 beamPosition = hardpoint.position + new Vector3(0f, 0f, 0f);
        audioManager.PlaySFX(audioManager.beam);
        GameObject beam = Instantiate(beamPrefab, beamPosition, Quaternion.Euler(90f, 0f, 0f));
        
        // Destroy beam after 1 second
        Destroy(beam, 1f);
    }

    private void FireMissile()
    {
        if (missilePrefab == null)
        {
            Debug.LogWarning("MissilePrefab not assigned!");
            return;
        }

        // Alternate between left and right hardpoints
        Transform spawnPoint = useLeftHardpoint ? hardpointLeft : hardpointRight;

        if (spawnPoint == null)
        {
            Debug.LogWarning($"Hardpoint{(useLeftHardpoint ? "Left" : "Right")} not assigned!");
            return;
        }

        // Create rotation with X = -150
        Quaternion missileRotation = Quaternion.Euler(-150f, spawnPoint.rotation.eulerAngles.y, spawnPoint.rotation.eulerAngles.z);
        Instantiate(missilePrefab, spawnPoint.position, missileRotation);

        // Toggle hardpoint for next missile
        useLeftHardpoint = !useLeftHardpoint;
    }

    private void OnDrawGizmos()
    {
        // Draw hardpoint locations for debugging
        Gizmos.color = Color.yellow;
        if (hardpoint != null)
        {
            Gizmos.DrawWireSphere(hardpoint.position, 0.5f);
        }

        Gizmos.color = Color.red;
        if (hardpointLeft != null)
        {
            Gizmos.DrawWireSphere(hardpointLeft.position, 0.3f);
        }

        Gizmos.color = Color.blue;
        if (hardpointRight != null)
        {
            Gizmos.DrawWireSphere(hardpointRight.position, 0.3f);
        }
    }
}

