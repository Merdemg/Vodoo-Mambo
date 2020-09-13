//#define USE_LOGS
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
	#region Singleton
	public static SpawnManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

	// TODO: Implement later in SpawnBall()
	public struct BallSpawnData
	{
		public float level;
		public Vector3 position;
		public bool isNew;
		public float angle;
	}

//    [SerializeField]FastPool fastPool;

	[Header("Prefabs")]
	public GameObject ballPrefab;
    public GameObject scoreTextPrefab;
	public GameObject comboTextPrefab;
	public GameObject powerUpBallPrefab;


    public List<IHoldable> holdables = new List<IHoldable> ();
    public List<Floating> floatings = new List<Floating> ();

	public Ball SpawnBall()
	{
		return SpawnBall(null, null, true, null);
	}

	public Ball SpawnBall(int? level = null, Vector3? position = null, bool isNew = true, float? creationAngle = null)
	{
//		GameObject ballGO = FastPoolManager.GetPool(ballPrefab).FastInstantiate();
		GameObject ballGO = Instantiate(ballPrefab, Vector2.zero, Quaternion.identity) as GameObject;

		Ball ball = ballGO.GetComponent<Ball>();

		ball.Initialize (level, position, isNew, creationAngle);

		GameManager.Instance.balls.Add (ball);
		holdables.Add ((IHoldable)ball);

        // Floating registration
        FloatingRegistration(ballGO, true);

		return ball;
	}

	public GameObject SpawnSpellObject(GameObject spellObjectPrefab, Vector3 position)
	{
        Trace.Msg("Spawning spell obj!");
		// Instantiate and register Spell Object at position
		GameObject spellObjectGO = Instantiate (spellObjectPrefab, position, Quaternion.identity) as GameObject;
		GameManager.Instance.spellObjects.Add (spellObjectGO);

		// If object holdable, add it to holdables
		IHoldable holdable = spellObjectGO.GetComponent<IHoldable> ();

		if(holdable != null)
		{
			holdables.Add (spellObjectGO.GetComponent<IHoldable>());
		}

        FloatingRegistration(spellObjectGO, true);

		return spellObjectGO;
	}

	public void DeSpawnBall(Ball ball)
	{
        Trace.Msg("DeSpawnBall callde on: " + ball.name);

        holdables.Remove (ball.GetComponent<IHoldable> ());
        GameManager.Instance.balls.Remove (ball);
        FloatingRegistration(ball.gameObject, false);

//        FastPoolManager.GetPool(ballPrefab).FastDestroy(ball.gameObject);
        Destroy (ball.gameObject);
    }

    public void DeSpawnSpellObject(GameObject spellObject)
    {

        Trace.Msg("DeSpawnSpellObject called on: " + spellObject.name);

        IHoldable holdable = spellObject.GetComponent<IHoldable> ();

        if(holdable != null)
        {
            holdables.Remove (holdable);
        }

        FloatingRegistration(spellObject.gameObject, false);
        GameManager.Instance.spellObjects.Remove (spellObject);

        Destroy (spellObject.gameObject);
	}
		
	public GameObject SpawnScoreTextAtPosition(Vector3 position, int score)
	{
        var scoreTextGameObject = FastPoolManager.GetPool(scoreTextPrefab).FastInstantiate();

		scoreTextGameObject.transform.SetParent(UIManager.Instance.worldSpaceCanvas.transform, false);
        scoreTextGameObject.GetComponent<ScoreText> ().Initialize (position, score, false);
		return scoreTextGameObject;
    }

    public void DeSpawnScoreText(GameObject obj)
    {
        FastPoolManager.GetPool(scoreTextPrefab).FastDestroy(obj);
    }

    /*public GameObject SpawnComboTextAtPosition(Vector3 position, int combo)
    {
        // FIXME: debugging performance
        return null;

        GameObject scoreTextGameObject = Instantiate(comboTextPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        scoreTextGameObject.transform.SetParent(UIManager.Instance.worldSpaceCanvas.transform, false);
        scoreTextGameObject.GetComponent<ScoreText> ().Initialize (position, combo, true);
        return scoreTextGameObject;
    }*/

	public GameObject SpawnPowerUpBall(PowerUp powerUp)
	{
		Vector3 position = GetRandomPositionInGameArea (.4f);
		GameObject powerUpBallGameObject = Instantiate (powerUpBallPrefab, position, Quaternion.identity) as GameObject;
		PowerUpBall powerUpBall = powerUpBallGameObject.GetComponent<PowerUpBall> ();
		powerUpBall.Initialize (powerUp);

		return powerUpBallGameObject;
	}

    void FloatingRegistration(GameObject go, bool isAdd)
    {
        // Floating registration
        Floating floating = go.GetComponent<Floating>();
        if(floating)
        {
        }

        if(isAdd)
        {
            floatings.Add(floating);
        }
        else
        {
            floatings.Remove(floating);
        }
    }

	#region Game Area Methods

	Vector3 screenBottomLeft {
		get {
			return GameManager.Instance.GameAreaBounds.min;
		}
	}

	Vector3 screenTopRight{
		get {
			return GameManager.Instance.GameAreaBounds.max;
		}
	}

	/// <summary>
	/// Gets the random position within screen boundaries.
	/// </summary>
	/// <returns>The random position within screen boundaries.</returns>
	/// <param name="edgeOffset">Edge offset to get the fit the object in screen. In case of ball, radius of the ball.</param>
	public Vector3 GetRandomPositionInGameArea(float edgeOffset)
	{
		float offsetTreshold = .2f;
		//		return new Vector3(
		//			Random.Range(screenBottomLeft.x + 2f, screenTopRight.x - 2f),
		//			Random.Range(screenBottomLeft.y + 2f, screenTopRight.y - 2f), 
		//			0f);
		return new Vector3(
			Random.Range(screenBottomLeft.x + (edgeOffset + offsetTreshold), screenTopRight.x - (edgeOffset + offsetTreshold)),
			Random.Range(screenBottomLeft.y + (edgeOffset + offsetTreshold), screenTopRight.y - (edgeOffset + offsetTreshold)),
			0f);
	}

	/// <summary>
	/// Gets the random position within screen boundaries, with no overlapping ball or spell objects
	/// </summary>
	/// <returns>The random position within screen boundaries.</returns>
	/// <param name="edgeOffset">Edge offset to get the fit the object in screen. In case of ball, radius of the ball.</param>
	public Vector3 GetRandomPositionInGameAreaFiltered(float radius)
	{
        Trace.Msg (("GetRandomPositionInGameAreaFiltered").Colored(Colors.red));

		Vector3 randomPosition = Vector3.zero;

		bool isOverlapping = true;
		int maximumCheckCount = 10;
		int checkCount = 0;
		while(isOverlapping && checkCount <= maximumCheckCount)
		{
			checkCount++;
			// Get random position in game area
			randomPosition = GetRandomPositionInGameArea (radius);
			isOverlapping = false;

			// TODO: buralari bi duzenle, hacklenmis gibi

			// Check all balls
			List<Ball> ballsToCheck = new List<Ball> (GameManager.Instance.balls.ToArray());
			List<GameObject> spellObjectsToCheck = new List<GameObject> (GameManager.Instance.spellObjects.ToArray());

//			foreach (var holdable in holdables) 
			foreach (var ball in ballsToCheck) 
			{
				CircleCollider2D holdableCollider = ball.GetComponent<CircleCollider2D>();

				if(holdableCollider != null)
				{
					//				CircleCollider2D holdableCollider = ((MonoBehaviour)holdable).GetComponent<CircleCollider2D>();
					if((holdableCollider.transform.position - randomPosition).sqrMagnitude <= (holdableCollider.radius + radius) * (holdableCollider.radius + radius))
					{
						isOverlapping = true;
						Trace.Msg (("Retrying GetRandomPositionInGameAreaFiltered for balls").Colored(Colors.lightblue));
						break;
					}
				}
			}

			// Check all spellObjects
			foreach (var spellObject in spellObjectsToCheck) 
			{
				CircleCollider2D holdableCollider = spellObject.GetComponent<CircleCollider2D>();

				if(holdableCollider != null)
				{
					//				CircleCollider2D holdableCollider = ((MonoBehaviour)holdable).GetComponent<CircleCollider2D>();
					if((holdableCollider.transform.position - randomPosition).sqrMagnitude <= (holdableCollider.radius + radius) * (holdableCollider.radius + radius))
					{
						isOverlapping = true;
						Trace.Msg (("Retrying GetRandomPositionInGameAreaFiltered for spells").Colored(Colors.lightblue));
						break;
					}
				}
			}

			// Try again if overlapping
			if(isOverlapping)
				continue;
		}

        Trace.Msg(("Check Count: " + checkCount).Colored(Colors.green));
		return randomPosition;
	}

	public Vector3 GetRandomPositionOnEdge(Direction direction)
	{
		float minX = screenBottomLeft.x;
		float minY = screenBottomLeft.y;
		float maxX = screenTopRight.x;
		float maxY = screenTopRight.y;

		switch (direction)
		{
		case Direction.North:
			//return new Vector3(Random.Range(minX, maxX), maxY + 0.5f, 0f);
			return new Vector3(Random.Range(minX, maxX), maxY, 0f);

		case Direction.East:
			//return new Vector3(maxX + 0.5f, Random.Range(minY, maxY), 0f);
			return new Vector3(maxX, Random.Range(minY, maxY), 0f);

		case Direction.South:
			//return new Vector3(Random.Range(minX, maxX), minY - 0.5f, 0f);
			return new Vector3(Random.Range(minX, maxX), minY, 0f);

		case Direction.West:
			//return new Vector3(minX - 0.5f, Random.Range(minY, maxY), 0f);
			return new Vector3(minX, Random.Range(minY, maxY), 0f);

		default:
			return new Vector3();
		}
	}

	#endregion
}

