using UnityEngine;
using System.Collections;

public class BallMerge : MonoBehaviour
{
	public void Initialize (int ballLevel, float angle)
	{
		// Rotate self aligning the angle
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
		GetComponent<Animator> ().runtimeAnimatorController = AssetManager.Instance.ballMergeAnimatorOverrides [ballLevel];

//		Trace.Msg (ballLevel);

	}

	public static void PreparePrefab(int level)
	{
//		// Swap the animation with the new one
//		Animator animator = AssetManager.Instance.ballMergePrefab.GetComponent<Animator> ();
//		RuntimeAnimatorController animatorController = animator.runtimeAnimatorController;
//
//		AnimatorOverrideController overrideController = new AnimatorOverrideController ();
//
//		overrideController.runtimeAnimatorController = animatorController;
//		overrideController ["BallMerge"] = AssetManager.Instance.ballMergeAnimationOverrides [level];

//		AssetManager.Instance.ballMergePrefab.GetComponent<Animator> ().runtimeAnimatorController = AssetManager.Instance.ballMergeAnimatorOverrides [level];
		
	}
}

