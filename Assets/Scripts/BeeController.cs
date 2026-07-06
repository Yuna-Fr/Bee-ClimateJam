using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class BeeController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotSpeed = 0.2f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject beeBody;
    [SerializeField] private InputActionAsset inputActions;
    private InputAction moveAction;
    private Vector2 moveInput;


    [Header("Bounce")]
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private float bounceDuration = 0.2f;
    private bool isBouncing = false;


    [Header("Ressources")]
    [SerializeField] private int energyDecreasePerSecond = 5;
    [SerializeField] private int energyGivenFromNectar = 25;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image energyBarSlow;
    private Tween energyBarFillTween = null;
    private int energy = 100;
    private int nectarStock = 0;


    void Awake()
    {
        inputActions.FindActionMap("Player").Enable();
        moveAction = inputActions.FindAction("Move");

        energyBar.fillAmount = 100;
        energyBarSlow.fillAmount = 100;
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
            energyBar.fillAmount = (float)energy / 100;
            energyBarFillTween = energyBarSlow.DOFillAmount((float)energy / 100, 1f).SetEase(Ease.Linear);

            if (energy <= 0)
                GameManager.Instance.GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Nectar")
        {
            Destroy(collision.gameObject);
            nectarStock++;

            energyBarFillTween?.Kill();
            energy = Mathf.Min(energy + energyGivenFromNectar, 100);

            float energyPercent = (float)energy / 100f;
            energyBar.fillAmount = energyPercent;
            energyBarSlow.fillAmount = energyPercent;
        }
    }

    void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

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
}
