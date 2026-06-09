using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public bool hasPinkKey = false;
    public TMP_Text keyStatusText;

    private void Start()
    {
        UpdateKeyUI();
    }

    public void CollectPinkKey()
    {
        hasPinkKey = true;
        UpdateKeyUI();
    }

    private void UpdateKeyUI()
    {
        if (keyStatusText != null)
        {
            if (hasPinkKey)
            {
                keyStatusText.text = "Pink Key: Collected";
            }
            else
            {
                keyStatusText.text = "Pink Key: Not Collected";
            }
        }
    }
}