using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class QuickRetry : MonoBehaviour
{
    [Tooltip("InputSystemのRetryを入れてね")]
    [SerializeField] 
    private InputActionReference Retry;

    private void Awake()
    {
        if (Retry == null) return;

        Retry.action.performed += OnHold;

        // 入力を受け取るためには必ず有効化する必要がある
        Retry.action.Enable();

    }

    private void OnHold(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Retryが開始されました。");
    }

    void Update()
    {
        // デバッグ用
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
