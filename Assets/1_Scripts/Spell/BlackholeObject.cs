using UnityEngine;
using System.Collections;

public class BlackholeObject : MonoBehaviour
{
	public BlackholeSpell spell;
	public GameObject[] parts;
	public float[] rotationSpeeds;

    [Header("Info")]
    [SerializeField]AudioClipExtended activeClip;
    [SerializeField]bool isTutorial;

	void Start()
	{
//		Invoke ("Die", spell.duration);

        StartCoroutine(DieRoutine());

        activeClip = spell.PlaySpawnSFX();
	}

	void Update()
	{
		Rotate ();
	}

    IEnumerator DieRoutine()
    {
        float timePassed = 0;
        while(timePassed <= spell.duration)
        {
            timePassed += Time.deltaTime;
            if(GameManager.Instance.GameplayState != GameplayState.Paused)
            {
                yield return true;
                continue;
            }
            yield return true;
        }

        Die();
    }

    public void SetIsTutorial(bool isTut)
    {
        isTutorial = isTut;

        if(!isTut)
        {
            Invoke ("Die", spell.duration);
        }
    }

	void OnTriggerStay2D(Collider2D other)
	{
		Ball ball = other.GetComponent<Ball> ();

		if(ball != null && CanSuckBall(ball))
		{
//			if(CanSuckBall)
			Suck (ball);
		}
	}

	bool CanSuckBall(Ball ball)
	{
		// TODO: Fix the had coded value && maybe implement velocity based check
        Trace.Msg("--- Suck ball");
        Trace.Msg("ball isActive: " + ball.IsActive);
        Trace.Msg("last interaction time dif: " + (Time.time - ball.LastInteractionTime));
		return Time.time - ball.LastInteractionTime < 1f && ball.IsActive;
	}

	public void DebugSuck(Ball ball) {
		Suck (ball);
	}

	void Suck(Ball ball) {
        ScoreManager.Instance.AddScore (ScoreType.Blackhole, ball.transform.position, ball.level, false);

		ball.Deactivate ();

		float duration = spell.suckDuration;

		LeanTween.scale(ball.gameObject, Vector3.zero, duration).setEase(LeanTweenType.easeOutSine);

		LeanTween.move (ball.gameObject, transform.position, duration).setEase(LeanTweenType.easeInOutSine).setOnComplete(()=> {
			
			SpawnManager.Instance.DeSpawnBall (ball);
            AudioManager2.Instance.Play (SoundsManager.Instance.blackholeSucks);

		});
	}

	void Die()
	{
        if(isTutorial)
        {
            return;
        }

		AudioManager2.Instance.Play (SoundsManager.Instance.blackholeEnd);
		SpawnManager.Instance.DeSpawnSpellObject (gameObject);

        AudioManager2.Instance.Stop(activeClip);
//        spell.StopSFX();
	}

	Spell GetSpell()
	{
		return spell;
	}

	// Animations
	void Rotate()
	{
		for (int i = 0; i < parts.Length; i++) 
		{
			GameObject part = parts [i];
			float rotationSpeed = rotationSpeeds [i];

			part.transform.Rotate(new Vector3 (0, 0, rotationSpeed));

		}

	}

	public void SetSortingLayer(string layerName)
	{
//		GetComponent<SpriteRenderer> ().sortingLayerName = layerName;

		GetComponent<SpriteRenderer> ().sortingLayerName = layerName;
	}
}

