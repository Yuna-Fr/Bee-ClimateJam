using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level")]
    [SerializeField] private float levelSpeed = 2f;

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

        gameoverScreen.gameObject.SetActive(false);
        victoryScreen.gameObject.SetActive(false);
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
        victoryScreen.gameObject.SetActive(true);

        stopLevel = true;
        if (victoryScreen == null) return;

        victoryScreen.DOFade(1f, gameOverFade)
            .onComplete += () => victoryScreen.gameObject.GetComponent<VicrotyUI>().LaunchUI();
    }

    private void GameOver()
    {
        gameoverScreen.gameObject.SetActive(true);

        stopLevel = true;
        BeeController.Instance.DiesAnim(1f);
        gameoverScreen.DOFade(1f, gameOverFade)
            .onComplete += () => gameoverScreen.gameObject.GetComponent<GameOverUI>().LaunchGameOverUI();
    }

    #region GameOver Buttons Callbacks
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    #endregion
}
