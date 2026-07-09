using UnityEngine;

/// <summary>Frelon patrouille vertical (Z=280 m).</summary>
public class HornetPatrol : MonoBehaviour
{
    [SerializeField] private float yMin = 3f;
    [SerializeField] private float yMax = 7f;
    [SerializeField] private float speed = 1.5f;

    private int dir = 1;

    private void Update()
    {
        var p = transform.position;
        p.y += dir * speed * Time.deltaTime;
        if (p.y >= yMax) dir = -1;
        else if (p.y <= yMin) dir = 1;
        transform.position = p;
    }
}