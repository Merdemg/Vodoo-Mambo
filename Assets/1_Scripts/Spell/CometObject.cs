using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CometObject : Floating, IHoldable {

	public CometSpell spell;

	bool isShooting = false;
//	bool isGettingHold = false;
	Vector2 holdingTargetPosition;

	List<Ball> hitBalls = new List<Ball> ();


	void Start()
	{
		AssignVelocityRandomValue ();
	}

	protected override void FixedUpdate()
	{
		if(isGettingHold)
		{
			HoldingLoop ();
		}
		else if(isMoving)
		{
//			AdjustFreeMovement ();
			KeepInGameArea ();
		}

		if(!isShooting)
		{
			AdjustFreeMovement ();
		}
	}

	void HoldingLoop ()
	{
		// Move bomb towards holding target location
		Vector3 newPos = Vector3.Lerp (transform.position, holdingTargetPosition, Time.deltaTime * GPM.Instance.ballHoldResponseMultiplier);
		_rigidbody.MovePosition (newPos);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Ball ball = other.GetComponent<Ball> ();

		if(ball != null && !hitBalls.Contains(ball))
		{
			hitBalls.Add (ball);
		}
	}

	IEnumerator Shoot()
	{
		yield return new WaitForSeconds (.1f);

		isShooting = true;

		// Wait while shooting
		while(isShooting)
		{
			Trace.Msg (_rigidbody.velocity.sqrMagnitude);
			isShooting = _rigidbody.velocity.sqrMagnitude > spell.shootStopVelocityMagSqr;

			yield return true;
		}
		// Shoot ended

		// Kill Balls
		foreach (var ball in hitBalls) 
		{
            ScoreManager.Instance.AddScore (ScoreType.Explode, ball.transform.position, ball.level, false);
			SpawnManager.Instance.DeSpawnBall (ball);
		}

		// Move rest of the balls
		foreach (var ball in GameManager.Instance.balls) 
		{
			ball.StartMoving ();
		}

		// Kill self
		SpawnManager.Instance.DeSpawnSpellObject (gameObject);
	}

	#region IHoldable implementation

	public void StartHolding(Vector2 position)
	{
		//		lastInteractionTime = Time.time;
		isGettingHold = true;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.isKinematic = true;

		holdingTargetPosition = position;
	}

	public void StopHolding(Vector2 velocity)
	{
		isGettingHold = false;
		_rigidbody.isKinematic = false;

		// Stop every ball
		foreach (var ball in GameManager.Instance.balls) 
		{
			ball.StopMoving ();
		}

		// Trigger
		_collider.isTrigger = true;

		// Start Shoot coroutine
		StartCoroutine (Shoot ());

		// Move comet as the same direction as last finger movement
		// Clamp velocity 
		if(velocity.magnitude > spell.maxVelocityMagnitude)
		{
			velocity = velocity.normalized * spell.maxVelocityMagnitude;
		}
		else if(velocity.magnitude < spell.minVelocityMagnitude)
		{
			velocity = velocity.normalized * spell.minVelocityMagnitude;
		}

		_rigidbody.velocity = velocity;
	}

	public void HoldTransformed(Vector2 position)
	{
		//		lastInteractionTime = Time.time;

		//		holdingTargetPosition += gestureData.DeltaPosition;
		//		holdingTargetPosition = Camera.main.ScreenToWorldPoint (gestureData.ScreenPosition);
		holdingTargetPosition = position;

		//		Vector3 gestureVelocity = gestureData.DeltaPosition / Time.deltaTime;

		//		lastHoldingVelocity = velocity;
	}

	public bool IsHoldable()
	{
		return !isGettingHold && !isShooting;
	}
	#endregion


}
