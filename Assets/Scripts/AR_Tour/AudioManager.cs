using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgAudioSource;
    public AudioSource voiceOverAudioSource;

    [Header("Audio Clips")]
    public AudioClip[] voiceOverClips;
    public AudioClip visitAgainClip;

    public int currentClipIndex = 0;
    public CharacterPatrol character;

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

        // Debug message for the first clip
        if (currentClipIndex == 0)
        {
            Debug.Log("Playing the intro clip.");
        }

        FadeBGMusic();
        voiceOverAudioSource.Play();

        // Use a coroutine to check when the clip ends
        StartCoroutine(CheckClipEnd());

        currentClipIndex++;
    }

    IEnumerator CheckClipEnd()
    {
        while (voiceOverAudioSource.isPlaying)
        {
            yield return null;
        }

        ClipToEnd();

        // Debug message for the first clip completion
        if (currentClipIndex == 1)
        {
            Debug.Log("The intro clip has finished playing.");
            character.PlayHandAnim();
        }
        if (currentClipIndex == voiceOverClips.Length)
        {
            Debug.Log("All clips have finished playing.");
            StartCoroutine(DelayedVisitAgainClip());
            InfoManager.Instance.highLightCam.SetActive(false);
        }
    }

    IEnumerator DelayedVisitAgainClip()
    {
        yield return new WaitForSeconds(2f); // 2 seconds delay
        PlayVisitAgainClip();
    }

    public void PlayVisitAgainClip()
    {
                    Debug.Log("Playing 'Visit Again' clip.  AFter 2f ");

        if (visitAgainClip != null)
        {
            voiceOverAudioSource.clip = visitAgainClip;
            voiceOverAudioSource.Play();
            Debug.Log("Playing 'Visit Again' clip.");
        }
        else
        {
            Debug.LogWarning("Visit Again clip is not assigned!");
        }
    }

    public void ClipToEnd()
    {
        voiceOverAudioSource.Stop();
        Debug.Log("Voice-over clip has finished playing: " + currentClipIndex);
        if (currentClipIndex == voiceOverClips.Length)
        {
            Debug.Log("All listed clips completed. Playing 'Visit Again' clip.");
            // PlayVisitAgainClip(); // This is already handled in CheckClipEnd()
        }
        LoudBGMusic();
    }

    public void LoudBGMusic()
    {
        bgAudioSource.volume = 1f;
    }

    public void FadeBGMusic()
    {
        bgAudioSource.volume = 0.15f;
    }

    public bool IsVoiceOverPlaying()
    {
        Debug.Log("voiceOverAudioSource: " + voiceOverAudioSource.clip);
        return voiceOverAudioSource.isPlaying;
    }
}
