using UnityEngine;
using System.Collections;

public class AnimationEventOpr : MonoBehaviour
{

    public AudioSource audioSouce;
    void Start()
    {
       
    }
    /// <summary>
    /// 根据路径播放AnimationEvent事件引发的声音
    /// </summary>
    /// <param name="audioPath"></param>
    public void PlayAudio(string audioPath)
    {
        if (audioSouce == null)
        {
            audioSouce = gameObject.audio;
        }
        PlayAudioWithSource(audioPath, audioSouce);
    }

    private void PlayAudioWithSource(string audioPath, AudioSource audioSource)
    {
        if (audioSource != null && !string.IsNullOrEmpty(audioPath))
        {
            AudioClip audioClip = TGameCore.GetIntance().GetResourceMgr().GetResourceByPath<AudioClip>(audioPath);
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
    /// <summary>
    /// 根据资源播放AnimationEvent事件引发的声音
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlayAudioClip(AudioClip audioClip)
    {
        if (audioSouce == null)
        {
            audioSouce = gameObject.audio;
        }
        PlayAudioClipWithSource(audioClip, audio);
    }

    private void PlayAudioClipWithSource(AudioClip audioClip, AudioSource audioSource)
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
