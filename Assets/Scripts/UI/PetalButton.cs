using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PetalButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{   
    enum ButtonPos { Up, Middle, Down };

    [SerializeField] private ButtonPos pos = ButtonPos.Middle;
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private float clickDuration = 0.3f;
    [SerializeField] private float moveForce = 20f;
    [SerializeField] private float secondForce = 10f;

    private Vector3 originalPos;
    private Vector3 originalRot;
    private Tween petalTween;

    void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation.eulerAngles;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        petalTween?.Kill();

        if (pos == ButtonPos.Up)
            petalTween = transform.DOLocalMove((originalPos + Vector3.right * moveForce) + (Vector3.up * secondForce), moveDuration);
        else if (pos == ButtonPos.Middle)
            petalTween = transform.DOLocalMove(originalPos + Vector3.right * moveForce, moveDuration);
        else if (pos == ButtonPos.Down)
            petalTween = transform.DOLocalMove((originalPos + Vector3.right * moveForce) + (Vector3.down * secondForce), moveDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        petalTween?.Kill();
        petalTween = transform.DOLocalMove(originalPos, moveDuration);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       transform.DOLocalRotate(transform.localRotation.eulerAngles + new Vector3(0, -20, 0), moveDuration )
            .onComplete += () => transform.DOLocalRotate(originalRot, clickDuration);
    }
}