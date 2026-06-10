using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;

    [Header("Footstep Clips")]
    public AudioClip[] footstepClips;

    [Header("Step Timing")]
    public float walkStepInterval = 0.5f;
    public float sprintStepInterval = 0.3f;
    public float crouchStepInterval = 0.7f;

    [Header("Random Variation")]
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;
    public float minVolume = 0.85f;
    public float maxVolume = 1f;

    private float stepTimer;

    void Update()
    {
        if (audioSource == null || footstepClips == null || footstepClips.Length == 0)
            return;

        bool isMoving =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow);

        if (!isMoving)
        {
            stepTimer = 0f;
            return;
        }

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isCrouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

        float currentInterval = walkStepInterval;

        if (isSprinting)
            currentInterval = sprintStepInterval;
        else if (isCrouching)
            currentInterval = crouchStepInterval;

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            PlayFootstep();
            stepTimer = currentInterval;
        }
    }

    void PlayFootstep()
    {
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        float volume = Random.Range(minVolume, maxVolume);

        audioSource.PlayOneShot(clip, volume);

        Debug.Log("Footstep played: " + clip.name);
    }
}