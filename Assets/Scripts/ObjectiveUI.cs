using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    public float displayTime = 2f;
    public float fadeDuration = 1f;

    private TMP_Text textObject;

    void Start()
    {
        textObject = GetComponent<TMP_Text>();

        Invoke("StartFade", displayTime);
    }

    void StartFade()
    {
        StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        Color originalColor = textObject.color;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            textObject.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );

            yield return null;
        }

        gameObject.SetActive(false);
    }
}