using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    private FirstPersonPlayer player;

    void Update()
    {
        if (player != null && Input.GetKeyDown(KeyCode.E))
        {
            bool newHiddenState = !player.IsHidden();

            player.SetHidden(newHiddenState);

            if (newHiddenState)
            {
                Debug.Log("Player Hidden");
            }
            else
            {
                Debug.Log("Player Unhidden");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        FirstPersonPlayer fp = other.GetComponent<FirstPersonPlayer>();

        if (fp != null)
        {
            player = fp;
            Debug.Log("Player entered hiding spot");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FirstPersonPlayer fp = other.GetComponent<FirstPersonPlayer>();

        if (fp != null)
        {
            Debug.Log("Player left hiding spot");

            if (player != null && player.IsHidden())
            {
                player.SetHidden(false);
                Debug.Log("Player Unhidden");
            }

            player = null;
        }
    }
}