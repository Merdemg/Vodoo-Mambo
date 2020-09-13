using UnityEngine;
using System.Collections;

public class BombSpell : Spell
{
	[Header ("Parameters")]
	public GameObject bombObjectPrefab;
	public GameObject shockwaveObjectPrefab;
	[Space]
	public float shockwaveStartRadius;
	public float shockwaveFinishRadius;
	public float shockwaveGrowDuration;


	public override void Cast (Vector2 position, float? creationAngle)
	{
        Trace.Msg("Casting Bomb Spell");
        if(GetComponent<LevelUpBombSpell>() != null)
        {
            GetComponent<LevelUpBombSpell>().PlaySpawnSFX();
        }
        else
		    PlaySpawnSFX ();

		// Show shaman
		string shamanMessage = Lean.Localization.LeanLocalization.GetTranslationText( "Shaman--Boom" );
		UIManager.Instance.ingameShaman.Show( shamanMessage );
		// Spawn spell object
		GameObject bombGO = SpawnManager.Instance.SpawnSpellObject (bombObjectPrefab, position);
		BombObject bombObject = bombGO.GetComponent<BombObject> ();

		// Check tutorial
		if(!Tutorial.Instance.IsTutorialShown(tutorialType))
		{
			Tutorial.Instance.InitTutorial (tutorialType, bombGO);
		}

		if(creationAngle != null)
		{
			bombObject.InitializeWithMerge ((float)creationAngle);
		}
		else
		{
			bombObject.InitializeWithoutMerge ();
		}
	}

    public override AudioClipExtended PlaySpawnSFX ()
	{
		return AudioManager2.Instance.Play (SoundsManager.Instance.bombSpawn);
	}

    public override void Boost(bool isOn)
    {
        // TODO: Sistematige oturt
        if(isOn)
            shockwaveFinishRadius *= 1.1f;
        else
            shockwaveFinishRadius /= 1.1f;
            
    }
}

