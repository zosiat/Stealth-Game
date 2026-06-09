using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public bool hasPinkKey = false;

    [Header("Hotbar UI")]
    [Tooltip("The icon shown inside the hotbar slot when the key is collected")]
    public Image keyIcon;

    [Header("Objective UI")]
    [Tooltip("Text shown at the top of the screen until the key is picked up")]
    public TMP_Text findKeyText;

    private void Start()
    {
        UpdateKeyUI();

        if (findKeyText != null)
        {
            findKeyText.gameObject.SetActive(false);
        }
    }

    public void ShowFindKeyObjective()
    {
        if (findKeyText != null && !hasPinkKey)
        {
            findKeyText.text = "FIND THE PINK KEY";
            findKeyText.gameObject.SetActive(true);
        }
    }

    public void CollectPinkKey()
    {
        hasPinkKey = true;
        UpdateKeyUI();

        if (findKeyText != null)
        {
            findKeyText.gameObject.SetActive(false);
        }
    }

    private void UpdateKeyUI()
    {
        if (keyIcon != null)
        {
            keyIcon.enabled = hasPinkKey;
        }
    }
}
