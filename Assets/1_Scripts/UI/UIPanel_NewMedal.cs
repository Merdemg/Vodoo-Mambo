using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPanel_NewMedal : UIPanel
{
	public Text medalText;

	public void LoadWithMedalAlert(MedalAlert medalAlert)
	{

		string medalName = ( "<size=18>" + medalAlert.medal.Colored("#D73714FF") + "</size>" );
		string medalScore = ( "<size=20>" + (medalAlert.score).ToString().Colored("#D73714FF") + "</size>" );

		string alertText = Lean.Localization.LeanLocalization
							.GetTranslationText( "Medal--Alert-Text" )
							.Replace( "%MedalName%", medalName )
							.Replace( "%MedalScore%", medalScore );

		// string newMedalString = "You earned a new " + medalAlert.medal + " medal last week by scoring  \n <size=25> " + (medalAlert.score).ToString().Colored("#D73714FF") +
		// 	"</size> PTS";

		medalText.text = alertText;

		Open ();
	}
}
//new Color(254/255, 200/ 255, 49/255)
