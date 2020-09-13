using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TipsScreen : MonoBehaviour
{
    [SerializeField] float fadeDuration = .2f;
    [SerializeField] float[] minMaxDuration;
    [SerializeField] string[] tips;
    [SerializeField] int tipCount = 4;
    [SerializeField] Text tipText;
    
    string RandomTip {
        get {

        	int tipIdx = ( 1 + UnityEngine.Random.Range( 0, tipCount ) );
        	string tipKey = ( "Tips--Random-Tip-" + tipIdx );
        	string tipText = Lean.Localization.LeanLocalization.GetTranslationText( tipKey );

        	return tipText;
        	
            // return tips[UnityEngine.Random.Range(0, tips.Length)];
        }
    }

    float RandomDuration {
        get {
            return UnityEngine.Random.Range(minMaxDuration[0], minMaxDuration[1]);
        }
    }

    public void Show(Action callback) {
		gameObject.SetActive(true);
        var canvasGroup = GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(canvasGroup, 1, fadeDuration);
        tipText.text = RandomTip;

        Utility.Instance.DoAfter(RandomDuration, () =>
        {
            // Hide after duration
            LeanTween.alphaCanvas(canvasGroup, 1, fadeDuration).setOnComplete((obj) => {
				gameObject.SetActive(false);
            });
            callback();
        });
    }
}
