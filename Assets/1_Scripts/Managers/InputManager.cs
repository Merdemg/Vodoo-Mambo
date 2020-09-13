using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript;

public class InputManager : MonoBehaviour {

	#region Singleton
	public static InputManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion
    [Header("Parameters")]
    [SerializeField]int maxHolderCount = 2;
    [Header("Info")]
	public List<TransformGesture> activeGestures;
	public List<GameObject> holdingObjects;

//	void Update()
//	{
//		var i = 0;
//		foreach (var gesture in activeGestures) 
//		{
////			Trace.Msg ("Gesture " + i + "position is " + gesture.ScreenPosition);
//			string pS = "Gesture velocity = " + gesture.DeltaPosition / Time.deltaTime;
//			string pS2 = "Gesture deltaposition = " + gesture.DeltaPosition;
//			Trace.Msg (pS);
//			Trace.Msg (pS2);
//			i++;
//		}

//	}

//	public void RegisterGesture(TransformGesture gesture)
//	{
//		activeGestures.Add (gesture);
//	}
//
//	public void DeRegisterGesture(TransformGesture gesture)
//	{
//		activeGestures.Remove (gesture);
//	}
//
//	public void RegisterBall(GameObject ball)
//	{
//		holdingObjects.Add (ball);
//	}
//
//	public void DeRegisterBall(GameObject ball)
//	{
//		holdingObjects.Remove (ball);
//	}

	///////


	private Camera _mainCamera;
	private IDictionary<int, Holder> holders;

	void Start()
	{
		holders = new Dictionary<int, Holder>();

		// Register Events
		if (TouchManager.Instance != null)
		{
			TouchManager.Instance.TouchesBegan += TouchesBeganHandler;
			TouchManager.Instance.TouchesCancelled += TouchesEndedHandler;
			TouchManager.Instance.TouchesEnded += TouchesEndedHandler;
			TouchManager.Instance.TouchesMoved += TouchesMovedHandler;
		}
    }

    void Update()
    {
        DebugConsole.Log("// Holders");
        DebugConsole.Log("Count: " + holders.Count.ToString());
        foreach(var holder in holders)
        {
//            if(holder.Key != null)
//            {
            DebugConsole.Log("Key: " + holder.Key.ToString());
            DebugConsole.Log("Value: " + holder.Value.ToString());
            DebugConsole.Log("Value: " + (holder.Value.holdingObject as MonoBehaviour).name);
//            }
//            else
//            {
//                DebugConsole.Log("holder is null", "error");
//            }
        }
    }


	private void TouchesBeganHandler(object sender, TouchEventArgs e)
	{
		if(GameManager.Instance.GameplayState != GameplayState.Playing)
		{
			return;
		}

		IList<TouchPoint> touches = e.Touches;

		for (int i = 0; i < touches.Count; i++)
		{
            // Return if current touches > Max Touches

            if (holders.Count >= maxHolderCount)
                return;

			TouchPoint touch = touches[i];

			Vector3 spawnPosition = ConvertScreenToWorldPosition(touch.Position);

			SpawnHolder(touch.Id, spawnPosition);
		}
	}

//	private void TouchesCancelledHandler(object sender, TouchEventArgs e)
//	{
//		if(GameManager.Instance.GameplayState != GameplayState.Playing)
//		{
//			return;
//		}
//		IList<TouchPoint> touches = e.Touches;
//
//		for (int i = 0; i < touches.Count; i++)
//		{
//			TouchPoint touch = touches[i];
//			Vector3 velocity = (touch.Position - touch.PreviousPosition) / Time.deltaTime;
//
//			DestroyMagnet(touch.Id, velocity);
//		}
//	}

	private void TouchesEndedHandler(object sender, TouchEventArgs e)
	{
//		if(GameManager.Instance.GameplayState != GameplayState.Playing)
//		{
//			return;
//		}

		IList<TouchPoint> touches = e.Touches;

		for (int i = 0; i < touches.Count; i++)
		{
			TouchPoint touch = touches[i];
			Vector3 velocity = (touch.Position - touch.PreviousPosition) / Time.deltaTime;

			DestroyHoldable(touch.Id, velocity);
		}
	}

	private void TouchesMovedHandler(object sender, TouchEventArgs e)
	{
		IList<TouchPoint> touches = e.Touches;

		for (int i = 0; i < touches.Count; i++)
		{
			TouchPoint touch = touches[i];

			Vector3 movePosition = ConvertScreenToWorldPosition(touch.Position);
//			Vector3 moveDeltaPosition = ConvertScreenToWorldPosition((touch.Position - touch.PreviousPosition)) ;
			Vector3 moveDeltaPosition = movePosition - ConvertScreenToWorldPosition(touch.PreviousPosition);
//			Vector3 velocity = (touch.Position - touch.PreviousPosition) / Time.deltaTime;
			Vector3 velocity = moveDeltaPosition / Time.deltaTime;

//			Trace.Msg ("movePosition: " + movePosition);
//			Trace.Msg ("moveDeltaPosition: " + moveDeltaPosition);

			Holder holder;
			holders.TryGetValue(touch.Id, out holder);

			if (holder != null)
			{
//				holder.MoveToPosition (movePosition, velocity);
				holder.MoveToDeltaPosition (moveDeltaPosition, velocity);
			}
		}
	}

	private void SpawnHolder(int touchId, Vector2 position)
	{
		Holder holder = new GameObject ("holder id: " + touchId).AddComponent<Holder> ();
		holder.transform.position = position;

		holders.Add (touchId, holder);
	}


	private void DestroyHoldable(int id, Vector2 velocity)
	{
		Holder holder;
		holders.TryGetValue(id, out holder);

		if (holder != null)
		{
			holder.StopHolding(velocity);
			holders.Remove(id);
			Destroy(holder);
		}
	}

	private Vector3 ConvertScreenToWorldPosition(Vector2 screenPosition)
	{
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
		worldPosition.z = 0f;
		return worldPosition;
	}

    public void DestroyHolders()
    {
        foreach (var keyval in holders)
        {
            Destroy(keyval.Value);

        }
        holders = new Dictionary<int, Holder>();
           
    }
}
