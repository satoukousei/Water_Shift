using UnityEngine;
using UnityEngine.InputSystem;

public class PhaseChange_Water : MonoBehaviour
{
    public InputActionAsset inputAction_PhaseChanger; // Inspectorで設定
    InputAction Water;

    private void Awake()
    {
        // "Player"という名前のアクションマップをInputActionAssetから探し出します。
        var actionMap = inputAction_PhaseChanger.FindActionMap("PhaseChange");

        // アクションマップ内から"CKey"という名前のアクションを取得し、cKeyActionに割り当てます。
        Water = actionMap.FindAction("Water");
    }

    private void Start()
    {

    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    void Update()
    {
        // 毎フレーム呼ばれるUpdateメソッド内で、Cキーがこのフレームで押されたかを確認します。
        if (Water.WasPressedThisFrame())
        {
            // Cキーが押された場合に、メッセージをコンソールに表示します。
            Debug.Log("状態を液体に変更。");
        }
    }
}
