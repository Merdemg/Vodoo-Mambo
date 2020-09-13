using UnityEngine;
using System.Collections;

public class CometSpell : Spell {

	public GameObject cometObjectPrefab;

	public float minVelocityMagnitude = 2f;
	public float maxVelocityMagnitude = 10f;

	public float shootStopVelocityMagSqr = 10;



	public override void Cast (Vector2 position, float? creationAngle)
	{
		SpawnManager.Instance.SpawnSpellObject (cometObjectPrefab, position);
	}
}
