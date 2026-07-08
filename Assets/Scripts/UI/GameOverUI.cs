using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Animator button1;
    [SerializeField] private Animator button2;
    [SerializeField] private GameObject flower;
    
    private void Start()
    {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        flower.SetActive(true);
    }

    public void LaunchGameOverUI()
    {
        Animator flowerAnimator = GetComponent<Animator>();

        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);

        //flowerAnimator.speed = 0.3f;
        button1.speed = 0.4f;
        button2.speed = 0.2f;
        
        flowerAnimator.Play("GameOver_Flower");
        button1.Play("GameOver_Petals");
        button2.Play("GameOver_Petals_2");
    }
}
