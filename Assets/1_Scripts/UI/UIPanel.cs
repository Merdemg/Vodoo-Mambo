using UnityEngine;
using System.Collections;

public class UIPanel : MonoBehaviour
{
	[Header("Parameters")]
	/// <summary>
	/// Check this at UIManager to close other UIPanels when opening this
	/// </summary>
	[SerializeField] int level = 0;

	public int Level
	{
		get
		{
			return level;
		}
	}

	[SerializeField] bool isInitial = false;
    public bool isPopup = false;
	[SerializeField] bool playsOpenSound = true;
	[SerializeField] protected GameObject panel;
	[Space]
	[SerializeField] protected string anaylticsScreenTitle = "";

	[Header("Info")]
	[SerializeField]protected CanvasGroup _canvasGroup;
	[SerializeField]protected LTDescr _currentTween;
	[SerializeField]protected LTDescr _additionalTween;
    [SerializeField]protected bool isOpen = false;
	public bool IsOpen
	{
		get
		{
//          return _canvasGroup.alpha > 0 && gameObject.activeSelf;
            return isOpen;
		}
	}

	virtual protected void Awake()
	{
        gameObject.SetActive(true);
        // Popups are always lonely :(
//      if(isPopup)
//      {
//          isLonely = true;
//      }
        _canvasGroup = GetComponent<CanvasGroup>();

        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

	}

	virtual protected void Start()
	{
		if (!isInitial)
        {
//            Utility.Instance.DoAfter(.1f, () =>
//                {
                    gameObject.SetActive(false);
//                });
        }
	}

	public virtual void Open()
	{
		Trace.Msg("UI Panel Opening - " + name);
        isOpen = true;
		if (anaylticsScreenTitle != "")
		{
			GoogleAnalyticsManager.Instance.LogScreen(anaylticsScreenTitle);
		}

		UIManager.Instance.OnPanelOpening(this);
		gameObject.SetActive(true);

		CancelCurrentTweens();

        _canvasGroup.alpha = 0;
        _currentTween = LeanTween.alphaCanvas(_canvasGroup, 1, GPM.Instance.panelTransitionTime).setEase(GPM.Instance.panelTransitionType).setOnComplete(() =>
			{
				// TODO: Might need some clean up
			});

		_additionalTween = AdditionalOpeningTween();
//		LeasnTween.scale(gameObject, finalSize, GPM.Instance.ballShowDuration).setEase(GPM.Instance.ballShowEasing).setOnComplete(()=> {
//
//
//		});

//		gameObject.SetActive (true);
	}


	public virtual void Close()
	{
		Trace.Msg("UI Panel Closing - " + name);

//		CancelCurrentTweens();

		UIManager.Instance.OnPanelClosing(this);

		_currentTween = LeanTween.alphaCanvas(_canvasGroup, 0, GPM.Instance.panelTransitionTime).setEase(GPM.Instance.panelTransitionType).setOnComplete(() =>
			{
				gameObject.SetActive(false);
                isOpen = false;
			});

		_additionalTween = AdditionalClosingTween();
	}

	protected virtual LTDescr AdditionalClosingTween()
	{
		return null;
	}

	protected virtual LTDescr AdditionalOpeningTween()
	{
		return null;
	}

	protected void CancelCurrentTweens()
	{
        Trace.Msg(("Cancelling current Tweens!!").Colored(Colors.red));
		// Cancel current tweens
//		if (_currentTween != null)
//            LeanTween.cancel(_currentTween.uniqueId);
//
//
//		if (_additionalTween != null)
//            LeanTween.cancel(_additionalTween.uniqueId);
	}

	protected LTDescr ScaleTweenOpening()
	{
		panel.gameObject.transform.localScale = Vector3.zero;
		return LeanTween.scale(panel, Vector3.one, GPM.Instance.panelTransitionTime).setEase(GPM.Instance.panelTransitionType);
		//		return base.AdditionalOpeningTween ();
	}

	protected LTDescr ScaleTweenClosing()
	{
		//		panel.gameObject.transform.localScale = Vector3.zero;
		return LeanTween.scale(panel, Vector3.one * 1.2f, GPM.Instance.panelTransitionTime).setEase(GPM.Instance.panelTransitionType);
		//		return base.AdditionalOpeningTween ();
	}

}

