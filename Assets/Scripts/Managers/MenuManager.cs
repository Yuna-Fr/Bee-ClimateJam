using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private float changeSceneDelay = 0.4f;


    [Header("Credit Leaf")]
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private Vector3 leafUpPos;
    [SerializeField] private GameObject creditLeaf;

    private Vector3 leafDownPos;
    private bool isCreditOpen = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        leafDownPos = creditLeaf.transform.localRotation.eulerAngles;
    }

    #region Button Callbacks
    
    public void OnStartButtonPressed()
    {
        StartCoroutine(StartGameCoroutine());

        IEnumerator StartGameCoroutine()
        {
            yield return new WaitForSeconds(changeSceneDelay);
            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnCreditsButtonPressed()
    {
        isCreditOpen = !isCreditOpen;

        if (isCreditOpen)
            OnCreditOpened();
        else
            OnCreditClosed();
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    } 

    #endregion

    private void OnCreditOpened()
    {
        creditLeaf.transform.DOLocalRotate(leafUpPos, moveDuration);
    }

    private void OnCreditClosed()
    {
        creditLeaf.transform.DOLocalRotate(leafDownPos, moveDuration);
    }
}

