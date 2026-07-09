using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private float changeSceneDelay = 0.4f;


    [Header("Credit Leaf")]
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private Vector3 leafUpPos;
    [SerializeField] private GameObject creditLeaf;
    [SerializeField] private Image logo;

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
        logo.DOFade(0f, moveDuration*2 );
        creditLeaf.transform.DOLocalRotate(leafUpPos, moveDuration);
    }

    private void OnCreditClosed()
    {
        logo.DOFade(1f, moveDuration*2 );
        creditLeaf.transform.DOLocalRotate(leafDownPos, moveDuration);
    }
}

