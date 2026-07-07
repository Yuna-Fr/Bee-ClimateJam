using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public bool isPollinated = false;

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
        if (isPollinated) return;

        isPollinated = true;
        StartCoroutine(PollinationAnimation());
    }

    private IEnumerator PollinationAnimation()
    {
         foreach (Transform flowerBaby in flowerBabies)
        {
            flowerBaby.localScale = Vector3.zero;
            flowerBaby.gameObject.SetActive(true);
            flowerBaby.DOScale(babiesSizes[flowerBabies.IndexOf(flowerBaby)], 1f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(Random.Range(babySpawnRate.x, babySpawnRate.y));
        }
    }
}
