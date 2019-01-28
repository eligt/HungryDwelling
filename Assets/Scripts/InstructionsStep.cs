using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsStep : MonoBehaviour
{
    public float startTime;
    public float duration;

    [HideInInspector]
    public bool shown;
    public float startedTime;

    private Color originalColor;
    private Color endColor;
    private float fadeDuration = 1.0f;
    private float fadeTime = 0.0f;
    bool isFading = false;
        
	void Awake ()
    {
        shown = false;
        startedTime = 0.0f;

        Image img = GetComponent<Image>();

        if (img != null)
            endColor = originalColor = img.color;
    }

    private void Update()
    {
        if (isFading)
        {
            if (fadeTime > fadeDuration)
            {
                //gameObject.SetActive(false);
                isFading = false;
                return;
            }

            Image img = GetComponent<Image>();

            if (img != null)
            {
                img.color = Color.Lerp(img.color, endColor, fadeTime / fadeDuration);
            }

            fadeTime += Time.deltaTime;
        }
    }

    public void Show(bool show)
    {
        if (show)
            endColor.a = originalColor.a;
        else
            endColor.a = 0.0f;

        fadeTime = 0.0f;
        //isFading = true;

        gameObject.SetActive(show);
    }

    public virtual bool meetsCondition()
    {
        return true;
    }
}
