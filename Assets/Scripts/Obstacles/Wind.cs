using UnityEngine;

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
}
