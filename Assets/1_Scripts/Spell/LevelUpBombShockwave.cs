using UnityEngine;
using System.Collections;

public class LevelUpBombShockwave : BombShockwave
{
	protected override void OnTriggerEnter2D (Collider2D other)
	{
		Ball ball = other.GetComponent<Ball> ();

		if(ball != null)
		{
			LevelUp (ball);
		}	
	}

	void LevelUp(Ball ball)
	{
		ball.LevelUp ();
        ScoreManager.Instance.AddScore(ScoreType.Pinch, ball.transform.position, ball.level, false);
	}

}

