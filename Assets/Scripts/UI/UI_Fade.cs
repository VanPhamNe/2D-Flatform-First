using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Fade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    public void FadeEffect(float target,float duration, System.Action oncomplete = null)
    {
        StartCoroutine(fadeCoroutine(target, duration,oncomplete));
    }
    private IEnumerator fadeCoroutine(float target,float duration,System.Action oncomplete)
    {
        float time = 0;
        Color currentColor = fadeImage.color;

        float startAlpha = currentColor.a;
        while(time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, target, time / duration);
            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
        fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, target);
        oncomplete?.Invoke();
    }
}
