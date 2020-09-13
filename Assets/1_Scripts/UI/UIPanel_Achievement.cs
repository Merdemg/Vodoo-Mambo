using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIPanel_Achievement : UIPanel
{
	[Header("UIPanel_Achievement")]
	[SerializeField]Text name;
	[SerializeField]Text nameShadow;
	[SerializeField]Image icon;
	[SerializeField]Text descpription;
	[SerializeField]Text coinValue;

	[Header("Info")]
	[SerializeField]List<Achievement> pendingAchievements = new List<Achievement>();
    public bool IsPending
    {
        get{
            return pendingAchievements.Count > 0;
        }
    }

	public void OpenWithAchievement(Achievement achievement)
	{
        // Add to pending if is open or game is active
        if(IsOpen || GameManager.Instance.GameplayState != GameplayState.Stopped)
		{
			pendingAchievements.Add (achievement);
			return;
		}

		Open ();

		string achievementTitle = Lean.Localization.LeanLocalization
							.GetTranslationText( "Achievement--Alert-Title--" + achievement.shortCode );
		if ( achievementTitle.Length < 1 ) {
			achievementTitle = achievement.name; }

		string achievementText = Lean.Localization.LeanLocalization
							.GetTranslationText( "Achievement--Alert-Text--" + achievement.shortCode );
		if ( achievementText.Length < 1 ) {
			achievementText = achievement.description; }

		name.text = achievementTitle;
		nameShadow.text = achievementTitle;
		icon.sprite = achievement.icon;
		descpription.text = achievementText;
		coinValue.text = achievement.coinValue.ToString ();

        AudioManager2.Instance.Play(SoundsManager.Instance.achievementPanel, AudioClipExtended.AudioType.SFX);
	}

	protected override LTDescr AdditionalOpeningTween ()
	{
		return ScaleTweenOpening ();

//		panel.gameObject.transform.localScale = Vector3.zero;
//		return LeanTween.scale (panel, Vector3.one, GPM.Instance.panelTransitionTime).setEase (GPM.Instance.panelTransitionType);
//		return base.AdditionalOpeningTween ();
	}

	protected override LTDescr AdditionalClosingTween ()
	{
		return ScaleTweenClosing ();
//		panel.gameObject.transform.localScale = Vector3.zero;
//		return LeanTween.scale (panel, Vector3.one * 1.2f, GPM.Instance.panelTransitionTime).setEase (GPM.Instance.panelTransitionType);
		//		return base.AdditionalOpeningTween ();
	}

	public void CloseCustom()
	{
		if(pendingAchievements.Count>0)
		{
			OpenWithAchievement (pendingAchievements [0]);
			pendingAchievements.RemoveAt (0);
		}	
	}

	public override void Close()
	{
		Trace.Msg ("UI Panel Closing - " + name);
		CancelCurrentTweens ();


        _currentTween =  LeanTween.alphaCanvas (_canvasGroup, 0, GPM.Instance.panelTransitionTime).setEase (GPM.Instance.panelTransitionType).setOnComplete(()=>{


            UIManager.Instance.OnPanelClosing (this);
			gameObject.SetActive(false);

//			if(pendingAchievements.Count>0)
//			{
//				OpenWithAchievement (pendingAchievements [0]);
//				pendingAchievements.RemoveAt (0);
//			}	
            isOpen = false;
            OpenIfPending();

		});

		_additionalTween = AdditionalClosingTween ();
	}

    public void OpenIfPending()
    {

        if(pendingAchievements.Count>0)
        {
            OpenWithAchievement (pendingAchievements [0]);
            pendingAchievements.RemoveAt (0);
        }   
    }
}

