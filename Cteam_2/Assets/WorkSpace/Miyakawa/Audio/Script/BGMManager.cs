using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [Header("BGMのリスト")]
    [SerializeField]
    private List<AudioClip> bgmList =
        new List<AudioClip>();

    [Header("BGMを再生するAudioSource")]
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            return;
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }
    //デバッグ用
    private void Start()
    {
        PlayBGM(0);
    }

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.I))
    //    {
    //        StopBGM();
    //    }
    //}

    public void PlayBGM(int index)
    {
        if (audioSource == null)
        {
            return;
        }

        if (index < 0 || index >= bgmList.Count)
        {
            return;
        }

        if (bgmList[index] == null)
        {
            return;
        }

        audioSource.clip = bgmList[index];
        audioSource.Play();
    }

    public void StopBGM()
    {
        if (audioSource == null)
        {
            return;
        }

        audioSource.Stop();
    }

}
