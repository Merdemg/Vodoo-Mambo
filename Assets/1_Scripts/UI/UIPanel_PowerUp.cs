using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPanel_PowerUp : UIPanel
{
    [Header("PowerUp Panel")]
    public Text nameText;
    public Text nameTextShadow;

    public Text descriptionText;

    public void OpenWithPowerUpButton(PowerUpButton button)
    {
        OpenWithPowerUp(button.powerUp);
    }

    void OpenWithPowerUp(PowerUp powerup)
    {
        nameText.text = powerup.name;
        nameTextShadow.text = powerup.name;
        descriptionText.text = powerup.desctription;

        Open();
    }

//    public void LoadWithMedalAlert(MedalAlert medalAlert)
//    {
//        // TODO: ---
//
//        string newMedalString = "You earned a new " + medalAlert.medal + " medal last week by scoring  \n <size=25> " + (medalAlert.score).ToString().Colored("#D73714FF") +
//            "</size> PTS";
//
//        nameText.text = newMedalString;
//
//        Open ();
//    }
}
//new Color(254/255, 200/ 255, 49/255)
