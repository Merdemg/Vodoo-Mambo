using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UICreditsButton : MonoBehaviour {

    [SerializeField]GameObject name;
    [SerializeField]GameObject bubble;
    [SerializeField] LeanTweenType easing = LeanTweenType.easeInOutSine;

    bool showingBubble = true;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Tapped);
        SetToInitialState();
    }

    public void SetToInitialState()
    {
        showingBubble = true;
        bubble.transform.localScale = Vector3.zero;
        name.transform.localScale = Vector3.zero;
        LeanTween.scale(bubble, Vector3.one, .4f).setEase(easing);
    }

    void Tapped() {
        
        GameObject toHide;
        GameObject toShow;
        if(showingBubble) {
            toHide = bubble;
            toShow = name;
            showingBubble = false;
        }
        else {
            toHide = name;
            toShow = bubble;
            showingBubble = true;
        }

        LeanTween.scale(toHide, Vector3.zero, .4f).setEase(easing);
        LeanTween.scale(toShow, Vector3.one, .4f).setDelay(.2f).setEase(easing);
    }
}
