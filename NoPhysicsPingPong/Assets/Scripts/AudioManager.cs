using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioManager;

    public AudioClip countdownStart;
    public AudioClip countdownEnd;
    public AudioClip countdownGo;
    public AudioClip ballHitted;
    public AudioClip ballHittedLimits;
    public AudioClip ballDestroyed;

    void Start()
    {
        audioManager = GetComponent<AudioSource>();
    }

    public void PlayCountdownStart()
    {
        audioManager.PlayOneShot(countdownStart);
    }

    public void PlayCountdownGo()
    {
        audioManager.PlayOneShot(countdownGo);
    }

    public void BallHittedPaddle()
    {
        audioManager.PlayOneShot(ballHitted);
    }

    public void BallHittedLimits()
    {
        audioManager.PlayOneShot(ballHittedLimits);
    }

    public void BallDestroyed()
    {
        audioManager.PlayOneShot(ballDestroyed);
    }
}