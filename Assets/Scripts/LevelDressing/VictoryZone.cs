using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class VictoryZone : MonoBehaviour
{
    private void Awake()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BeeController>() != null)
            GameManager.Instance.Victory();
    }
}