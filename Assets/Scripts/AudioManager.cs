using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip brickHitSound;
    public AudioClip bounceSound;
    public AudioClip crashSound;

    private AudioSource audioPlayer;


    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    public void PlayBrickHitSound()
    {
        Play(brickHitSound);
    }

    public void PlayPaddleBounceSound()
    {
        PlayBounceSound();
    }

    public void PlayBrickBounceSound()
    {
        PlayBounceSound();
    }

    public void PlayWallBounceSound()
    {
        PlayBounceSound();
    }

    private void PlayBounceSound()
    {
        Play(bounceSound);
    }

    public void PlayCrashSound()
    {
        Play(crashSound);
    }

    private void Play(AudioClip clip)
    {
       audioPlayer.PlayOneShot(clip, 1.0f);
    }

}
