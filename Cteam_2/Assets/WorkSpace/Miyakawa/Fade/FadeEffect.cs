using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class FadeEffect : MonoBehaviour
{
    [Header("Eventsを指定")]
    [SerializeField] private Events events;

    [Header("フェード用イメージを指定")]
    [SerializeField]　private Image fadeImage;
  
    [Header("フェードイン時間")]
    [SerializeField] private float fadeinTime = 1.0f;
    
    [Header("フェードアウト時間")]
    [SerializeField] private float fadeoutTime = 1.0f;
    
    [Header("フェードイン前の待ち時間")]
    [SerializeField] private float beforeFadeinTime = 1.0f;
    
    [Header("フェードアウト前の待ち時間")]
    [SerializeField] private float beforeFadeoutTime = 1.0f; 

    private float fadeDuration = 1.0f;
    private float beforeWaitTime = 1.0f;

    public void StartFadeIn()
    {
        fadeDuration = fadeinTime;
        beforeWaitTime = beforeFadeinTime;
        StartCoroutine(fade(false));
        Debug.Log("フェードイン処理実行");
    }
    public void StartFadeOut()
    {
        fadeDuration = fadeoutTime;
        beforeWaitTime = beforeFadeoutTime;
        StartCoroutine(fade(true));
        Debug.Log("フェードアウト処理実行");
    }

    void Update()
    {

    }


    private IEnumerator fade(bool isFadeOut)
    {
    float startAlphaColor = fadeImage.color.a;
    float targetAlphaColor = 0;
    float elapsedTime = 0f;

        if (isFadeOut)
        {
            targetAlphaColor = 1;
            yield return new WaitForSeconds(beforeWaitTime);
        }
        else
        {
            yield return new WaitForSeconds(beforeWaitTime);
        }

        // 開始時のアルファ値を保存
        float initialAlphaColor = startAlphaColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            startAlphaColor = Mathf.Lerp(
                initialAlphaColor,
                targetAlphaColor,
                elapsedTime / fadeDuration
            );

            fadeImage.color = new Color(0f, 0f, 0f, startAlphaColor);

            yield return null;
        }

        // 最終的なアルファ値を設定
        fadeImage.color = new Color(0f, 0f, 0f, targetAlphaColor);

        yield return new WaitForSeconds(2f);

        if (isFadeOut)
        {
            events.EndFadein.Invoke();
        }
        else
        {
            events.EndFadeout.Invoke();
        }
            Debug.Log("フェードイン終了");

    }
}
