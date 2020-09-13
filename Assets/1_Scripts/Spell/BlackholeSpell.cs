using UnityEngine;
using System.Collections;

public class BlackholeSpell : Spell
{
	public GameObject blackholePrefab;
	public float duration = 2f;
	public float suckDuration;

	public override void Cast (Vector2 position, float? creationAngle)
	{
//		PlaySpawnSFX ();

		// Show shaman
		string shamanMessage = Lean.Localization.LeanLocalization.GetTranslationText( "Shaman--Blackhole" );
		UIManager.Instance.ingameShaman.Show( shamanMessage );
		// Spawn spell object
		GameObject blackholeObject = SpawnManager.Instance.SpawnSpellObject (blackholePrefab, position);

		// Check tutorial
		if(!Tutorial.Instance.IsTutorialShown(TutorialType.SpellBlackhole))
		{
			Tutorial.Instance.InitTutorial (TutorialType.SpellBlackhole, blackholeObject);
		}
	}

    public override AudioClipExtended PlaySpawnSFX ()
	{
        return AudioManager2.Instance.Play (SoundsManager.Instance.blackholeSpawn);
    }

    public override void Boost(bool isOn)
    {
        if(isOn)
            duration *= 1.4f;
        else
            duration /= 1.4f;
            
    }
}

