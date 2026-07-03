using UnityEngine;

public class Enemy : ObstacleBase
{
    protected override void OnCollisonReaction(BeeController bee)
    {
        base.OnCollisonReaction(bee);
        Debug.Log($"GameOver");
    }
}
