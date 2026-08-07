using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Animator flowerAnim;
    [SerializeField] private GameObject petal1;
    [SerializeField] private GameObject petal2;
    [SerializeField] private GameObject flower;
    
    private void Start()
    {
        flower.SetActive(false);
        petal1.SetActive(false);
        petal2.SetActive(false);
    }

    public void LaunchGameOverUI()
    {
        flower.SetActive(true);
        petal1.SetActive(true);
        petal2.SetActive(true);

        flowerAnim.speed = 0.5f;
        
        flowerAnim.Rebind();
        flowerAnim.Update(0f); 
        flowerAnim.Play("GameOver_Flower");

    }

    #region Buttons Callback

    public void OnRestart()
    {
        GameManager.Instance.RestartGame();
    }

    public void OnMenu()
    {
        GameManager.Instance.LoadMenu();
    }

    #endregion
}
