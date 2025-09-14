using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource; 

    [Header("Sound Effects")]
    [SerializeField] private AudioClip flipClip;
    [SerializeField] private AudioClip matchClip;
    [SerializeField] private AudioClip mismatchClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip gameOverClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayFlip()
    {
        PlaySFX(flipClip);
    }

    public void PlayMatch()
    {
        PlaySFX(matchClip);
    }

    public void PlayMismatch()
    {
        PlaySFX(mismatchClip);
    }

    public void PlayWin()
    {
        PlaySFX(winClip);
    }

    public void PlayGameOver()
    {
        PlaySFX(gameOverClip);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
