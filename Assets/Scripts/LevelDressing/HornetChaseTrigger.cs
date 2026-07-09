using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HornetChaseTrigger : MonoBehaviour
{
    [SerializeField] private GameObject hornetPrefab;
    [SerializeField] private Vector3 spawnOffset = new(-4f, 0f, 0f);

    private bool triggered;

    public void Configure(GameObject hornet) => hornetPrefab = hornet;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered || collision.GetComponent<BeeController>() == null)
            return;
        triggered = true;
        if (hornetPrefab != null)
            Instantiate(hornetPrefab, transform.position + spawnOffset, Quaternion.identity);
    }
}