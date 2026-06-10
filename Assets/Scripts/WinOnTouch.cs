using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class WinOnTouch : MonoBehaviour
{
    public TextMeshProUGUI winText;
    public float restartDelay = 2f;

    private bool hasWon = false;

    private void Start()
    {
        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasWon) return;

        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null && inventory.hasPinkKey)
            {
                hasWon = true;

                if (winText != null)
                {
                    winText.text = "You escaped!";
                    winText.gameObject.SetActive(true);
                }

                StartCoroutine(RestartAfterDelay());
            }
            else
            {
                if (winText != null)
                {
                    winText.text = "You need to find the pink key first!";
                    winText.gameObject.SetActive(true);
                }
            }
        }
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}