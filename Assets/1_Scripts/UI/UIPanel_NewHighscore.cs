using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_NewHighscore : UIPanel {

    [Header("UIPanel_NewHighscore")]
    [SerializeField]Text scoreText ;
    [SerializeField]GameObject scoreBg;
    [SerializeField]GameObject mainBg;
    [SerializeField]float scaleTransitionDuration = .4f;
    [SerializeField]AnimationCurve scaleTransitionCurve;

    public void OpenWithScore(int score)
    {
        scoreText.text = score.ToString();
        Open();
        AudioManager2.Instance.Play(SoundsManager.Instance.hiscorePanel, AudioClipExtended.AudioType.SFX);
    }

    protected override LTDescr AdditionalOpeningTween()
    {
        mainBg.transform.localScale = Vector3.zero;

        LeanTween.scale(mainBg, Vector3.one, scaleTransitionDuration).setEase(scaleTransitionCurve);

        scoreBg.transform.localScale = Vector3.one * 4;

        CanvasGroup scoreBgCanvasGroup = scoreBg.GetComponent<CanvasGroup>();
        LeanTween.scale(scoreBg, Vector3.one, scaleTransitionDuration).setEase(LeanTweenType.easeInOutExpo);
        scoreBgCanvasGroup.alpha = 0;
        return LeanTween.alphaCanvas(scoreBgCanvasGroup, 1, scaleTransitionDuration).setEase(LeanTweenType.easeInOutExpo);

    }
}
