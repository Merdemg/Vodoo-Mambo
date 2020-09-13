using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIToggle : MonoBehaviour
{

	[Header("Parameters")]
	[SerializeField]GameObject mover;
	[SerializeField]CanvasGroup moverOn;
	[SerializeField]CanvasGroup moverOff;
	[SerializeField]Transform moverOffPos;
	[SerializeField]Transform moverOnPos;

	[Header("Info")]
	[SerializeField]bool _isOn;
	[SerializeField]Button _button;

	void Awake()
	{
		_button = GetComponentInChildren<Button> ();
		_button.onClick.AddListener (OnClick);
		moverOffPos.gameObject.SetActive (false);
		moverOnPos.gameObject.SetActive (false);
	}


	public void Set (bool isOn, bool isAnimated = true)
	{
		_isOn = isOn;

		// Move the mover
		Vector3 fromPos = isOn ? moverOffPos.position : moverOnPos.position;
		Vector3 toPos = isOn ? moverOnPos.position : moverOffPos.position;
		CanvasGroup toFadeOut = isOn ? moverOff : moverOn;
		CanvasGroup toFadeIn = isOn ? moverOn : moverOff;

		if(isAnimated)
		{
			mover.transform.position = fromPos;
			toFadeIn.alpha = 0;
			toFadeOut.alpha = 1;

			LeanTween.move (mover, toPos, GPM.Instance.toggleDuration).setEase(GPM.Instance.toggleEasing);
			LeanTween.alphaCanvas (toFadeIn, 1, GPM.Instance.toggleDuration);
			LeanTween.alphaCanvas (toFadeOut, 0, GPM.Instance.toggleDuration);
		}
		else
		{
			toFadeIn.alpha = 1;
			toFadeOut.alpha = 0;
			mover.transform.position = toPos;
		}
	}

	public int GetValueInt()
	{
		return _isOn ? 1 : 0;
	}

	public bool GetValue()
	{
		return _isOn;
	}

	void OnClick()
	{
		_isOn = !_isOn;

		Set (_isOn);

		UIManager.Instance.optionsPanel.ApplyOptions ();
	}

}

