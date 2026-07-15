using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioClip musicIntro;
    [SerializeField] private AudioClip musicLoop;

    private AudioSource musicSource;

    void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;

        if (musicIntro != null)
        {
            musicSource.clip = musicIntro;
            musicSource.loop = false;
            musicSource.Play();

            if (musicLoop != null) StartCoroutine(PlayLoopAfterIntro());
        }
        else if (musicLoop != null) 
            LaunchLoopingMusic();
    }

    private IEnumerator PlayLoopAfterIntro()
    {
        yield return new WaitForSeconds(musicIntro.length);
        LaunchLoopingMusic();
    }

    private void LaunchLoopingMusic()
    {
        musicSource.clip = musicLoop;
        musicSource.loop = true;
        musicSource.Play();
    }
}
