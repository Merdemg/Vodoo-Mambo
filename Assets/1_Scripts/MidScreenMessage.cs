using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MidScreenMessage : MonoBehaviour
{
    [SerializeField]Text text;
    [SerializeField]GameObject panel;
    CanvasGroup textCanvasGroup;

    [SerializeField]float inDuration = 0.2f;
    [SerializeField]float stayDuration = 0.2f;
    [SerializeField]float outDuration = 0.2f;
    [SerializeField]float timescale = 0.5f;

	void Awake() {
		textCanvasGroup = panel.GetComponent<CanvasGroup>();
		panel.gameObject.SetActive(false);
	}

    public void Show(string message, Action onComplete = null, bool slowtime = true) {
		StopAllCoroutines ();
		text.text = message;
        StartCoroutine(ShowRoutine(onComplete, slowtime));
    }

    IEnumerator ShowRoutine(Action onComplete, bool slowtime) {
        // Slow down time
        if(slowtime)
            Time.timeScale = timescale;
        // Initialize text
		panel.gameObject.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        textCanvasGroup.alpha = 1;

        // Scale in
		LeanTween.scale(panel.gameObject, Vector3.one,inDuration);

        yield return new WaitForSeconds(inDuration + stayDuration);

        // Scale up and fade out
		LeanTween.scale(panel.gameObject, Vector3.one * 4f, outDuration);
        LeanTween.alphaCanvas(textCanvasGroup, 0, outDuration);

        yield return new WaitForSeconds(outDuration);

		if(onComplete != null) {
			onComplete();
		}

        // Disable text
		panel.gameObject.SetActive(false);
        // Restore time scale
        if(slowtime)
            Time.timeScale = 1;
    }

}

