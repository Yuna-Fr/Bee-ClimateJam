using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ObstacleBase : MonoBehaviour //If col is trigger is perticide otherwise is a physical obstacle
{
    [SerializeField] private bool isTrunk = false;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.TryGetComponent<BeeController>(out var bee))
            OnTriggerReaction(bee);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<BeeController>(out var bee))
            OnCollisonReaction(bee);
    }

    protected virtual void OnTriggerReaction(BeeController bee)
    {
        bee.HitFeedback();
        GameManager.Instance.RemoveAHeart();
    }

    protected virtual void OnCollisonReaction(BeeController bee)
    {
        Vector2 bounceDirection = (bee.transform.position - transform.position).normalized;
        bee.TakeBounce(bounceDirection);
        bee.PushedFeedback();

        SoundManager.Instance.PlayCollision(isTrunk);
    }
}
