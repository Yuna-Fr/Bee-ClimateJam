using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VicrotyUI : MonoBehaviour
{
    [SerializeField] private float textFadeDelay = 1f;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private CanvasGroup infoButton;

    private void Start()
    {
        infoButton.alpha = 0f;
        textUI.alpha = 0f;
    }

    public void LaunchUI()
    {   
        StartCoroutine(TextCoroutine());
    }

    private IEnumerator TextCoroutine()
    {
        textUI.DOFade(1f, textFadeDelay);
        yield return new WaitForSeconds(textFadeDelay * 2); //Let text sit so player can read it

        infoButton.DOFade(1f, textFadeDelay);
    }

    #region Buttons Callback
    public void OnMenu()
    {
        GameManager.Instance.LoadMenu();
    }
    
    public void OnLink()
    {
        Application.OpenURL("https://en.wikipedia.org/wiki/Guerrilla_gardening");
    }
    #endregion
}
