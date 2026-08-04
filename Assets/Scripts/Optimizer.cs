using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Optimizer : MonoBehaviour
{
    private int temporaryLayer; // Caching the layer ID for perf instead of a string every frame

    private void Start()
    {
        temporaryLayer = LayerMask.NameToLayer("Temporary");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == temporaryLayer)
        {
            Light2D lightComponent = other.GetComponent<Light2D>();
            
            if (lightComponent != null)
                lightComponent.enabled = true; // Turn the light ON

            else
            {
                if (other.transform.childCount > 0)
                    other.transform.GetChild(0).gameObject.SetActive(true); // Activate particles   
            }     
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == temporaryLayer)
        {
            Light2D lightComponent = other.GetComponent<Light2D>();
            if (lightComponent != null)
                lightComponent.enabled = false; // Turn the light OFF
            
            else
                other.gameObject.SetActive(false); // Deactivate particles
        }        
    }
}