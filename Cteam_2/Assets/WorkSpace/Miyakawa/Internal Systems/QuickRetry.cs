using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class QuickRetry : MonoBehaviour
{
    [Header("InputSystemのRetryを指定")]
    [SerializeField] private InputActionReference Retry;

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
    }

}
