using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class GdprManager : MonoBehaviour
{
    private const string DidAcceptGdprKey = "vm_DidAcceptGdpr";
    private const string PrivacyUrl = "https://sillywalkgames.com/privacy.html";
    
    [SerializeField] private CanvasGroup _Panel;
    [SerializeField] private Button _AcceptButton;
    [SerializeField] private Button _PrivacyButton;
    
    private bool DidAcceptGdpr
    {
        get
        {
            return PlayerPrefs.GetInt(DidAcceptGdprKey, 0) == 1;
        }

        set
        {
            PlayerPrefs.SetInt(DidAcceptGdprKey, value ? 1 : 0);
        }
    }

    private void Awake()
    {
        _Panel.alpha = 0;
    }

    private void Start()
    {
        _AcceptButton.onClick.AddListener(Accept);
        _PrivacyButton.onClick.AddListener(ShowPrivacy);
        
        if(!DidAcceptGdpr)
            ShowGdprPanel();
        else
            DestroyPanel();
    }

    private void ShowPrivacy()
    {
        Application.OpenURL(PrivacyUrl);
    }

    private void Accept()
    {
        DestroyPanel();

        DidAcceptGdpr = true;
    }

    private void DestroyPanel()
    {
        LeanTween.alphaCanvas(_Panel, 0, .2f).setOnComplete(() => { Destroy(_Panel.gameObject); });
    }

    private void ShowGdprPanel()
    {
        LeanTween.alphaCanvas(_Panel, 1, .2f);
    }
    
    #if UNITY_EDITOR
    [Button]
    private void ClearPrefs()
    {
        PlayerPrefs.DeleteKey(DidAcceptGdprKey);
    }
    #endif
}
