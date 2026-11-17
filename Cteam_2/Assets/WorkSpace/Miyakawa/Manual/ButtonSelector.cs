using UnityEngine;

public class ButtonSelector : MonoBehaviour
{
    [Header("ƒ{ƒ^ƒ“‚ÌImage‚ðŽw’è")]
    [SerializeField] private GameObject RetryButton;
    [SerializeField] private GameObject SelectStageButton;
    [SerializeField] private GameObject ManualButton;

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
}
