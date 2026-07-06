using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float levelSpeed = 2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    private void Update()
    {
        Camera.main.transform.position += new Vector3(levelSpeed * Time.deltaTime, 0, 0);   
    }

    public void GameOver()
    {
        //Debug.Log("Game Over");
        RestartGame();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
