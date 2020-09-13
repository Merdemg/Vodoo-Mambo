using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Countdown : MonoBehaviour {

	public Text countdownText;
	CanvasGroup canvasGroup;

	public float interval = .7f;
	public float initialWaitTime = .4f;

	public float fadeInTime = .4f;
	public LeanTweenType ease = LeanTweenType.easeInOutQuad;

	void Start()
	{
		canvasGroup = GetComponent<CanvasGroup> ();
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
//		StartCoroutine (StartCountdownRoutine (5, ()=>{
//			Trace.Msg("countdown ended");
//		}));
	}


	public void StartCountown(Action callback)
	{
		StartCoroutine (StartCountdownRoutine (callback));
	}

	IEnumerator StartCountdownRoutine(Action callback)
	{
        // FIXME: DEBUG
//        if (canvasGroup != null)
            canvasGroup.alpha = 1;

        countdownText.text = Lean.Localization.LeanLocalization.GetTranslationText( "Common--Ready-Excl" );
        AudioManager2.Instance.Play(clip:SoundsManager.Instance.readyStart, type:AudioClipExtended.AudioType.SFX, isFade: false);
        FadeInText (()=>{
            
        });

        yield return new WaitForSeconds (interval);


        countdownText.text = Lean.Localization.LeanLocalization.GetTranslationText( "Common--Go-Excl" );

		FadeInText (()=>{
//			canvasGroup.alpha = 0;
			FadeOutText(.2f);

			callback ();
		});

//		for (int i = seconds; i >= 0 + 1; i--) 
//		{
//			countdownText.text = i.ToString();
//
//			canvasGroup.alpha = 0;
//
//			FadeInText ();
//
//			yield return new WaitForSeconds (interval);
//		}

	}

	void FadeInText(Action callback)
	{
		gameObject.transform.localScale = Vector3.one * .3f;

		LeanTween.scale (gameObject, Vector3.one, fadeInTime).setEase (ease).setOnComplete(()=>{
			
			if(callback != null)
				callback();
		});
	}

	void FadeOutText(float seconds)
	{
		LeanTween.alphaCanvas (canvasGroup, 0, seconds).setEase (ease);
	}

}
