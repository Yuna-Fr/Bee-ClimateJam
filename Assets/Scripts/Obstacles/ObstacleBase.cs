using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ObstacleBase : MonoBehaviour //If col is trigger is perticide otherwise is a physical obstacle
{
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.TryGetComponent<BeeController>(out var bee))
        {
            GameManager.Instance.GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<BeeController>(out var bee))
        {
            OnCollisonReaction(bee);
        }
    }

    protected virtual void OnCollisonReaction(BeeController bee)
    {
        Vector2 bounceDirection = (bee.transform.position - transform.position).normalized;
        bee.TakeBounce(bounceDirection);
    }
}
