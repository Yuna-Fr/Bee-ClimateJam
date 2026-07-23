using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [HideInInspector] public Action OnGameEnd;

    [Header("Level")]
    [SerializeField] private float levelSpeed = 2f;
    [SerializeField] private float fadeInDelay = 2f;
    [SerializeField] private CanvasGroup fadeBG;

    [Header("Difficulty")]
    [SerializeField] private Image heart;

    [Header("GameOver")]
    [SerializeField] private float gameOverFade = 2f;
    [SerializeField] private CanvasGroup gameoverScreen;

    [Header("Victory")]
    [SerializeField] private CanvasGroup victoryScreen;

    private bool stopLevel = false;
    private int currentLife = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        currentLife = 2;
        heart.fillAmount = 1f;

        fadeBG.alpha = 1f;

        gameoverScreen.gameObject.SetActive(false);
        victoryScreen.gameObject.SetActive(false);
    }

    private void Start()
    {
        fadeBG.DOFade(0f, fadeInDelay * 2);
    }

    private void Update()
    {
        if (stopLevel) return;

        Camera.main.transform.position += new Vector3(levelSpeed * Time.deltaTime, 0, 0);   
    }

    public void RemoveAHeart()
    {
        if (currentLife <= 0) return; // Already dead

        currentLife--;

        if (currentLife == 1)
            heart.fillAmount = 0.43f;
            
        else if (currentLife <= 0)
        {
            heart.fillAmount = 0.0f;
            GameOver();
        }
    }

    public void Victory()
    {
        stopLevel = true;
        OnGameEnd.Invoke();

        SoundManager.Instance.SwitchToEndMusic(true);

        victoryScreen.gameObject.SetActive(true);
        victoryScreen.DOFade(1f, gameOverFade)
            .onComplete += () => victoryScreen.gameObject.GetComponent<VicrotyUI>().LaunchUI();
    }

    private void GameOver()
    {
        stopLevel = true;
        OnGameEnd.Invoke();

        SoundManager.Instance.SwitchToEndMusic(false);

        gameoverScreen.gameObject.SetActive(true);
        BeeController.Instance.DiesAnim(1f);
        gameoverScreen.DOFade(1f, gameOverFade)
            .onComplete += () => gameoverScreen.gameObject.GetComponent<GameOverUI>().LaunchGameOverUI();
    }

    #region GameOver Buttons Callbacks
    public void RestartGame()
    {
        SoundManager.Instance.FadeOutMusic(fadeInDelay);
        fadeBG.DOFade(1f, fadeInDelay)
            .OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }

    public void LoadMenu()
    {
        SoundManager.Instance.FadeOutMusic(fadeInDelay * 2);
        fadeBG.DOFade(1f, fadeInDelay * 2)
            .OnComplete(() => SceneManager.LoadScene("Menu"));
    }
    #endregion
}
