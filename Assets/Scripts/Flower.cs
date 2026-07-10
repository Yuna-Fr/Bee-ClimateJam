using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [HideInInspector] public bool isPollinated = false;
    
    [SerializeField] private List<Transform> flowerBabies;
    [SerializeField] private Vector2 babySpawnRate = new(0.2f, 0.7f);

    private List<Vector3> babiesSizes = new();

    private void Start()
    {
        foreach (Transform flowerBaby in flowerBabies)
        {
            flowerBaby.gameObject.SetActive(false);
            babiesSizes.Add(flowerBaby.localScale);
        }
    }

    public void Pollinate()
    {
        isPollinated = true;
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

            yield return new WaitForSeconds(Random.Range(babySpawnRate.x, babySpawnRate.y));
        }
    }
}
