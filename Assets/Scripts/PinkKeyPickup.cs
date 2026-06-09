using UnityEngine;

public class PinkKeyPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.hasPinkKey = true;
            Debug.Log("Pink key collected!");
            gameObject.SetActive(false);
        }
    }
}