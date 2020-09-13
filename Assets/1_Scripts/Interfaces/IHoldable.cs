using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public interface IHoldable
{

	void StartHolding (Vector2 position);

	void StopHolding (Vector2 velocity);

	void HoldTransformed (Vector2 deltaPosition);

	bool IsHoldable ();
}

