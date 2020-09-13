using UnityEngine;
using System.Collections;

public class PowerUp_Combo : PowerUp {

	public override void Action ()
	{
		base.Action ();

		ScoreManager.Instance.minimumComboMultiplier = 2;
	}

	public override void CleanUp ()
	{
		ScoreManager.Instance.minimumComboMultiplier = 1;
	}
}
