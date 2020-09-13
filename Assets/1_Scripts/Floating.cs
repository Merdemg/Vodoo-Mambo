using UnityEngine;
using System.Collections;

public class Floating : MonoBehaviour
{
    [Header("Floating - Parameters")]
    protected Rigidbody2D _rigidbody;
    protected CircleCollider2D _collider;
    [SerializeField]protected PointEffector2D _pointeffector;
    [SerializeField]protected int _triggerCount = 0;
    protected bool isGettingHold;

    [Header("Floating - Info")]
    [SerializeField]protected bool isMoving = true;

	virtual protected void Awake ()
	{
		_rigidbody = GetComponent<Rigidbody2D> ();
		_collider = GetComponent<CircleCollider2D> ();
        _pointeffector = GetComponentInChildren<PointEffector2D>();
	}


	protected virtual void FixedUpdate()
	{
		if(isMoving)
		{
			AdjustFreeMovement ();
		}

		KeepInGameArea ();
	}


	// If velocity smaller then normal value, set it to normal
	protected void AdjustFreeMovement()
	{
        if (_rigidbody.velocity.magnitude < GameManager.Instance.CurrentGameBallNormalVelocity)
		{
			// If almost stopping, go random direction
			if (_rigidbody.velocity.magnitude < 0.01f)
			{
				AssignVelocityRandomValue();
			}
			else
			{
                NormalizeSpeed();
//				_rigidbody.velocity = _rigidbody.velocity.normalized * GPM.Instance.ballNormalVelocity;
			}
		}
	}

	protected void KeepInGameArea() {

		Direction dir = Direction.East;

		float offset = _collider.radius;

		// Left
		if(transform.position.x + offset > GameManager.Instance.GameAreaBounds.max.x)
		{
			transform.position = new Vector3 (GameManager.Instance.GameAreaBounds.max.x - offset, transform.position.y, 0);
			_rigidbody.velocity = new Vector2 (-_rigidbody.velocity.x, _rigidbody.velocity.y);
//			dir = Direction.East;
		} 
		// Right
		else if (transform.position.x - offset < GameManager.Instance.GameAreaBounds.min.x)
		{
			transform.position = new Vector3 (GameManager.Instance.GameAreaBounds.min.x + offset, transform.position.y, 0);
			_rigidbody.velocity = new Vector2 (-_rigidbody.velocity.x, _rigidbody.velocity.y);
//			dir = Direction.West;
		}
		// Top
		else if (transform.position.y + offset > GameManager.Instance.GameAreaBounds.max.y)
		{
			transform.position = new Vector3 (transform.position.x, GameManager.Instance.GameAreaBounds.max.y - offset,  0);
			_rigidbody.velocity = new Vector2 (_rigidbody.velocity.x, -_rigidbody.velocity.y);
//			dir = Direction.North;
		}
		// Bottom
		else if (transform.position.y - offset < GameManager.Instance.GameAreaBounds.min.y)
		{
			transform.position = new Vector3 (transform.position.x, GameManager.Instance.GameAreaBounds.min.y + offset,  0);
			_rigidbody.velocity = new Vector2 (_rigidbody.velocity.x, -_rigidbody.velocity.y);
//			dir = Direction.South;
		}

	}

	protected void AssignVelocityRandomValue()
	{
        if (_rigidbody.velocity.magnitude < GameManager.Instance.CurrentGameBallNormalVelocity)
		{
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
//			_rigidbody.velocity =  .normalized * GPM.Instance.ballNormalVelocity;
            NormalizeSpeed(randomDirection);
		}
	}

	protected void AssignVelocityRandomValueByPositionDirection(Direction initialPisitionDirection)
	{
		switch (initialPisitionDirection)
		{
            case Direction.North:
                NormalizeSpeed(new Vector2(Random.Range(-0.5f, 0.5f), -1f));
//                _rigidbody.velocity = NormalizeSpeed (new Vector2(Random.Range(-0.5f, 0.5f), -1f)).normalized * GPM.Instance.ballNormalVelocity;
			break;

		case Direction.East:
                NormalizeSpeed( new Vector2(-1f, Random.Range(-0.5f, 0.5f)) );
//			_rigidbody.velocity =  new Vector2(-1f, Random.Range(-0.5f, 0.5f)).normalized * GPM.Instance.ballNormalVelocity;
			break;

		case Direction.South:
                NormalizeSpeed(new Vector2(Random.Range(-0.5f, 0.5f), 1f));
//			_rigidbody.velocity = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized * GPM.Instance.ballNormalVelocity;
			break;

		case Direction.West:
                NormalizeSpeed(new Vector2(1f, Random.Range(-0.5f, 0.5f)));
//			_rigidbody.velocity = new Vector2(1f, Random.Range(-0.5f, 0.5f)).normalized * GPM.Instance.ballNormalVelocity;
			break;
		}
	}

	public void StopMoving()
	{
		isMoving = false;
//		_rigidbody.drag = 100f;
        if(_rigidbody)
		    _rigidbody.velocity = Vector2.zero;
	}

	public void StartMoving()
	{
		isMoving = true;
//		_rigidbody.drag = 4f;
	}

    // TODO: Explain
    protected IEnumerator WaitAfterHold()
    {
        while(_triggerCount > 0)
        {
            if (isGettingHold )
            {
                yield break;
            }

            yield return true;
        }

        if(_pointeffector != null)
            _pointeffector.forceMagnitude = GPM.Instance.ballNormalEffectorMagnitude;

        if(_collider != null)
            _collider.isTrigger = false;
    }

    protected void SetPhysicalStateOnHold()
    {
        RefreshEffectorMagnitude();
//        _pointeffector.forceMagnitude = GPM.Instance.ballHoldingEffectorMagnitude;
        _collider.isTrigger = true;
    }

    protected void NormalizeSpeed(Vector2? direction = null)
    {
        if(direction == null)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * GameManager.Instance.CurrentGameBallNormalVelocity;
            
        }
        else
        {
            _rigidbody.velocity = ((Vector2)direction).normalized * GameManager.Instance.CurrentGameBallNormalVelocity;
            
        }

    }

    protected void RefreshEffectorMagnitude()
    {
        if(isGettingHold)
            _pointeffector.forceMagnitude = GPM.Instance.ballHoldingEffectorMagnitude * _triggerCount;
        else
            _pointeffector.forceMagnitude = GPM.Instance.ballNormalEffectorMagnitude * _triggerCount;

    }
}

