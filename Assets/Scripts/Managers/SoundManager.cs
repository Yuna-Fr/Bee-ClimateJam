using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private List<AudioClip> musicInOrder;
    [SerializeField] private AudioClip musicLoop;

    private AudioSource musicSource;

    void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;

        if (musicInOrder.Count != 0) 
            StartCoroutine(PlayInOrder());
    
        else if (musicLoop != null) 
            LaunchLoopingMusic();
    }

    private IEnumerator PlayInOrder()
    {
        foreach(var music in musicInOrder)
        {
            musicSource.clip = music;
            musicSource.loop = false;
            musicSource.Play();

            yield return new WaitForSeconds(music.length);
        }

        LaunchLoopingMusic();
    }

    private void LaunchLoopingMusic()
    {
        musicSource.clip = musicLoop;
        musicSource.loop = true;
        musicSource.Play();
    }
}
