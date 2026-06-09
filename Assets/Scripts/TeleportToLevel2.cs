using UnityEngine;

public class TeleportToLevel2 : MonoBehaviour
{
    public Transform level2SpawnPoint;
    public GameObject player;

    private CharacterController characterController;
    private Rigidbody rb;

    private void Start()
    {
        characterController = player.GetComponent<CharacterController>();
        rb = player.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            TeleportPlayer();

            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.ShowFindKeyObjective();
            }
        }
    }

    private void TeleportPlayer()
    {
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        player.transform.position = level2SpawnPoint.position;
        player.transform.rotation = level2SpawnPoint.rotation;

        if (characterController != null)
        {
            characterController.enabled = true;
        }
    }
}