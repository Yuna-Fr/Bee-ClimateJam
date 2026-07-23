using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] public bool isPollinated = false;
    
    [Header("Babies")]
    [SerializeField] private List<Transform> flowerBabies;
    [SerializeField] private Vector2 babySpawnRateBetween = new(0.2f, 0.7f);

    [Header("Main Flower")]
    [SerializeField] private SpriteRenderer mainFlower;
    [SerializeField] private Sprite healthyFlower;
    [SerializeField] private AudioSource audioSource;

    private List<Vector3> babiesSizes = new();

    private void Start()
    {
        if (isPollinated)
        {
            mainFlower.sprite = healthyFlower;
            return;
        }
        
        foreach (Transform flowerBaby in flowerBabies)
        {
            flowerBaby.gameObject.SetActive(false);
            babiesSizes.Add(flowerBaby.localScale);
        }
    }

    public void Pollinate()
    {
        if (isPollinated) return;

        isPollinated = true;

        audioSource.clip = SoundManager.Instance.GetFlowerAliveSound();
        audioSource.Play();
        mainFlower.sprite = healthyFlower;
        
        StartCoroutine(PollinationAnimation());
    }

    private IEnumerator PollinationAnimation()
    {
         foreach (Transform flowerBaby in flowerBabies)
        {
            flowerBaby.localScale = Vector3.zero;
            flowerBaby.gameObject.SetActive(true);

            float spinAxis = Random.value > 0.5f ? 90f : -90f;
            flowerBaby.DOLocalRotate(new Vector3(0, 0, spinAxis), 1f, RotateMode.FastBeyond360).SetRelative().SetEase(Ease.OutQuad);
            flowerBaby.DOScale(babiesSizes[flowerBabies.IndexOf(flowerBaby)], 1f).SetEase(Ease.OutBack);

            var audioPop = flowerBaby.gameObject.AddComponent<AudioSource>();
            audioPop.clip = SoundManager.Instance.GetFlowerPopSound();
            audioPop.spatialBlend = 1f;
            audioPop.Play();

            var audioGrow = flowerBaby.gameObject.AddComponent<AudioSource>();
            audioGrow.clip = SoundManager.Instance.GetFlowerGrowSound();
            audioGrow.spatialBlend = 1f;
            audioGrow.Play();

            yield return new WaitForSeconds(Random.Range(babySpawnRateBetween.x, babySpawnRateBetween.y));
        }
    }
}
