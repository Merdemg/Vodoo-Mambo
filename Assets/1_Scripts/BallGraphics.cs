using UnityEngine;
using System.Collections;

public class BallGraphics : MonoBehaviour
{
	Ball ball;
	public GameObject icon;

	public void Awake()
	{
		icon = transform.GetChild (0).gameObject;
	}

	public void Initialize (Ball _ball)
	{
		ball = _ball;
		GetComponent<SpriteRenderer>().sprite = AssetManager.Instance.ballSpritesByLevel [ball.level];
		icon.transform.localScale = Vector3.zero;
	}

	public void InitializeIcon ()
	{
		SpriteRenderer graphicsRenderer = icon.GetComponent<SpriteRenderer> ();
		graphicsRenderer.sprite = AssetManager.Instance.ballIconSpritesByLevel[ball.level];

		LeanTween.value (gameObject, (value) => {

			icon.transform.localScale = new Vector3 (value, value, value);

		}, 0, 0.7f, GPM.Instance.ballIconShowDuration).setEase (GPM.Instance.ballIconShowEasing);
	}
}

