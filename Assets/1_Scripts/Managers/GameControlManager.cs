//#define USE_LOGS
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameControlManager : MonoBehaviour
{
	#region Singleton
	public static GameControlManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

	public List<Ball> holdingBalls;
	public List<GameObject> holdingOthers;

	// TODO: make private
	public List<Ball> lastReleasedBalls;
//	public Ball lastReleasedBall;
//	public Ball LastReleasedBall
//	{
//		get{
//			return lastReleasedBall;
//		}
//	}

    void Start()
    {
        StartCoroutine(RemoveLastReleaseBottomLoop());
    }

	void Update ()
	{
//        DebugConsole.Log(holdin)

	}

	public void Pinch(Ball[] balls)
	{
		// Get balls
		Ball theBall = balls [0];
		Ball otherBall = balls [1];
//		print (theBall);
//		print (otherBall);

		Vector3 midPointOffBalls = new Vector3((otherBall.transform.position.x + theBall.transform.position.x) / 2f, (otherBall.transform.position.y + theBall.transform.position.y) / 2f);
		float angle = Utility.Instance.AngleBetweenVector2 (theBall.transform.position, otherBall.transform.position);

		Trace.Msg ("Mid Point of Balls = " + midPointOffBalls);

		// Play SFX (not far 
//        if(theBall.level <= 2)
//        {
//            AudioManager2.Instance.Play (SoundsManager.Instance.mergeLevels [theBall.level ]);
//        }
        int comboLevel = ScoreManager.Instance.GetComboLevel();

        // Check if sound exists
        if (comboLevel < SoundsManager.Instance.mergeByCombo.Length)
        {
            AudioManager2.Instance.Play(SoundsManager.Instance.mergeByCombo[comboLevel], AudioClipExtended.AudioType.SFX, 1, true);
        }
        // If not play last sound
        else
        {
            AudioManager2.Instance.Play(SoundsManager.Instance.mergeByCombo[SoundsManager.Instance.mergeByCombo.Length - 1]);
        }
        Trace.Msg(comboLevel);

//		AudioManager.Instance.PlayPop ();

		// Add score
		ScoreManager.Instance.AddScore (ScoreType.Pinch, midPointOffBalls, theBall.level);

		// Count balls
		GameManager.Instance.IncrementBallCount (theBall.level);

		// Send balls event
		theBall.StartPinch();
		otherBall.StartPinch();

		// Spawn new ball or spell
		if (theBall.level < GPM.Instance.ballMaxLevel)
		{
			SpawnManager.Instance.SpawnBall (theBall.level + 1, midPointOffBalls, false, angle);
		}
		else 
		{
			SpellManager.Instance.GetRandomSpell ().Cast (midPointOffBalls, angle);
		}

//		BallMerge.PreparePrefab (theBall.level);

		// Instantiate ( we need pooling here probably ) animation object and start animation
//		GameObject ballMerge = Instantiate (AssetManager.Instance.ballMergePrefab, midPointOffBalls, Quaternion.identity) as GameObject; 
//
//		ballMerge.GetComponent<BallMerge> ().Initialize (theBall.level, angle);
//
//		// Wait till animation end
//		Animator animator = ballMerge.GetComponent<Animator> ();
//
//		Utility.Instance.WaitTillAnimationEnd (animator, () => {
//			
//			Destroy (ballMerge);
//
//			// Spawn new ball or spell
//			if (theBall.level < GPM.Instance.ballMaxLevel) {
//				SpawnManager.Instance.SpawnBall (theBall.level + 1, midPointOffBalls, false);
//			} else {
//				SpellManager.Instance.GetRandomSpell ().Cast (midPointOffBalls);
//			}
//		});

		SpawnManager.Instance.DeSpawnBall(theBall);
		SpawnManager.Instance.DeSpawnBall(otherBall);


	}
		
//	public void BallDespawned(Ball ball)
//	{
//		holdingBalls.Remove (ball);
//		lastReleasedBalls.Remove (ball);
//	}

//	void obs_CheckPairs ()
//	{
//		// TODO: Check pinches for more than 2 balls
//		if(holdingBalls.Count == 2 && holdingBalls[0].level == holdingBalls[1].level)
//		{
//			// Create ball pair
//			obs_BallPair pair = new GameObject ("ballPair").AddComponent<obs_BallPair> ();
//			pair.Init (holdingBalls.ToArray ());
//
//			holdingBalls = new List<Ball> ();
////			float dist = (holdingBalls [0].transform.position - holdingBalls [1].transform.position).magnitude;
////
////			if(dist - GPM.Instance.pinchThreshold <= holdingBalls[0].Radius * 2)
////			{
////				Pinch ();
////			}
//		}
//	}

	public void HoldableRegistration(IHoldable holdable, bool isAdd)
	{
		MonoBehaviour holdedMono = (MonoBehaviour)holdable;
		
		Ball ball = holdedMono.GetComponent<Ball>();

		if(ball != null)
		{
			if(isAdd)
				holdingBalls.Add (ball);
			else
			{
				holdingBalls.Remove (ball);
				LastRelease (ball);
			}
		}
		else
		{
			if (isAdd)
				holdingOthers.Add (holdedMono.gameObject);
			else
				holdingOthers.Remove (holdedMono.gameObject);
		}
	}

	/// <summary>
	/// Find a holded ball or a ball released lately.
	/// </summary>
	/// <returns>The target for released ball.</returns>
	/// <param name="ball">Ball.</param>
	public Ball GetTargetForReleasedBall(Ball ball)
	{
		foreach (var b in holdingBalls) 
		{
			if(b != null &&
				ball != b &&
				ball.level == b.level &&
				ball.CanPinch())
			{
				return b;
			}
		}

		// Iterating from top of the list, to get the most recent released ball
		for (int i = lastReleasedBalls.Count - 1; i >= 0 ; i--) {
			
			Ball b = lastReleasedBalls [i];

			if(b != null &&
				ball != b &&
				ball.level == b.level &&
				ball.CanPinch())
			{
				return b;
			}

		}

		return null;
	}

	/// <summary>
	/// Adds ball to last released list.
	/// If list length > 4, remove first
	/// </summary>
	/// <param name="ball">Ball.</param>
	void LastRelease(Ball ball)
	{
		int maxLastReleasedBalls = GPM.Instance.maxLastReleasedBalls;

		lastReleasedBalls.Add (ball);
		if(lastReleasedBalls.Count > maxLastReleasedBalls)
		{
			lastReleasedBalls.RemoveAt (0);
		}
	}

    IEnumerator RemoveLastReleaseBottomLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(.2f);

            if(lastReleasedBalls.Count > 0)
                lastReleasedBalls.RemoveAt(0);
        }
    }

//	public Vector2 GetReleaseVelocityForBall(Ball ball, Ball targetBall)
//	{
//		// Get velocity
//		Vector3 vel = (targetBall.transform.position - ball.transform.position).normalized * GPM.Instance.ballPairedReleaseVelocityMagnitude;
//
//		return vel;
//	}
}

