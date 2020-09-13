using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntroFader : MonoBehaviour {

    [SerializeField]private float transitionTime = 1f;
    [SerializeField]private LeanTweenType easing = LeanTweenType.easeInOutSine;

	void Start () {
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0, transitionTime).setOnComplete(()=>{
            Destroy(gameObject);
        });
	}
	
}
