using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Tutorial : MonoBehaviour {

	#region Singleton
	public static Tutorial Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

	public GameObject tutorialOverlay;
	[Header ("PinchTutorial")]
	public GameObject pinchAnimPrefab;
	public Transform[] pinchTutorialBallPositions;
	public Transform pinchTutorialAnimPosition;
    public LeanTweenType pinchTransitionEasing = LeanTweenType.easeOutElastic;
	[Header ("Bomb Tutorial")]
	public GameObject bombAnimPrefab;
	[Header ("Blackhole Tutorial")]
	public GameObject blackholeAnimPrefab;
	[Header("Tutorial State")]
	public TutorialType activeTutorialType;

	public void InitTutorial(TutorialType type, GameObject obj = null, Action callback = null) {
		// Can't start multiple tutorials at the same time
		if(activeTutorialType != TutorialType.None) {
			return;
		}

		tutorialOverlay.SetActive (true);

		activeTutorialType = type;

		switch (type) {
		case TutorialType.Pinch:

//			UIManager.Instance.ShowIngame ();
			StartCoroutine (StartPinchTutorial (callback));

			break;
		case TutorialType.SpellBomb:

//			UIManager.Instance.ShowIngame ();
			StartCoroutine (StartBombTutorial (obj.GetComponent<BombObject>(), callback , type));

			break;
		case TutorialType.SpellLevelup:

//			UIManager.Instance.ShowIngame ();
			StartCoroutine (StartBombTutorial (obj.GetComponent<BombObject>(), callback , type));

			break;
		case TutorialType.SpellBlackhole:

//			UIManager.Instance.ShowIngame ();
			StartCoroutine (StartBlackholeTutorial (obj.GetComponent<BlackholeObject>(), callback));

			break;
		default:
			break;
		}
	}
		
	IEnumerator StartPinchTutorial(Action callback)
	{
		GameManager.Instance.PauseTimer (true);
//		GameManager.Instance.newTimer.SetPaused (true);
		Vector2[] ballspawnPositions = new Vector2[]{ pinchTutorialBallPositions[0].position, pinchTutorialBallPositions[1].position};

		// Spawn two level 1 balls at positions
		Ball ball1 = SpawnManager.Instance.SpawnBall (0, ballspawnPositions [0], true, 0);

        // Stop balls' free movement
        ball1.StopMoving ();
        ball1.SetSortingLayer ("TutorialBall");

        yield return new WaitForSeconds(.4f);

        Ball ball2 = SpawnManager.Instance.SpawnBall (0, ballspawnPositions [1], true, 0);
        
        // Stop balls' free movement
        ball2.StopMoving ();
        ball2.SetSortingLayer ("TutorialBall");

        yield return new WaitForSeconds(.4f);

		// Play pinch animation

		GameObject pinchAnim = Instantiate (pinchAnimPrefab, pinchTutorialAnimPosition.transform.position, Quaternion.identity) as GameObject;

        Vector3 pinchAnimScale = pinchAnim.transform.localScale;
        pinchAnim.transform.localScale = Vector3.zero;
        LeanTween.scale(pinchAnim, pinchAnimScale, .4f).setEase(pinchTransitionEasing);

        yield return new WaitForSeconds(.4f);

		// Wait for one of the ball's pinch
		yield return StartCoroutine (ListenBallPinch(ball1));

        LeanTween.scale(pinchAnim, Vector3.zero, .05f).setEase(LeanTweenType.easeOutSine);

		// Stop new ball's movement
		Ball ball3 = GameManager.Instance.balls [0];
		ball3.StopMoving ();
        ball3.SetSortingLayer ("TutorialBall");
        ball3.transform.position = ballspawnPositions [0];

        yield return new WaitForSeconds(.4f);

        // Spawn level 2 ball at position
        Ball ball4 = SpawnManager.Instance.SpawnBall (1, ball3.transform.position + new Vector3(1,3.5f), true, 0);
        ball4.SetSortingLayer ("TutorialBall");
        ball4.StopMoving ();
        ball4.transform.position = ballspawnPositions [1];

        yield return new WaitForSeconds(.4f);

        pinchAnim.transform.localScale = Vector3.zero;
        LeanTween.scale(pinchAnim, pinchAnimScale, .4f).setEase(pinchTransitionEasing);

        // Stop balls' free movement

		// Wait for one of the ball's pinch
		yield return StartCoroutine (ListenBallPinch(ball4));

        // Delay
        yield return new WaitForSeconds(.2f);

		tutorialOverlay.SetActive (false);

        LeanTween.scale(pinchAnim, Vector3.zero, .2f).setEase(LeanTweenType.easeInOutSine).setOnComplete(()=>{
            
            // Destroy tutorial animation
            Destroy (pinchAnim);
        });

		// Go to main menu
//		UIManager.Instance.ShowMainMenu();

		ShowedTutorial (TutorialType.Pinch);

		if(callback != null)
		{
			callback ();
		}

		// Resume Timer
		GameManager.Instance.PauseTimer (false);
		activeTutorialType = TutorialType.None;

	}

	IEnumerator StartBombTutorial(BombObject bombObject, Action callback, TutorialType type = TutorialType.SpellBomb)
	{
		// Stop bomb
		bombObject.GetComponent<Rigidbody2D> ().isKinematic = true;
		bombObject.StopMoving ();

		// Set bomb layer
		bombObject.SetSortingLayer ("TutorialBall");
//		.GetComponent<SpriteRenderer> ().sortingLayerName = layerName;

		// Pause timer
//		GameManager.Instance.PauseTimer (true);
		GameManager.Instance.PauseGame (true);

		// Stop balls and set them inactive
		foreach (var ball in GameManager.Instance.balls) 
		{
			ball.StopMoving ();
			ball.Deactivate();
		}

		// Stop spell objects if floating
		foreach (var so in GameManager.Instance.spellObjects) 
		{
			Floating floating = so.GetComponent<Floating> ();

			if(floating != null)
			{
				floating.StopMoving ();
			}
		}

		// Show tutorial graphics at position
		Vector2 animPos = bombObject.transform.position;
		GameObject anim = Instantiate (bombAnimPrefab, animPos, Quaternion.identity) as GameObject;

		// Wait for bomb explode
        while(!bombObject.exploded)
		{
			yield return true;
		}

		// UnStop balls and set them active
		foreach (var ball in GameManager.Instance.balls) 
		{
			ball.StartMoving ();
			ball.Activate();
		}

		// Continue timer
		Destroy (anim);
		GameManager.Instance.PauseGame (false);

		tutorialOverlay.SetActive (false);

		ShowedTutorial (type);

		if(callback != null)
		{
			callback ();
		}
		activeTutorialType = TutorialType.None;
	}

	IEnumerator StartBlackholeTutorial(BlackholeObject blackholeObject, Action callback)
	{
		// Pause timer
		GameManager.Instance.PauseTimer (true);

        // Setup
        // Set blachole layer
        blackholeObject.SetSortingLayer ("TutorialBall");
        blackholeObject.SetIsTutorial(true);

        // Stop balls and set them inactive
        foreach (var ball in GameManager.Instance.balls) 
        {
            ball.StopMoving ();
            ball.Deactivate();
        }

        // Get new suitable position for tutorial
        Vector2 pos = new Vector3(1,1);

        // Move blackhole to pos
        blackholeObject.transform.position = pos;
		Vector2 animPos = pos;
		// Show tutorial graphics at position
        var anim = Instantiate(blackholeAnimPrefab, animPos, Quaternion.identity);
        var blackholeTutorialAnimation = anim.GetComponent<BlackholeTutorialAnimation>();

		const float ballCount = 3;
//        var selectedBalls = new List<Ball>();
		var ballsLeftToSuck = ballCount;
        for (int i = 0; i < ballCount; i++) {
            // Move one ball to finger position
            //Ball selectedBall = GameManager.Instance.balls[0];
            Ball selectedBall = SpawnManager.Instance.SpawnBall(position: blackholeTutorialAnimation.ballPositions[i].position);

            selectedBall.SetSortingLayer("TutorialBall");
            selectedBall.StopMoving();
//            selectedBalls.Add(selectedBall);

	        var savedIndex = i;
	        selectedBall.BallDestroyed += () =>
	        {
		        blackholeTutorialAnimation.KillAnimation(savedIndex);
		        ballsLeftToSuck--;
	        };
        }

		while (ballsLeftToSuck > 0)	
		{
			yield return true;

		}
        // Wait for suck
//        var allBallsSucked = false;
//        while (!allBallsSucked) {
//            allBallsSucked = true;
//            foreach (var ball in selectedBalls)
//            {
//                allBallsSucked &= !ball.IsActive;
//            }
//            yield return true;
//		}
		// UnStop balls and set them active
		foreach (var ball in GameManager.Instance.balls){
			ball.StartMoving ();
			ball.Activate();
		}

        blackholeObject.SetIsTutorial(false);

		tutorialOverlay.SetActive (false);

		// Continue timer
		Destroy (anim);
		GameManager.Instance.PauseTimer (false);
	
		yield return true;

		ShowedTutorial (TutorialType.SpellBlackhole);

		if(callback != null)
		{
			callback ();
		}
		activeTutorialType = TutorialType.None;
	}

	IEnumerator ListenBallPinch(Ball ball)
	{
        while(ball.IsActive)
		{
			yield return false;
		}
	}

	/// <summary>
	/// !! Returns false if any tutorial is active
	/// </summary>
	/// <returns><c>true</c> if this instance is tutorial shown the specified tutorialType; otherwise, <c>false</c>.</returns>
	/// <param name="tutorialType">Tutorial type.</param>
	public bool IsTutorialShown(TutorialType tutorialType) {
		
		if(activeTutorialType != TutorialType.None) {
			return false;
		}

		string tutorialName = tutorialType.ToString ();

		int tutorialshown = 0;

		tutorialshown = PlayerPrefs.GetInt (tutorialName + "isshown");

		if (tutorialshown == 0) 
		{
			return false;
		}
		else 
		{
			return true;
		}
	}

	void ShowedTutorial(TutorialType type)
	{
		PlayerPrefs.SetInt (type.ToString() + "isshown", 1);
	}

	public void ResetTutorialPrefs()
	{
		PlayerPrefs.SetInt (TutorialType.Pinch + "isshown", 0);
		PlayerPrefs.SetInt (TutorialType.SpellBomb + "isshown", 0);
		PlayerPrefs.SetInt (TutorialType.SpellLevelup + "isshown", 0);
		PlayerPrefs.SetInt (TutorialType.SpellBlackhole + "isshown", 0);
	}
}