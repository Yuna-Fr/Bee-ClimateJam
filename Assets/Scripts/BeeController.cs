using DG.Tweening;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeeController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotSpeed = 0.2f;
    [SerializeField] private float rotAngle = 40f;
    [SerializeField] private GameObject beeBody;
    [SerializeField] private InputActionAsset inputActions;

    private Vector2 moveInput;
    private InputAction moveAction;

    void Awake()
    {
        inputActions.FindActionMap("Player").Enable();
        moveAction = inputActions.FindAction("Move");
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        //Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0f);
        Vector3 moveDirection = new Vector3(0f, moveInput.y, 0f);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Clamp the bee's pos to screen bounds
        if (transform.position.y >= Camera.main.ScreenToWorldPoint(new(0, Screen.height, 0)).y)
            transform.position = new Vector3(transform.position.x, Camera.main.ScreenToWorldPoint(new(0, Screen.height, 0)).y, transform.position.z);
        
        else if (transform.position.y <= Camera.main.ScreenToWorldPoint(new(0, 0, 0)).y)
            transform.position = new Vector3(transform.position.x, Camera.main.ScreenToWorldPoint(new(0, 0, 0)).y, transform.position.z);
    
        // Rotate the bee based on the vertical input
        float targetZRotation = moveInput.y * rotAngle;
        beeBody.transform.DOLocalRotate(new Vector3(0, 0, 90 + targetZRotation), rotSpeed);
    }

    void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }
}
