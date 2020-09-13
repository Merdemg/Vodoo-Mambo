using UnityEngine;
using System.Collections;

public class SpellManager : MonoBehaviour {

	#region Singleton

	public static SpellManager Instance { private set; get; }
	
	void Awake()
	{
		Instance = this;
	}
	#endregion

	public Spell[] spells;
    public int currentPseudoRandomIndex = 0;

    private void Start() {
        GameManager.Instance.OnGameStart += () => {
            spells.Shuffle(); 
        };
    }

    public Spell GetRandomSpell()
	{
        var randomSpell = spells [currentPseudoRandomIndex];
		currentPseudoRandomIndex++;

        if(currentPseudoRandomIndex >= spells.Length) {
            currentPseudoRandomIndex = 0;
            spells.Shuffle();
        }

        return randomSpell;
	}
}
