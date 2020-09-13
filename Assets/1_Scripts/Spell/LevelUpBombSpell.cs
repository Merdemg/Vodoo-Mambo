using UnityEngine;
using System.Collections;

public class LevelUpBombSpell : BombSpell
{



    public override AudioClipExtended PlaySpawnSFX ()
	{
		return AudioManager2.Instance.Play (SoundsManager.Instance.levelupSpawn);
	}
}

