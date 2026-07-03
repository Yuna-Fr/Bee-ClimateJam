using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BeeController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotSpeed = 0.2f;
    [SerializeField] private float rotAngle = 40f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject beeBody;
    [SerializeField] private InputActionAsset inputActions;

    [Header("Bounce")]
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private float bounceDuration = 0.2f;

    private bool isBouncing = false;
    private Vector2 moveInput;
    private InputAction moveAction;

    void Awake()
    {
        inputActions.FindActionMap("Player").Enable();
        moveAction = inputActions.FindAction("Move");
    }

    void FixedUpdate()
    {
        if (!isBouncing)
        {
            moveInput = moveAction.ReadValue<Vector2>();

            if (moveInput != Vector2.zero)
            {
                rb.AddForce(moveInput * moveSpeed, ForceMode2D.Force);

                // Rotate the bee based on the vertical input
                // float targetZRotation = moveInput.y * rotAngle;
                // beeBody.transform.DOLocalRotate(new Vector3(0, 0, 90 + targetZRotation), rotSpeed);
            }
        }

        ClampMovements();
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
        moveInput = Vector2.zero;
        //rb.constraints = RigidbodyConstraints2D.None;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * bounceForce, ForceMode2D.Impulse);
        beeBody.transform.DOShakePosition(bounceDuration, 0.3f, 10, 90, false, true);

        yield return new WaitForSeconds(bounceDuration);

        isBouncing = false;
        
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return beeBody.transform.DOLocalRotate(new Vector3(0, 0, 90), rotSpeed / 3).WaitForCompletion();
        
        //rb.constraints = RigidbodyConstraints2D.None;

    }

    #endregion
}
