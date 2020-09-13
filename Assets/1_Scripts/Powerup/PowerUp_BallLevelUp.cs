using UnityEngine;
using System.Collections;

public class PowerUp_BallLevelUp : PowerUp {

	public override void Action ()
	{
		base.Action ();


//		foreach (var ball in GameManager.Instance._ballList)
		foreach (var ball in GameManager.Instance.balls)
		{
			ball.GetComponent<Ball> ().LevelUp ();
		}
	}
}
