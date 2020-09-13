using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBlinker : MonoBehaviour {


	[SerializeField]float[] randomWaitDurationMinMax = new float[] { 1, 5 };
	[SerializeField]float[] randomStayDurationMinMax = new float[] { .1f, 1f };
	[SerializeField]float[] randomShowHideDurationMinMax = new float[]{ .1f, 1f };
	[SerializeField]LeanTweenType showHideEasing = LeanTweenType.easeInOutSine;
	[SerializeField]CanvasGroup canvasGroup;

	void Start () {
		
		canvasGroup = GetComponent<CanvasGroup> ();

		StartCoroutine (Blink ());
	}
	
	IEnumerator Blink()
	{
		
		while(true)
		{
			float rngWait = Random.Range (randomWaitDurationMinMax [0], randomWaitDurationMinMax [1]);
			float rngStay = Random.Range (randomStayDurationMinMax [0], randomStayDurationMinMax [1]);
			float rngShowHide = Random.Range (randomShowHideDurationMinMax [0], randomShowHideDurationMinMax [1]);

			yield return new WaitForSeconds (rngWait);

			float animationTime = (rngShowHide * 1.66f) + rngStay;
			LeanTween.alphaCanvas (canvasGroup, 1, rngShowHide).setEase(showHideEasing);
			LeanTween.alphaCanvas (canvasGroup, 0, rngShowHide * .66f).setEase (showHideEasing).setDelay(rngStay + rngShowHide);

			// Wait for animation
			yield return new WaitForSeconds (animationTime);

		}

	}
}
