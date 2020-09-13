using UnityEngine;
using System.Collections;

public class BombShockwave : MonoBehaviour, ISpellObject
{
	public BombSpell spell;
	private BombObject bombObject;

	void Start()
	{
//		CircleCollider2D _collider = GetComponent<CircleCollider2D> ();
		Animator _animator = GetComponent<Animator> ();

		Utility.Instance.WaitTillAnimationTime(_animator, .6f, ()=>{
			if(this == null) {
				Trace.Msg("Can't destroy spell object!, this is ok.");
				return;
			}
			// Spell Objects are destroyed on game end
//			if(bombObject != null) 
				bombObject.shockwaves.Remove(this);
				SpawnManager.Instance.DeSpawnSpellObject(gameObject);
				Destroy(gameObject);

		});

//		LeanTween.value(gameObject, (float value) =>{
//			
//			_collider.radius = value;
////				transform.localScale = new Vector2(value, value);
//			},
//			spell.shockwaveStartRadius, spell.shockwaveFinishRadius, spell.shockwaveGrowDuration).setOnComplete(() =>
//				{
//					SpawnManager.Instance.DeSpawnSpellObject(gameObject);
////					Destroy(gameObject);
//				});
	}

	public void Register(BombObject bombObject)
	{
		this.bombObject = bombObject;
		bombObject.shockwaves.Add (this);
	}

	// When another ball is triggered
	virtual protected void OnTriggerEnter2D(Collider2D other)
	{
		Ball ball = other.GetComponent<Ball> ();

		if(ball != null)
		{
			Boom (ball);
		}
	}

	void Boom(Ball ball)
	{
        // Kinda hack to play level up audio correctly
        if (GetComponent<LevelUpBombShockwave>())
        {
//            AudioManager2.Instance.Play(SoundsManager.Instance.levelup);
        }
        else
            AudioManager2.Instance.Play(SoundsManager.Instance.bombExplosions);

        ScoreManager.Instance.AddScore (ScoreType.Explode, ball.transform.position, ball.level, false);
		SpawnManager.Instance.DeSpawnBall (ball);

		GameObject newShockwaveGO =  SpawnManager.Instance.SpawnSpellObject (spell.shockwaveObjectPrefab, ball.transform.position);
		newShockwaveGO.GetComponent<BombShockwave> ().Register(bombObject);

	}

	public Spell GetSpell()
	{
		return spell;
	}
}

