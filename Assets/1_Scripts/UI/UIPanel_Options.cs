using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPanel_Options : UIPanel
{
	[Header("Parameters")]
	[SerializeField]string musicPrefKey = "pitchpincher_options_music";
	[SerializeField]string soundPrefKey = "pitchpincher_options_sound";
	[SerializeField]UIToggle musicToggle;
	[SerializeField]UIToggle soundToggle;
    [SerializeField]Button tutorialButton;
	[SerializeField]Button cinematicsButton;
	[SerializeField]Button creditsButton;

//	[Header("Info")]
//	bool _isMusicOn = true;
//	bool _isSoundOn = true;

	// API
	public bool IsMusicOn{
		get{
			return musicToggle.GetValue();
		}
	}

	public bool IsSoundOn{
		get{
			return soundToggle.GetValue();
		}
	}

	protected override void Start()
	{
		base.Start ();
        tutorialButton.onClick.AddListener (TutorialButtonClick);
        cinematicsButton.onClick.AddListener (CinematicButtonClick);
	}

	public void Init()
	{
		LoadPrefsIn ();
	}

	void LoadPrefsIn()
	{
		bool _isMusicOn = PlayerPrefs.GetInt (musicPrefKey, 1) == 1;
		bool _isSoundOn = PlayerPrefs.GetInt (soundPrefKey, 1) == 1;

		musicToggle.Set (_isMusicOn, false);
		soundToggle.Set (_isSoundOn, false);
		ApplyOptions ();
	}

	void SetPrefs()
	{
		PlayerPrefs.SetInt (musicPrefKey, musicToggle.GetValueInt());
		PlayerPrefs.SetInt (soundPrefKey, soundToggle.GetValueInt());
	}

	public void ApplyOptions()
	{
//		AudioManager2.Instance.SetMusic (IsMusicOn);
		AudioManager2.Instance.SetSound(IsSoundOn, IsMusicOn);

		SetPrefs ();
	}

    public void TutorialButtonClick()
    {
        Tutorial.Instance.ResetTutorialPrefs ();
        UIManager.Instance.OnPlayClicked ();
    }

	public void CinematicButtonClick()
	{
        PitchCinematic.Instance.ShowCinematic();
		//UIManager.Instance.OnPlayClicked ();
	}

}

