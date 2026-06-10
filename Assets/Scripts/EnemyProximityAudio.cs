using UnityEngine;

// Plays a looping sound whose volume scales with distance to the player.
// Attach directly to an enemy; it creates its own AudioSource at runtime,
// so it won't conflict with other AudioSources on the same object.
public class EnemyProximityAudio : MonoBehaviour
{
    [Header("references")]
    public Transform player;
    [Tooltip("The looping sound that gets louder as the enemy gets closer")]
    public AudioClip proximityClip;

    [Header("distance settings")]
    [Tooltip("At this distance or closer, the sound plays at max volume")]
    public float minDistance = 2f;
    [Tooltip("Beyond this distance the sound is silent")]
    public float maxDistance = 15f;

    [Header("volume settings")]
    [Range(0f, 1f)]
    public float maxVolume = 1f;
    [Tooltip("How quickly the volume adjusts (higher = snappier)")]
    public float fadeSpeed = 5f;

    [Header("detection gating")]
    [Tooltip("If true, only audible while the guard is chasing the player")]
    public bool onlyWhenChasing = true;

    private AudioSource audioSource;
    private GuardAI guard;

    void Start()
    {
        guard = GetComponentInParent<GuardAI>();

        // Dedicated source so we never touch the guard's detection AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = proximityClip;
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.playOnAwake = false;

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    void Update()
    {
        if (player == null || audioSource == null || audioSource.clip == null) return;

        float targetVolume = 0f;

        bool detected = !onlyWhenChasing || guard == null || guard.IsChasing();

        if (detected)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // 1 when at minDistance or closer, 0 at maxDistance or farther
            float proximity = Mathf.InverseLerp(maxDistance, minDistance, distance);
            targetVolume = proximity * maxVolume;
        }

        audioSource.volume = Mathf.MoveTowards(
            audioSource.volume,
            targetVolume,
            fadeSpeed * Time.deltaTime
        );
    }
}
