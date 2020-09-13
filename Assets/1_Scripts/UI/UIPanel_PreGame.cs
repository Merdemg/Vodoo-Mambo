using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPanel_PreGame : UIPanel {

    [Header("UIPanel_PreGame")]
	public Text coinsText;
    public Text powerupDescription;
    public string initialPowerupText = "SELECT ONE OR MORE BOOSTS!";

	public void SetCoins(int coinsCount)
	{
		coinsText.text = coinsCount.ToString ();
	}

    public override void Open()
    {
        base.Open();

        // Feature: Localization
        // 2019.12.19 - LEVON
        //
        string localizationKey = "PreGame--PowerUp-Description-Select";
        string localizedText = Lean.Localization.LeanLocalization.GetTranslationText( localizationKey );
        powerupDescription.text = localizedText;
		//
		// End of Feature

        // powerupDescription.text = initialPowerupText;
        
        coinsText.text = ((int)MetaManager.Instance.Get(MetaManager.MetaType.coins)).ToString();
    }

    protected override LTDescr AdditionalOpeningTween()
    {
        Vector3 cachedPos = panel.transform.position;
        panel.transform.position += Vector3.down * (Camera.main.orthographicSize * 2);

        return LeanTween.move(panel.gameObject, cachedPos, GPM.Instance.panelTransitionTime).setEase(LeanTweenType.easeOutExpo);

//        return base.AdditionalOpeningTween();

    }

    public void ShowPowerUpDescription(PowerUp powerupButton)
    {
    	// Feature: Localization
        // 2019.12.19 - LEVON
        //
        string powerupKey = powerupButton.name.Replace( " ", "-" ).Replace( ".", "" ).ToLower();
        string localizationKey = ( "PreGame--PowerUp-Description--" + powerupKey );
        string localizedText = Lean.Localization.LeanLocalization.GetTranslationText( localizationKey );
        powerupDescription.text = localizedText;
		//
		// End of Feature

        // powerupDescription.text = powerupButton.desctription;
    }
}
