using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_BoostSpell : PowerUp {

    public Spell spell;

    public override void Action ()
    {
        base.Action ();

        spell.Boost(true);
    }

    public override void CleanUp()
    {
        spell.Boost(false);
    }

}
