using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public interface ITappable
{

	void Tapped (TapGesture gesture);

	bool IsTappable ();
}

