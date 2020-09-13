using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;

public class BombObject : Floating, IHoldable, ISpellObject
{
    [Header("Parameters")]
    public BombSpell spell;

    private GameObject icon;
    private float iconScaleCached;

    private GameObject cloudAnimation;

    [Header("Info")]
//	public bool isActive = true;
	Vector3 holdingTargetPosition;
	Vector3 lastHoldingVelocity;

	TapGesture tapGesture;

    public bool exploded = false;

	public List<BombShockwave> shockwaves = new List<BombShockwave>();

	protected override void Awake ()
	{
		base.Awake ();

		// Get Icon and cache scale
		icon = transform.GetChild (0).gameObject;
		iconScaleCached = icon.transform.localScale.x;

		// Get cloud animation
		cloudAnimation = transform.GetChild (1).gameObject;
	}

	void Start()
	{
		tapGesture = GetComponent<TapGesture> ();
		tapGesture.Tapped += (object sender, System.EventArgs e) => {
			Explode();
		};

//		AssignVelocityRandomValue ();

//		icon.transform.localScale = Vector3.zero;
	}

	protected override void FixedUpdate()
	{
		if(isGettingHold)
		{
			HoldingLoop ();
		}
		else if(isMoving)
		{
			AdjustFreeMovement ();
		}

		KeepInGameArea ();
	}

	void HoldingLoop ()
	{
		// Move bomb towards holding target location
		Vector3 newPos = Vector3.Lerp (transform.position, holdingTargetPosition, Time.deltaTime * GPM.Instance.ballHoldResponseMultiplier);
		Trace.Msg("bomb newPos = " + newPos);
		_rigidbody.MovePosition (newPos);
	}

	void Explode ()
	{
        if (exploded)
            return;

        // Level up sound
        if(GetComponent<LevelUpBombObject>() == null)
            AudioManager2.Instance.Play (SoundsManager.Instance.bombActivate);
        else
            AudioManager2.Instance.Play (SoundsManager.Instance.levelupActivate);
            
        
        exploded = true;

//		isActive = false;
		PlayCloudAnimation ();

		// Stop Timer
        GameManager.Instance.PauseGame(true,false,false);
        Time.timeScale = .5f;
//        Time.timeScale
//			GameManager.Instance.PauseTimer (true);

		// Hide bomb
		GetComponent<SpriteRenderer>().enabled = false;
		icon.SetActive (false);

		// Add shockwave to list
		GameObject shockwave = SpawnManager.Instance.SpawnSpellObject (spell.shockwaveObjectPrefab, transform.position);
		shockwave.GetComponent<BombShockwave> ().Register (this);

		StartCoroutine (CheckShocwaveLoop ());
	}

	private void PlayCloudAnimation()
	{
		// Play cloud animation
		cloudAnimation.SetActive (true);
		Animator animator = cloudAnimation.GetComponent<Animator> ();
		Utility.Instance.WaitTillAnimationTime (animator, 1, () => {
            
            if(cloudAnimation != null)
			    cloudAnimation.SetActive(false);
		});
	}

	private IEnumerator CheckShocwaveLoop()
	{
		while(shockwaves.Count > 0)
		{
			Trace.Msg("Active Shocwave Count = " + shockwaves.Count);
			yield return true;
		}
		
		Trace.Msg ("Shockwaves ended!");

		// Resume Game
		GameManager.Instance.PauseGame (false);
//		GameManager.Instance.PauseTimer (false);

		SpawnManager.Instance.DeSpawnSpellObject (gameObject);
        Time.timeScale = 1f;

	}

	public void InitializeWithoutMerge()
	{
		InitializeIcon ();
	}

	public virtual void InitializeWithMerge(float creationAngle)
	{
		SpriteRenderer graphicsRenderer = GetComponent<SpriteRenderer> ();
		graphicsRenderer.color = new Color (0, 0, 0, 0);

		// Start ball merge animation

		// TODO: Pooling
		// Instantiate ( we need pooling here probably ) animation object and start animation
		GameObject ballMerge = Instantiate (AssetManager.Instance.ballMergePrefab, transform.position, Quaternion.identity) as GameObject; 
		ballMerge.transform.parent = transform;

		ballMerge.GetComponent<BallMerge> ().Initialize (3, (float)creationAngle);

		// Wait till animation time
		Animator animator = ballMerge.GetComponent<Animator> ();

		Utility.Instance.WaitTillAnimationTime (animator, GPM.Instance.ballMergeAnimationCrossTime, () => {

			InitializeIcon();

			// Tween alpha to 1
			LeanTween.value (gameObject, (value) => {

				graphicsRenderer.color = value;

			}, new Color(1,1,1, 0), Color.white, GPM.Instance.ballMergeFadeInDuration).setOnComplete(()=>{

				Destroy(ballMerge);

			});
		});
	}

	void InitializeIcon ()
	{
//		SpriteRenderer graphicsRenderer = icon.GetComponent<SpriteRenderer> ();
//		graphicsRenderer.sprite = AssetManager.Instance.ballIconSpritesByLevel[ball.level];

		icon.transform.localScale = Vector3.zero;
		LeanTween.value (gameObject, (value) => {

			icon.transform.localScale = new Vector3 (value, value, value);

		}, 0, iconScaleCached, GPM.Instance.ballIconShowDuration).setEase (GPM.Instance.ballIconShowEasing);
	}

	public void SetSortingLayer(string layerName)
	{
		GetComponent<SpriteRenderer> ().sortingLayerName = layerName;

		icon.GetComponent<SpriteRenderer> ().sortingLayerName = layerName;
	}

	#region IHoldable implementation

	public void StartHolding(Vector2 position)
	{
//		lastInteractionTime = Time.time;

		isGettingHold = true;
		_rigidbody.velocity = Vector3.zero;
		holdingTargetPosition = transform.position;

        SetPhysicalStateOnHold();

	}

	public void StopHolding(Vector2 velocity)
	{
		isGettingHold = false;

		// Move ball as the same direction as last finger movement
		if(velocity.magnitude > GPM.Instance.ballVelocityMagnitudeLimit)
		{
			velocity = velocity.normalized * GPM.Instance.ballVelocityMagnitudeLimit;
		}
		_rigidbody.velocity = velocity;

        Explode();

	}

	public void HoldTransformed(Vector2 deltaPosition)
	{
		holdingTargetPosition += (Vector3)deltaPosition;

		if(_collider != null)
			holdingTargetPosition = Utility.Instance.ClampPositionToBounds (holdingTargetPosition, GameManager.Instance.GameAreaBounds, _collider.radius);
	}

	public bool IsHoldable()
	{
        return !exploded;
	}

	#endregion

	#region ISpellObject implementation

	public Spell GetSpell ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion



}

