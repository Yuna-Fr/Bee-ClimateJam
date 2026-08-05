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
    [SerializeField] private GameObject beeShadow;
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
    [SerializeField] private int damageOnEnergy = 50;
    [SerializeField] private int energyDecreasePerSecond = 5;
    [SerializeField] private int energyGivenFromNectar = 25;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image energyBarSlow;
    [SerializeField] private Image energyBorder;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private RectTransform heart;
    [SerializeField] private Color DamageColor;
    [SerializeField] private Color DamageBorderColor;
    private Tween energyBarFillTween = null;
    private int energy = 100;
    private int nectarStock = 0;
    private int pollinatedFlowersScore = 0;
    private Color barColor;
    private Color barSlowColor;
    private Color barBorderColor;
    private Tween heartShakeTween;
    private Vector3 heartDefaultPos;


    [Header("Damage Cooldown")]
    [SerializeField] private float damageCooldown = 1.5f;
    [SerializeField] private bool flashSpriteOnHurt = true;
    private bool isInvincible = false;
    private Coroutine invincibilityCoroutine;


    [Header("Sounds")]
    [SerializeField] private AudioClip barkToxic;
    [SerializeField] private AudioClip barkStung;
    [SerializeField] private AudioClip barkExausted;
    [SerializeField] private List<AudioClip> barkPushed;
    [SerializeField] private AudioClip buzzMax;
    [SerializeField] private AudioClip buzzMid;
    [SerializeField] private AudioClip buzzMin;
    [SerializeField] private AudioSource buzzSource;
    [SerializeField] private AudioSource buzzSource2;
    [SerializeField] private AudioSource barksSource;

    [Header("Buzz Crossfade Settings")]
    [SerializeField] private float buzzCrossfadeDuration = 0.25f;
    private AudioSource activeBuzzSource;
    private AudioSource inactiveBuzzSource;
    private AudioClip currentBuzzClip;
    private Coroutine buzzLoopCoroutine;


    #region Base Unity Methods
    void Awake()
    {
        Instance = this;

        inputActions.FindActionMap("Player").Enable();
        moveAction = inputActions.FindAction("Move");

        energyBar.fillAmount = 100;
        energyBarSlow.fillAmount = 100;
        barColor = energyBar.color;
        barSlowColor = energyBarSlow.color;
        barBorderColor = energyBorder.color;
        score.text = pollinatedFlowersScore.ToString();

        activeBuzzSource = buzzSource;
        inactiveBuzzSource = buzzSource2;
        activeBuzzSource.loop = false;
        inactiveBuzzSource.loop = false;
    }

    void Start()
    {
        heartDefaultPos = heart.localPosition;
        GameManager.Instance.OnGameEnd += OnGameEnd;

        // Start buzz sound and loop routine
        AudioClip initialBuzz = energy > 66 ? buzzMax : (energy < 33 ? buzzMin : buzzMid);
        CrossfadeBuzzTo(initialBuzz);
        buzzLoopCoroutine = StartCoroutine(BuzzLoopRoutine());
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
                beeShadow.transform.DOLocalRotate(new Vector3(0, 0, angle), rotSpeed);
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
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameEnd -= OnGameEnd;

        if (buzzLoopCoroutine != null)
            StopCoroutine(buzzLoopCoroutine);

        if (invincibilityCoroutine != null)
            StopCoroutine(invincibilityCoroutine);

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        activeBuzzSource?.DOKill();
        inactiveBuzzSource?.DOKill();
    }

    private void OnGameEnd()
    {
        if (buzzLoopCoroutine != null) StopCoroutine(buzzLoopCoroutine);
        if (invincibilityCoroutine != null) StopCoroutine(invincibilityCoroutine);

        barksSource.DOFade(0f, 1f);
        buzzSource2.DOFade(0f, 1f);
        buzzSource.DOFade(0f, 1f)
            .OnComplete(() => enabled = false);
    }
    #endregion

    #region Feedbacks
    public void HitFeedback(bool byEnemy = false)
    {
        //Change colors on energy bar
        energyBar.DOKill();
        energyBarSlow.DOKill();
        energyBorder.DOKill();
        energyBar.DOColor(DamageColor, 0.15f).OnComplete(() => energyBar.DOColor(barColor, 0.6f));
        energyBarSlow.DOColor(DamageColor, 0.15f).OnComplete(() => energyBarSlow.DOColor(barSlowColor, 0.6f));
        energyBorder.DOColor(DamageBorderColor, 0.15f).OnComplete(() => energyBorder.DOColor(barBorderColor, 0.6f));

        barksSource.clip = byEnemy ? barkStung : barkToxic;
        barksSource.Play();

        spriteRenderer.DOColor(Color.red, 0.15f)
            .OnComplete(() => spriteRenderer.DOColor(Color.white, 0.15f));
    }

    public void PushedFeedback()
    {
        if (Random.value < 0.2f) // 1 chance sur 5
        {
            barksSource.clip = barkPushed[Random.Range(0, barkPushed.Count)];
            barksSource.Play();
        }
    }

    public void DiesAnim(float duration)
    {
        if (buzzLoopCoroutine != null) StopCoroutine(buzzLoopCoroutine);
        if (invincibilityCoroutine != null) StopCoroutine(invincibilityCoroutine);

        beeBody.transform.DOShakePosition(duration, 0.5f, 8, 90, false, true);
        beeBody.transform.DOScale(Vector3.zero, duration);

        barksSource.DOFade(0f, duration);
        buzzSource2.DOFade(0f, duration);
        buzzSource.DOFade(0f, duration)
            .OnComplete(() => this.enabled = false);
    }

    private void UpdateHeartShake()
    {
        if (energy >= 50) // Stop shaking above 50%
        {
            heartShakeTween?.Kill();
            heart.localPosition = heartDefaultPos;
            return;
        }

        float t = 1f - (energy / 50f); // t = 0 at 50 energy and t = 1 at 0 energy
        float strength = Mathf.Lerp(1f, 8f, t);
        float duration = Mathf.Lerp(0.15f, 0.04f, t);

        heartShakeTween?.Kill();
        heart.localPosition = heartDefaultPos;
        heartShakeTween = heart.DOShakeAnchorPos(duration, strength, 20, 90f, false, false).SetLoops(-1);
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

    #region Energy Management & Damage Cooldown

    public void DamangeOnEnergy()
    {
        if (isInvincible) return;

        energy = Mathf.Max(energy - damageOnEnergy, 0);
        
        UpdateEnergyBar(0.1f);
        StartInvincibility();
    }

    private void StartInvincibility()
    {
        if (invincibilityCoroutine != null)
            StopCoroutine(invincibilityCoroutine);

        invincibilityCoroutine = StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        if (flashSpriteOnHurt && spriteRenderer != null)
        {
            float elapsed = 0f;
            float flashInterval = 0.1f;

            while (elapsed < damageCooldown)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                yield return new WaitForSeconds(flashInterval);
                elapsed += flashInterval;
            }

            spriteRenderer.enabled = true;
        }
        else
            yield return new WaitForSeconds(damageCooldown);

        isInvincible = false;
    }

    private void UpdateEnergyBar(float fillDelay = 1f)
    {
        float energyPercent = (float)energy / 100f;
        energyBar.fillAmount = energyPercent;
        energyBarFillTween.Kill();
        energyBarFillTween = energyBarSlow.DOFillAmount(energyPercent, fillDelay).SetEase(Ease.Linear);

        // Set audio buzz based on current energy level with crossfade
        AudioClip targetBuzzClip = energy > 66 ? buzzMax : (energy < 33 ? buzzMin : buzzMid);
        if (currentBuzzClip != targetBuzzClip) 
            CrossfadeBuzzTo(targetBuzzClip);

        UpdateHeartShake();
        
        if (energy <= 0)
        {
            HitFeedback();

            energyBarFillTween = energyBarSlow.DOFillAmount(0, fillDelay).SetEase(Ease.Linear);
            
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

    #region Audio Management
    private IEnumerator BuzzLoopRoutine()
    {
        while (enabled)
        {
            if (activeBuzzSource != null && currentBuzzClip != null && activeBuzzSource.isPlaying)
            {
                float remainingTime = currentBuzzClip.length - activeBuzzSource.time;
                if (remainingTime <= buzzCrossfadeDuration)
                    CrossfadeBuzzTo(currentBuzzClip);
            }
            yield return null;
        }
    }

    private void CrossfadeBuzzTo(AudioClip newClip)
    {
        if (newClip == null) return;

        bool isInitialPlay = !activeBuzzSource.isPlaying && !inactiveBuzzSource.isPlaying;
        currentBuzzClip = newClip;

        if (isInitialPlay)
        {
            activeBuzzSource.clip = newClip;
            activeBuzzSource.volume = 1f;
            activeBuzzSource.Play();
            return;
        }

        // Swap active and inactive sources
        AudioSource temp = activeBuzzSource;
        activeBuzzSource = inactiveBuzzSource;
        inactiveBuzzSource = temp;

        // Setup new active source
        activeBuzzSource.clip = newClip;
        activeBuzzSource.time = 0f;
        activeBuzzSource.volume = 0f;
        activeBuzzSource.Play();

        activeBuzzSource.DOKill();
        inactiveBuzzSource.DOKill();
        activeBuzzSource.DOFade(1f, buzzCrossfadeDuration);
        inactiveBuzzSource.DOFade(0f, buzzCrossfadeDuration).OnComplete(() => inactiveBuzzSource.Stop());
    }
    #endregion
}