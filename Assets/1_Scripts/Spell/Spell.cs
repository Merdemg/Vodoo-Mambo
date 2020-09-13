using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

	public TutorialType tutorialType;

	public virtual void Cast(Vector2 position, float? creationAngle = null)
	{
	
	}

    public virtual AudioClipExtended PlaySpawnSFX()
	{
        return null;
	}

    public virtual void Boost(bool isOn){}
}
