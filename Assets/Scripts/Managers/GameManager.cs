using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float levelSpeed = 2f;
    void Update()
    {
        //Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+1f, Camera.main.transform.position.y, Camera.main.transform.position.z);
        Camera.main.transform.position += new Vector3(levelSpeed * Time.deltaTime, 0, 0);   
    }
}
