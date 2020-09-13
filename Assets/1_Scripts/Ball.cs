using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class Ball : Floating, IHoldable
{
	public delegate void BallEvent();

	public event BallEvent BallDestroyed;
	
    [Header("Ball - Parameters")]
    BallGraphics graphics;
    [SerializeField]ParticleSystem _particleSystem;
//    [SerializeField]PointEffector2D _pointeffector;

    [Header("Ball - Info")]
    public int level = 0;
    [SerializeField]private bool isActive = true;
    public bool IsActive{
        get{
            return isActive;
        }
    }
    [SerializeField]private bool pinchStarted = false ;

    float lastInteractionTime = 0f;
    public float LastInteractionTime {
        get{
            return lastInteractionTime;
        }
    }

    // Holding 
//  private bool isGettingHold = false;
    Vector3 holdingTargetPosition = Vector3.zero;
    Holder holder;

    public float Radius{
        get {
            return _collider.radius * transform.localScale.x;
        }
    }

//  public obs_BallPair ballPair;


	protected override void Awake ()
	{
		base.Awake ();

		graphics = GetComponentInChildren<BallGraphics> ();

        Activate();
	}

	// void Update()
	// {
//		if(isGettingHold)
//		{
//			HoldingLoop ();
//		}
//        DebugConsole.Log()
	// }

	protected override void FixedUpdate()
	{
		if(isGettingHold)
		{
			HoldingLoop ();
		}
		else if(isMoving && isActive)
		{
			AdjustFreeMovement ();
		}

		KeepInGameArea ();
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
//        if(coll.gameObject != gameObject)
        if(coll.name == "Effector")
            _triggerCount++;

        if(CanPinch())
            CheckPinchWith (coll);

        RefreshEffectorMagnitude();

    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if(CanPinch())
            CheckPinchWith (coll);
    }

    void OnTriggerExit2D (Collider2D coll)
    {
//        if(coll.gameObject != gameObject)
//        if(coll.isTrigger)
        if(coll.name == "Effector")
            _triggerCount--;
        RefreshEffectorMagnitude();
    }

	void OnCollisionStay2D(Collision2D coll) 
	{
		if(CanPinch())
			CheckPinchWith (coll);
	}

	// TODO: Bunlara birlikte bkamali
	void OnCollisionEnter2D(Collision2D coll) 
	{
//        _triggerCount++;
        HitFeedback();

		if(CanPinch())
			CheckPinchWith (coll);
        
	}

    // void OnCollisionExit2D(Collision2D coll) 
    // {
//        _triggerCount--;
    // }

    public bool CanPinch()
    {
		// Can't pinch if any blackhole object exists
		if(GameManager.Instance.IsSpellObjectExists<BlackholeObject>()) {
			return false;
		}
		
        bool pinchIsValid = 
            lastInteractionTime + GPM.Instance.ballPairLifeAfterRelease > Time.time &&
            !pinchStarted &&
            IsActive;


        //      Trace.Msg ("Checking can pinch of " + name);
        //      Trace.Msg ("Pinch is " + pinchIsValid);
        //      Trace.Msg (Time.time - lastInteractionTime);

        return pinchIsValid && GameManager.Instance.GameplayState == GameplayState.Playing;
    }

	void CheckPinchWith(Collision2D coll)
	{
		Ball otherBall = coll.gameObject.GetComponent<Ball> ();

//		Trace.Msg ("Checking pinch of ball " + name + " and " + otherBall.name + ".");

		if (otherBall != null && otherBall.level == level && otherBall.CanPinch())
		{
//			Trace.Msg ("Pinching!");
			GameControlManager.Instance.Pinch (new Ball[]{ this, otherBall });
		}
	}

    void CheckPinchWith(Collider2D coll)
    {
        Ball otherBall = coll.gameObject.GetComponent<Ball> ();

        //      Trace.Msg ("Checking pinch of ball " + name + " and " + otherBall.name + ".");

        if (otherBall != null && otherBall.level == level && otherBall.CanPinch())
        {
            //          Trace.Msg ("Pinching!");
            GameControlManager.Instance.Pinch (new Ball[]{ this, otherBall });
        }
    }

    void HitFeedback()
    {

        gameObject.transform.localScale = GPM.Instance.bhr1 * Vector3.one;
        LeanTween.scale(gameObject, Vector3.one, GPM.Instance.bhr2).setEase(GPM.Instance.bhr3);
        _particleSystem.Emit(4);
    }

	// Initialize Methods

	public void Initialize(int? ballLevel, Vector3? position, bool isNew, float? creationAngle) 
	{
		// Random level if null
		if(ballLevel == null)
		{
			AudioManager2.Instance.Play (SoundsManager.Instance.ballSpawns, .2f);

			ballLevel = GPM.Instance.RandomBallLevel;
		}

		// Random pos if null
		if(position == null)
		{
			position = Vector3.zero;
			bool isValid = false;

			while(!isValid)
			{
                position = SpawnManager.Instance.GetRandomPositionInGameAreaFiltered (_collider.radius);
//                position = SpawnManager.Instance.GetRandomPositionInGameArea(_collider.radius);

//				SpawnManager.Instance

				// FILL: Check if position valid - is not valid if on top of other balls or spells etc
				isValid = true;
			}		
		}

		level = (int)ballLevel;

		graphics.Initialize (this);

		SpawnSelf ((Vector3)position, isNew, creationAngle); 
		AssignVelocityRandomValue();

		// Editor
		transform.name = "Ball Level " + level + Random.Range(0, 100).ToString();
	}
		
	void SpawnSelf(Vector3 position, bool isNew, float? creationAngle)
	{
		// Position
		transform.position = position;

		// Collider
		GetComponent<CircleCollider2D> ().radius = GPM.Instance.BallColliderRadiuses [level];

		// Scale up on spawn
		Vector2 finalSize = GPM.Instance.ballNormalSize;
			
//		float maxBallMultiplier = GPM.Instance.ballLevelledMaxSizeFactor;
//		float sizeIncrease = ((maxBallMultiplier - 1) / (GPM.Instance.ballMaxLevel) ) * level;
//		finalSize *= (1 + sizeIncrease);

		if(isNew)
		{
			//			_rigidbody.isKinematic = true;
			graphics.InitializeIcon();
			transform.localScale = GPM.Instance.ballDeadSize;
			LeanTween.scale(gameObject, finalSize, GPM.Instance.ballShowDuration).setEase(GPM.Instance.ballShowEasing).setOnComplete(()=> {


			});
		}
		else
		{
//            isActive = false;

			SpriteRenderer graphicsRenderer = graphics.GetComponent<SpriteRenderer> ();
			graphicsRenderer.color = new Color (0, 0, 0, 0);

			// Start ball merge animation

			// TODO: Pooling
			// Instantiate ( we need pooling here probably ) animation object and start animation
			GameObject ballMerge = Instantiate (AssetManager.Instance.ballMergePrefab, transform.position, Quaternion.identity) as GameObject; 
			ballMerge.transform.parent = transform;
//			Debug.LogError ("ball merge check");

			ballMerge.GetComponent<BallMerge> ().Initialize (level - 1, (float)creationAngle);

			// Wait till animation time
			Animator animator = ballMerge.GetComponent<Animator> ();

			Utility.Instance.WaitTillAnimationTime (animator, GPM.Instance.ballMergeAnimationCrossTime, () => {

				// Graphics can be null if this ball is destroyed meanwhile
				if(this == null && graphics == null) 
					return;
				
				graphics.InitializeIcon();

				// Tween alpha to 1
				LeanTween.value (gameObject, (value) => {
					
					graphicsRenderer.color = value;

				}, new Color(1,1,1, 0), Color.white, GPM.Instance.ballMergeFadeInDuration).setOnComplete(()=>{

					Destroy(ballMerge);

				});
			});
		}

		
	}

	void HoldingLoop()
	{
		// Move ball towards holding target location
		lastInteractionTime = Time.time;
//		transform.position = Vector3.Lerp (transform.position, holdingTargetPosition, Time.deltaTime * GPM.Instance.ballHoldResponseMultiplier);
		Vector3 newPos = Vector3.Lerp (transform.position, holdingTargetPosition, Time.deltaTime * GPM.Instance.ballHoldResponseMultiplier);
		_rigidbody.MovePosition (newPos);
	}

	// API
	public void StartPinch()
	{
		pinchStarted = true;
		isActive = false;
		isMoving = false;
		isGettingHold = false;

		GameControlManager.Instance.HoldableRegistration (GetComponent<IHoldable>(), false);

		if(holder != null)
			holder.BallPinched ();
	}
		
	public void Activate()
	{
        isActive = true;
        _collider.isTrigger = false;
        gameObject.layer = LayerMask.NameToLayer("BallActive");
        _pointeffector.gameObject.layer = LayerMask.NameToLayer("BallActive");
	}
        
    public void Deactivate()
    {
        isActive = false;
        _collider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("BallInactive");
        _pointeffector.gameObject.layer = LayerMask.NameToLayer("BallInactive");
    }

	public void LevelUp()
	{
        Trace.Msg("Levelling up ball!");
		if (level >= GPM.Instance.ballMaxLevel)
			return;

		level++;
		graphics.Initialize (this);
		graphics.InitializeIcon ();
	}

	public void SetKinematic(bool isTrue)
	{
		_rigidbody.isKinematic = isTrue;
	}

	public void SetSortingLayer(string layerName)
	{
		graphics.GetComponent<SpriteRenderer> ().sortingLayerName = layerName;
		graphics.icon.GetComponent<SpriteRenderer> ().sortingLayerName = layerName;
	}

	// #region IHoldable implementation

	public void StartHolding(Vector2 holdingPosition)
	{
		isGettingHold = true;
		_rigidbody.velocity = Vector3.zero;

		// If ball is far away, move it to finger
		// Else just move it by delta position

        float distance = ((Vector3)holdingPosition - transform.position).sqrMagnitude;

		if(distance < 1)
		{
			holdingTargetPosition = transform.position;
		}
		else
		{
			holdingTargetPosition = holdingPosition;
		}

//        _rigidbody.isKinematic = true;
        SetPhysicalStateOnHold();

	}


	public void StopHolding(Vector2 velocity)
	{
		isGettingHold = false;

		// Paired release velocity
		Ball targetBall = GameControlManager.Instance.GetTargetForReleasedBall(this);

		if(targetBall != null)
		{
			Vector3 vel = (targetBall.transform.position - transform.position).normalized * GPM.Instance.ballPairedReleaseVelocityMagnitude;
			_rigidbody.velocity = vel;
		}
		// No target release velocity
		else
		{
			// Move ball as the same direction as last finger movement
			// Clamp velocity 
            // TODO: Rethink
			if(velocity.magnitude > 100)
			{
                Trace.Msg("Magnitude = " + velocity.magnitude);
				if(velocity.magnitude > GPM.Instance.ballVelocityMagnitudeLimit)
				{
					velocity = velocity.normalized * GPM.Instance.ballVelocityMagnitudeLimit;
				}

                _rigidbody.velocity = velocity;
            }

		}
//        _rigidbody.isKinematic = false;

        if(IsActive)
        {
            StartCoroutine(WaitAfterHold());
        }

//        _collider.isTrigger = false;
	}


	public void HoldTransformed(Vector2 deltaPosition)
	{
//		lastInteractionTime = Time.time;
		holdingTargetPosition += (Vector3)deltaPosition;

		if(_collider != null)
			holdingTargetPosition = Utility.Instance.ClampPositionToBounds (holdingTargetPosition, GameManager.Instance.GameAreaBounds, _collider.radius);


//		Trace.Msg ("holdingTargetPosition: " + holdingTargetPosition);
	}

	public bool IsHoldable()
	{
		return !isGettingHold && isActive;
	}

	// #endregion

//    This method will be called when object FastInstantiated
    public void OnFastInstantiate()
    {
//        isActive = true;
        pinchStarted = false;
        isMoving = true;
        Activate();
        lastInteractionTime = 0;
        SetSortingLayer ("Ball");
    }

//    This method will be called when object FastDestroyed
    public void OnFastDestroy()
    {
//        name = "I'm cached...";
    }

	private void OnDestroy()
	{
		OnBallDestroyed();
	}

	protected virtual void OnBallDestroyed()
	{
		var handler = BallDestroyed;
		if (handler != null) handler();
	}
	
}

