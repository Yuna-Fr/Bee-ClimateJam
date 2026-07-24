using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BeeController : MonoBehaviour
{
    public static BeeController Instance { get; private set; }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotSpeed = 0.2f;
    [SerializeField] private GameObject beeBody;
    [SerializeField] private InputActionAsset inputActions;
    public Rigidbody2D rb;
    private InputAction moveAction;
    private Vector2 moveInput;


    [Header("Bounce")]
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private float bounceDuration = 0.2f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isBouncing = false;


    [Header("Ressources")]
    [SerializeField] private int energyDecreasePerSecond = 5;
    [SerializeField] private int energyGivenFromNectar = 25;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image energyBarSlow;
    [SerializeField] private TextMeshProUGUI score;
    private Tween energyBarFillTween = null;
    private int energy = 100;
    private int nectarStock = 0;
    private int pollinatedFlowersScore = 0;


    [Header("Sounds")]
    [SerializeField] private AudioClip barkToxic;
    [SerializeField] private AudioClip barkStung;
    [SerializeField] private AudioClip barkExausted;
    [SerializeField] private List<AudioClip> barkPushed;
    [SerializeField] private AudioClip buzzMax;
    [SerializeField] private AudioClip buzzMid;
    [SerializeField] private AudioClip buzzMin;
    [SerializeField] private AudioSource buzzSource;
    [SerializeField] private AudioSource barksSource;


    #region Base Unity Methods
    void Awake()
    {
        Instance = this;

        inputActions.FindActionMap("Player").Enable();
        moveAction = inputActions.FindAction("Move");

        energyBar.fillAmount = 100;
        energyBarSlow.fillAmount = 100;
        score.text = pollinatedFlowersScore.ToString();
    }

    void Start()
    {
        GameManager.Instance.OnGameEnd += OnGameEnd;
    }

    void FixedUpdate()
    {
        if (!isBouncing)
        {
            moveInput = moveAction.ReadValue<Vector2>();

            if (moveInput != Vector2.zero)
            {
                rb.AddForce(moveInput * moveSpeed, ForceMode2D.Force);

                float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
                beeBody.transform.DOLocalRotate(new Vector3(0, 0, angle - 90), rotSpeed);
            }
        }

        ClampMovements();

        if (Time.fixedTime % 1f < Time.fixedDeltaTime) // every 1 second, decrease energy
        {
            energy -= energyDecreasePerSecond;
            UpdateEnergyBar();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Nectar"))
        {
            Destroy(collision.gameObject);
            RecoltNectar();
        }
        else if (collision.gameObject.CompareTag("Flower"))
        {
            var flower = collision.gameObject.GetComponent<Flower>();
            if (flower && !flower.isPollinated && nectarStock > 0)
            {
                nectarStock--;
                flower.Pollinate();
                pollinatedFlowersScore++;
                
                score.text = pollinatedFlowersScore.ToString();
                var parentUI = score.transform.parent.transform;

                // Reset
                parentUI.DOKill(complete: true);
                parentUI.localRotation = Quaternion.identity;
                parentUI.localScale = Vector3.one;
                
                parentUI.DOPunchScale(Vector3.one * 0.1f, 0.5f, 5, 1);
                parentUI.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutBack);
            }
        }
    }

    void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
        GameManager.Instance.OnGameEnd -= OnGameEnd;
    }

    private void OnGameEnd()
    {
        barksSource.DOFade(0f, 1f);
        buzzSource.DOFade(0f,  1f)
            .OnComplete(() => enabled = false);
    }
    #endregion

    #region Feedbacks
    public void HitFeedback(bool byEnemy = false)
    {
        barksSource.clip = byEnemy ? barkStung : barkToxic;
        barksSource.Play();

        spriteRenderer.DOColor(Color.red, 0.15f)
            .OnComplete(() => spriteRenderer.DOColor(Color.white, 0.15f));
    }

    public void PushedFeedback()
    {
        if (Random.value < 0.2f) // 1 chance sur 3 
        {
            barksSource.clip = barkPushed[Random.Range(0, barkPushed.Count)];
            barksSource.Play();
        }
    }

    public void DiesAnim(float duration)
    {
        beeBody.transform.DOShakePosition(duration, 0.5f, 8, 90, false, true);
        beeBody.transform.DOScale(Vector3.zero, duration);

        barksSource.DOFade(0f, duration);
        buzzSource.DOFade(0f, duration)
            .OnComplete(() => this.enabled = false);
    }
    #endregion

    #region Movements & Physics
    private void ClampMovements() // Clamp the bee's position to screen bounds
    {
        if (transform.position.y >= Camera.main.ScreenToWorldPoint(new(0, Screen.height, 0)).y)
            transform.position = new Vector3(transform.position.x, Camera.main.ScreenToWorldPoint(new(0, Screen.height, 0)).y, transform.position.z);
        
        else if (transform.position.y <= Camera.main.ScreenToWorldPoint(new(0, 0, 0)).y)
            transform.position = new Vector3(transform.position.x, Camera.main.ScreenToWorldPoint(new(0, 0, 0)).y, transform.position.z);

        if (transform.position.x >= Camera.main.ScreenToWorldPoint(new(Screen.width, 0, 0)).x)
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new(Screen.width, 0, 0)).x, transform.position.y, transform.position.z);
        
        else if (transform.position.x <= Camera.main.ScreenToWorldPoint(new(0, 0, 0)).x)
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new(0, 0, 0)).x, transform.position.y, transform.position.z);

    }

    public void TakeBounce(Vector2 direction)
    {
        if (isBouncing) return;
        StartCoroutine(BounceCoroutine(direction));
    }

    private IEnumerator BounceCoroutine(Vector2 direction)
    {
        isBouncing = true;
        rb.AddForce(direction.normalized * bounceForce, ForceMode2D.Impulse);
        beeBody.transform.DOShakePosition(bounceDuration, 0.3f, 10, 90, false, true);

        yield return new WaitForSeconds(bounceDuration);

        isBouncing = false;
    }
    #endregion

    #region Energy Management

    private void UpdateEnergyBar()
    {
        float energyPercent = (float)energy / 100f;
        energyBar.fillAmount = energyPercent;
        energyBarFillTween = energyBarSlow.DOFillAmount(energyPercent, 1f).SetEase(Ease.Linear);

        // Set audio buzz based on current energy level
        AudioClip targetBuzzClip = energy > 66 ? buzzMax : energy < 33 ? buzzMin : buzzMid;
        if (buzzSource.clip != targetBuzzClip) 
        {
            buzzSource.clip = targetBuzzClip;
            buzzSource.Play();
        }
        
        if (energy <= 0)
        {
            HitFeedback();
            GameManager.Instance.RemoveAHeart();
            barksSource.clip = barkExausted;
            barksSource.Play();
        }
    }

    private void RecoltNectar()
    {
        SoundManager.Instance.PlayPollen();
        nectarStock++;

        energyBarFillTween?.Kill();
        energy = Mathf.Min(energy + energyGivenFromNectar, 100);

        UpdateEnergyBar();
    }
    #endregion
}
