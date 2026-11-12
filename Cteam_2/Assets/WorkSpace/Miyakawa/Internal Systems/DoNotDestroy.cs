using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    public bool[] isStageClear;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // ƒV[ƒ“‘JˆÚŒã‚àc‚·
    }

    public void CheckStageClear()
    {
        isStageClear = new bool[11] { false, false, false, false, false, false, false, false, false, false ,false};
    }
}

