using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Video;

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

    [Header("Video")]
    [SerializeField] private string videoFileName = "Video.mp4";
    [SerializeField] private VideoPlayer vp;
    [SerializeField] private CanvasGroup video;
    
    private Vector3 leafDownPos;
    private bool isCreditOpen = false;

    private void Awake()
    {
        fadeBG.alpha = 1f;
        video.alpha = 0;
        video.gameObject.SetActive(false);
    }

    private void Start()
    {
        vp.loopPointReached += OnVideoFinished;

        fadeBG.DOFade(0f, fadeInDelay);
        Cursor.lockState = CursorLockMode.None;
        leafDownPos = creditLeaf.transform.localRotation.eulerAngles;
    }

    private void OnDestroy()
    {
        vp.loopPointReached -= OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        OnSkipButtonPressed();
    }

    #region Button Callbacks

    public void OnStartButtonPressed()
    {
        video.gameObject.SetActive(true);
        video.DOFade(1f, 0.2f);
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        vp.url = videoPath;
        vp.Play();
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

    public void OnSkipButtonPressed()
    {
        fadeBG.DOFade(1f, (fadeInDelay / 1.5f))
            .OnComplete(() => SceneManager.LoadScene(sceneName));
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
