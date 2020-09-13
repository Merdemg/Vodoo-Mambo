//using UnityEngine;
//using System.Collections;
//
//public class obs_BallPair : MonoBehaviour
//{
//	Ball[] balls;
//
//	bool released = false;
//
//	public void Init(Ball[] _balls)
//	{
//		balls = _balls;
//
//		foreach (var ball in _balls) {
//			ball.ballPair = this;
//		}
//	}
//
//	void Update()
//	{
//		// Check if any of the balls are associated with other pair; if so kill
//		foreach (var ball in balls) 
//		{
//			if(ball == null || ball.ballPair != this)
//			{
//				Die ();
//			}
//		}
//
//		// Check balls distance to pinch
//		float dist = (balls [0].transform.position - balls [1].transform.position).magnitude;
//
////		Trace.Msg ("dist: " + dist);
////		Trace.Msg ("dist with thresholds: " + (dist - GPM.Instance.pinchThreshold));
////		Trace.Msg (balls [0].Radius * 2);
//
////		float distThreshold = dist - GPM.Instance.pinchThreshold;
//
//		if(dist - GPM.Instance.pinchDistanceThreshold <= balls[0].Radius * 2)
//		{
//			Die ();
//			GameControlManager.Instance.Pinch (balls);
//		}
//	}
//
//	void Die()
//	{
//		// Set balls pair values to null if it is this
//		foreach (var ball in balls) 
//		{
//			if (ball.ballPair == this)
//				ball.ballPair = null;
//		}
//		Destroy (gameObject);
//	}
//
//	public void ReleaseInitiated()
//	{
//		if (released)
//			return;
//
//		Invoke ("Die", GPM.Instance.ballPairLifeAfterRelease);
//	}
//
//	public Vector2 GetReleaseVelocityForBall(Ball ball)
//	{
//		// Get ball index
//		int ballIndex = System.Array.IndexOf (balls, ball);
//		Ball otherBall = ballIndex == 0 ? balls[1] : balls[0];
//
//		// Get velocity
//		Vector3 vel = (otherBall.transform.position - ball.transform.position).normalized * GPM.Instance.ballPairedReleaseVelocityMagnitude;
//
//		return vel;
//	}
//
//}
//
