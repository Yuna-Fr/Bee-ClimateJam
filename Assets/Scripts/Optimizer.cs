using UnityEngine;

public class Optimizer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player entered the optimization zone.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Player exited the optimization zone.");
    }
}
