using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class FadeEffect : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;
    [Tooltip("フェード時間")]
    [SerializeField] private float fadeDuration = 1.0f;
    [Tooltip("暗転前の待ち時間")]
    [SerializeField] private float beforeWaitTime = 1.0f;

    public void StartFadeIn()
    {
        StartCoroutine(fade(false));
    }
    public void StartFadeOut()
    {
        StartCoroutine(fade(true));
    }

    void Update()
    {
        //デバッグ用のキー入力処理
        if (Input.GetKey(KeyCode.F))
        {
            StartCoroutine(fade(true));
            Debug.Log("フェードアウト処理を実行");
        }
        if (Input.GetKey(KeyCode.G))
        {
            StartCoroutine(fade(false));
            Debug.Log("フェードアウト処理を実行");
        }
    }

    private IEnumerator fade(bool isFadeOut)
    {
        float startAlphaColor = fadeImage.color.a;
        float targetAlphaColor = 0;
        float elapsedTime = 0f;

        if(isFadeOut)
        {
            targetAlphaColor = 1;
            yield return new WaitForSeconds(beforeWaitTime);
        }
        else
        {
            yield return new WaitForSeconds(beforeWaitTime);
        }
        

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                startAlphaColor = Mathf.Lerp(startAlphaColor, targetAlphaColor, elapsedTime/fadeDuration);
                fadeImage.color = new Color(0f,0f,0f,startAlphaColor);
                yield return null;
            }
        
    }
}
