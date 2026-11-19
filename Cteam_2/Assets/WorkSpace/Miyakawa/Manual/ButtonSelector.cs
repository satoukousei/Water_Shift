using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    [Header("SoundEffectManagerを指定")]
    [SerializeField] private SoundEffectManager seManager;

    [Header("ボタンのImageを指定")]
    [SerializeField] private GameObject RetryButton;
    [SerializeField] private GameObject SelectStageButton;
    [SerializeField] private GameObject ManualButton;
    [Header("操作説明のImageを指定")]
    [SerializeField] private Image manualImage;

    private bool isManualImageActive = false;

    public enum MenuButtonState
    {
        Retry,
        SelectStage,
        Manual
    }

    public MenuButtonState menucurrentState = MenuButtonState.Retry;

    private void Start()
    {
        ScaleUpSelectedButton();
        HideManualImage();
    }

    public void CurrentUp()
    {
        menucurrentState = (MenuButtonState)(((int)menucurrentState + 2) % 3);
        seManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.ButtonSelecting);
        ScaleUpSelectedButton();
        Debug.Log("現在の数値" + menucurrentState);
    }

    public void CurrentDown()
    {
        menucurrentState = (MenuButtonState)(((int)menucurrentState + 1) % 3);
        seManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.ButtonSelecting);
        ScaleUpSelectedButton();
        Debug.Log("現在の数値" + menucurrentState);
    }

    private void ScaleUpSelectedButton()
    {
        switch (menucurrentState)
        {
            case MenuButtonState.Retry:
                RetryButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                SelectStageButton.transform.localScale = Vector3.one;
                ManualButton.transform.localScale = Vector3.one;
                break;
            case MenuButtonState.SelectStage:
                RetryButton.transform.localScale = Vector3.one;
                SelectStageButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                ManualButton.transform.localScale = Vector3.one;
                break;
            case MenuButtonState.Manual:
                RetryButton.transform.localScale = Vector3.one;
                SelectStageButton.transform.localScale = Vector3.one;
                ManualButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                break;
        }
    }

    public void ActiveManualImage()
    {
        if (manualImage == null)
        {
            return;
        }

        manualImage.gameObject.SetActive(true);
        isManualImageActive = true;
    }

    public void HideManualImage()
    {
        isManualImageActive = false;

        manualImage.gameObject.SetActive(false);
    }

    public void ResetSelection()
    {
        menucurrentState = MenuButtonState.Retry;
        ScaleUpSelectedButton();
    }
}
