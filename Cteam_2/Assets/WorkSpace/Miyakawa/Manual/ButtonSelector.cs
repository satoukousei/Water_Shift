using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    [Header("ボタンのImageを指定")]
    [SerializeField] private GameObject RetryButton;
    [SerializeField] private GameObject SelectStageButton;
    [SerializeField] private GameObject ManualButton;
    [Header("操作説明のImageを指定")]
    [SerializeField] private Image manualImage;

    public bool isManualImageActive = false;

    public enum MenuButtonState
    {
        Retry,
        SelectStage,
        Manual
    }

    public MenuButtonState menucurrentState = MenuButtonState.Retry;

    void Start()
    {
        ScaleUpSelectedButton();
        isManualImageActive = false;
    }

    public void CurrentUp()
    {
        menucurrentState = (MenuButtonState)(((int)menucurrentState + 2) % 3);
        ScaleUpSelectedButton();
    }

    public void CurrentDown()
    {
        menucurrentState = (MenuButtonState)(((int)menucurrentState + 1) % 3);
        ScaleUpSelectedButton();
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
        if(manualImage != null)
        {
            manualImage.color = new Color(0f, 0f, 0f, 1f);
            isManualImageActive = true;
        }
    }
}
