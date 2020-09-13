//#define USE_LOGS
using UnityEngine;
using System.Collections;

public class Holder : MonoBehaviour
{
	public IHoldable holdingObject;

	void Update()
	{
		if(holdingObject == null && GameManager.Instance.GameplayState == GameplayState.Playing)
		{
			FindHoldable ();
		}
	}

	void FindHoldable()
	{
        
		// Find closest holdable if holding object is null
		IHoldable closestHoldable = null;
		var closestDistance = 1000f;

		// Find closest holdable
		foreach (IHoldable holdable in SpawnManager.Instance.holdables) 
		{
			// Continue if holdable is not holdable
			if (!holdable.IsHoldable ())
				continue;

			float distSqr = (transform.position - ((MonoBehaviour)holdable).transform.position).sqrMagnitude;

			if(distSqr < closestDistance)
			{
				closestHoldable = holdable;
				closestDistance = distSqr;
			}
		}

		// Hold the found object
		if(closestDistance < GPM.Instance.holdThresholdSqr && closestHoldable != null)
		{
			StartHolding (closestHoldable);
		}
		
	}

	public void StartHolding(IHoldable holdable)
	{
        Trace.Msg("Holder starting holding: " + gameObject);
		holdingObject = holdable;
		holdable.StartHolding (transform.position);
		GameControlManager.Instance.HoldableRegistration (holdingObject, true);
	}

//	public void MoveToPosition(Vector2 position, Vector2 velocity)
//	{
//		transform.position = position;
//		if(holdingObject != null)
//		{
//			holdingObject.HoldTransformed (position);
//		}
//	}

	public void MoveToDeltaPosition(Vector2 deltaPosition, Vector2 velocity)
	{
		transform.position += (Vector3)deltaPosition;
		if(holdingObject != null)
		{
			holdingObject.HoldTransformed (deltaPosition);
		}
	}

	public void StopHolding(Vector2 velocity)
	{
//		if (holdingObject != null && !Equals (holdingObject, null) && (MonoBehaviour)holdingObject != null)
		if ((MonoBehaviour)holdingObject != null)
		{
			holdingObject.StopHolding (velocity);
			GameControlManager.Instance.HoldableRegistration (holdingObject, false);
		}

		Destroy (gameObject);
	}

	public void BallPinched()
	{
		Destroy (gameObject);
	}
}

