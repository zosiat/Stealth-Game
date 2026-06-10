using UnityEngine;

public class HideAbilityIntro : MonoBehaviour
{
    public float displayTime = 5f;

    void Start()
    {
        Invoke(nameof(HideText), displayTime);
    }

    void HideText()
    {
        gameObject.SetActive(false);
    }
}