using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgAudioSource;
    public AudioSource voiceOverAudioSource;
   // public int voiceClip;

    [Header("Audio Clips")]
    public AudioClip[] voiceOverClips;

    public int currentClipIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (bgAudioSource == null || voiceOverAudioSource == null)
        {
            Debug.LogError("AudioSources are not assigned!");
            return;
        }
    }

    public void PlayNextVoiceOverClip()
    {
        if (currentClipIndex >= voiceOverClips.Length)
        {
            Debug.Log("All voice-over clips have been played.");
            return;
        }

        AudioClip currentClip = voiceOverClips[currentClipIndex];
        voiceOverAudioSource.clip = currentClip;
        FadeBGMusic();
        voiceOverAudioSource.Play();
        currentClipIndex++;
    }

    public void ClipToEnd()
    {
        voiceOverAudioSource.Stop();
        Debug.Log("Voice-over clip has finished playing.");
        LoudBGMusic();
    }

    public void LoudBGMusic()
    {
        bgAudioSource.volume = 1f;
    }

    public void FadeBGMusic()
    {
        bgAudioSource.volume = 0.25f;
    }

    // New method to check if voice-over is playing
    public bool IsVoiceOverPlaying()
    { 
        Debug.Log("voiceOverAudioSource : " + voiceOverAudioSource.clip);
        return voiceOverAudioSource.isPlaying;
    }
}
