using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NUnit.Framework;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sound")]
    [SerializeField] private List<AudioClip> flowerAlive;
    [SerializeField] private List<AudioClip> flowerPop;
    [SerializeField] private List<AudioClip> flowerGrow;

    [SerializeField] private List<AudioClip> branchCol;
    [SerializeField] private List<AudioClip> trunkCol;
    [SerializeField] private AudioSource soundSource;

    [Header("Music")]
    [SerializeField] private float fadeTransition = 1f;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource hornetMusicSource;
    [SerializeField] private List<AudioClip> musicInOrder;
    [SerializeField] private AudioClip musicLoop;
    [SerializeField] private AudioClip looseMusic;
    [SerializeField] private AudioClip winMusic;

    private bool chaseActive;
    private float chaseEndTime;
    private TweenerCore<float, float, FloatOptions> fadeTween;
    private Coroutine musicCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        musicSource.playOnAwake = false;
        musicSource.volume = 0f;
        hornetMusicSource.volume = 0f;
        musicSource.DOFade(1f, fadeTransition);

        if (musicInOrder.Count != 0) 
            musicCoroutine = StartCoroutine(PlayInOrder());
    
        else if (musicLoop != null) 
            LaunchLoopingMusic();
    }

    private void Update()
    {
        if (chaseActive && Time.time >= chaseEndTime - 1)
        {
            chaseActive = false;
            fadeTween?.Kill();
            fadeTween = hornetMusicSource.DOFade(0f, fadeTransition * 2);
        }
    }

    public void PlayCollision(bool isTrunk)
    {
        if (soundSource.isPlaying) return;

        soundSource.clip = isTrunk ? trunkCol[Random.Range(0, trunkCol.Count)] : branchCol[Random.Range(0, branchCol.Count)];
        soundSource.Play();
    }

    public AudioClip GetFlowerAliveSound() { return flowerAlive[Random.Range(0, flowerAlive.Count)]; }
    public AudioClip GetFlowerPopSound() { return flowerPop[Random.Range(0, flowerPop.Count)]; }
    public AudioClip GetFlowerGrowSound() { return flowerGrow[Random.Range(0, flowerGrow.Count)]; }

    #region Music Gestion

    public void LaunchChaseMusic(float duration)
    {
        chaseEndTime = Mathf.Max(chaseEndTime, Time.time + duration);

        if (!chaseActive)
        {
            chaseActive = true;

            fadeTween?.Kill();
            fadeTween = hornetMusicSource.DOFade(1f, 0.5f);
        }
    }

    public void FadeOutMusic(float fadeDelay)
    {
        musicSource.DOFade(0f, fadeDelay);
        hornetMusicSource.DOFade(0f, fadeDelay);
    }

    public void SwitchToEndMusic(bool isWinLoop)
    {
        hornetMusicSource.DOFade(0f, fadeTransition);
        musicSource.DOFade(0f, fadeTransition).OnComplete(() =>
        {
            if (musicCoroutine != null)
            {
                StopCoroutine(musicCoroutine);
                musicCoroutine = null;
            }
            
            musicSource.clip = isWinLoop ? winMusic : looseMusic;
            musicSource.loop = true;
            musicSource.Play();
            musicSource.DOFade(1f, fadeTransition);
        });
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
    #endregion
}
