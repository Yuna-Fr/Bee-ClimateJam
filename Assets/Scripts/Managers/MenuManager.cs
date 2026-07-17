using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private float fadeInDelay = 1f;
    [SerializeField] private CanvasGroup fadeBG;


    [Header("Credit Leaf")]
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private Vector3 leafUpPos;
    [SerializeField] private GameObject creditLeaf;
    [SerializeField] private Image logo;

    private Vector3 leafDownPos;
    private bool isCreditOpen = false;

    private void Awake()
    {
        fadeBG.alpha = 1f;
    }

    private void Start()
    {
        fadeBG.DOFade(0f, fadeInDelay);
        Cursor.lockState = CursorLockMode.None;
        leafDownPos = creditLeaf.transform.localRotation.eulerAngles;
    }

    #region Button Callbacks
    
    public void OnStartButtonPressed()
    {
        fadeBG.DOFade(1f, (fadeInDelay / 1.5f))
            .OnComplete(() => SceneManager.LoadScene(sceneName));
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

