using UnityEngine;
using System.Collections;

public class PowerUp_HourGlass : PowerUp {

	public float timeIncreaseAmount = 5;

	public override void Action ()
	{
		base.Action ();
        GameManager.Instance.newTimer.isBonusActive = true;
        //GPM.Instance.gameplayTime += timeIncreaseAmount;

//      GameManager.Instance.IncreaseRemainingTime (timeIncreaseAmount);
    }

    public override void CleanUp()
    {
        base.CleanUp();

        GameManager.Instance.newTimer.isBonusActive = false;
        //GPM.Instance.gameplayTime -= timeIncreaseAmount;
    }

}
