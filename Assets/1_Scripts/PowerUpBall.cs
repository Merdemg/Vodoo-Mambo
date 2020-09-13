using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class PowerUpBall : Floating
{
	PowerUp powerUp;
	bool activated = false;

	void Start()
	{
		GetComponent<TapGesture>().Tapped += TappedHandler;
	}

	public void Initialize(PowerUp p)
	{
		powerUp = p;
//		GetComponent<SpriteRenderer> ().sprite = powerUp.sprite;
	}

	void OnEnable()
	{
		GetComponent<TapGesture>().Tapped += TappedHandler;
	}

	void OnDisable()
	{
		GetComponent<TapGesture>().Tapped -= TappedHandler;
	}


	private void TappedHandler(object sender, System.EventArgs e)
	{
		if (activated)
			return;
		
		activated = true;

		powerUp.Action ();

		LeanTween.scale(gameObject, new Vector3(GPM.Instance.powerUpDestroySize,GPM.Instance.powerUpDestroySize,1), GPM.Instance.powerUpDestroyDuration).setOnComplete(() =>
			{
				Destroy(gameObject);
//				GameManager.Instance.DestroyBomb(gameObject);
			});
	}
}

