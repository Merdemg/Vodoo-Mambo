using UnityEngine;
using System.Collections;

public class PowerUp_Bomb : PowerUp {

	public Spell bombSpell;

	public override void Action ()
	{
		base.Action ();

        bombSpell.Boost(true);
	}

    public override void CleanUp()
    {
        bombSpell.Boost(false);
    }

}
