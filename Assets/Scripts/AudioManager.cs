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
        Debug.Log("Brick Hit Sound");
    }

    public void PlayPaddleBounceSound()
    {
        PlayBounceSound();
        Debug.Log("Paddle Hit Sound");
    }

    public void PlayWallBounceSound()
    {
        PlayBounceSound();
        Debug.Log("Wall Bounce Sound");
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
