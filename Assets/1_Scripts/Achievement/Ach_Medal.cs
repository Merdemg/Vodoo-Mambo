using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ach_Medal : Achievement 
{
	/// <summary>
	/// 0 = Gold, 1 = Silver etc.
	/// </summary>
	[Space]
	[SerializeField]int medalIndex;

    /// <summary>
    /// Checks if achievement is achieved.
    /// </summary>
    /// <returns><c>true</c>, if new was checked, <c>false</c> otherwise.</returns>
	public override bool CheckNew ()
	{
		bool condition = GameSparksManager.Instance.player.medals [medalIndex] > 0;

		if(!earned && condition)
		{
			Achieve ();
            return true;
		}

        return false;
	}
}
