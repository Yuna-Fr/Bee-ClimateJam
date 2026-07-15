using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Wind : MonoBehaviour
{
    public enum WindDirection2D { Up, Down, Left, Right }
    
    [SerializeField] private WindDirection2D chooseDirection = WindDirection2D.Right;
    [SerializeField] private float windStrength = 10f;

    private void OnTriggerStay2D(Collider2D collision) 
    {
        if (collision.gameObject.TryGetComponent<BeeController>(out var bee))
            bee.rb.AddForce(GetWindDirection() * windStrength, ForceMode2D.Force);
    }

    private Vector2 GetWindDirection()
    {
        var direction = chooseDirection switch
        {
            WindDirection2D.Up => transform.up,
            WindDirection2D.Down => -transform.up,
            WindDirection2D.Left => -transform.right,
            WindDirection2D.Right => transform.right,
            _ => transform.up
        };

        return direction;
    }

    private void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null || !col.enabled) return;

        DrawColliderZone(col);
        DrawWindArrow(col);
    }

    private void DrawColliderZone(Collider2D col)
    {
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(0f, 0.8f, 1f, 0.4f);

        if (col is BoxCollider2D box)
        {
            Gizmos.DrawWireCube(box.offset, box.size);
            Gizmos.color = new Color(0f, 0.8f, 1f, 0.1f); // Remplissage léger
            Gizmos.DrawCube(box.offset, box.size);
        }

        Gizmos.matrix = originalMatrix;
    }

    private void DrawWindArrow(Collider2D col)
    {
        Vector3 center = col.bounds.center;
        Vector3 direction = (Vector3)GetWindDirection();
        Gizmos.color = Color.skyBlue;

        float arrowLength = Mathf.Clamp(windStrength * 0.1f, 1f, 3f);
        Vector3 arrowVector = direction.normalized * arrowLength;

        Gizmos.DrawRay(center, arrowVector);

        Vector3 arrowEnd = center + arrowVector;
        float wingLength = arrowLength * 0.25f;
        float wingAngle = 25f;

        Vector3 rightWing = Quaternion.Euler(0, 0, wingAngle) * -direction.normalized * wingLength;
        Vector3 leftWing = Quaternion.Euler(0, 0, -wingAngle) * -direction.normalized * wingLength;

        Gizmos.DrawRay(arrowEnd, rightWing);
        Gizmos.DrawRay(arrowEnd, leftWing);
    }
}