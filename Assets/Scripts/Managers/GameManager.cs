using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level")]
    [SerializeField] private float levelSpeed = 2f;

    [Header("GameOver")]
    [SerializeField] private float gameOverFade = 2f;
    [SerializeField] private CanvasGroup gameoverScreen;

    [Header("Victory")]
    [SerializeField] private CanvasGroup victoryScreen;

    private bool stopLevel = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    private void Update()
    {
        if (stopLevel) return;

        Camera.main.transform.position += new Vector3(levelSpeed * Time.deltaTime, 0, 0);   
    }

    public void GameOver()
    {
        stopLevel = true;
        BeeController.Instance.DiesAnim(1f);
        gameoverScreen.DOFade(1f, gameOverFade)
            .onComplete += () => gameoverScreen.gameObject.GetComponent<GameOverUI>().LaunchGameOverUI() ;
    }

    public void Victory()
    {
        stopLevel = true;
        if (victoryScreen != null)
            victoryScreen.DOFade(1f, gameOverFade);
        else if (gameoverScreen != null)
            gameoverScreen.DOFade(0.5f, gameOverFade);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
