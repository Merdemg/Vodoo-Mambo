using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

	public class Utility : MonoBehaviour
	{

	#region Singleton
	public static Utility Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

	public void WaitTillAnimationTime(Animator animation, float time, Action callback)
	{
		StartCoroutine (WaitTillAnimationTimeRoutine (animation, time, callback));
	}

	private IEnumerator WaitTillAnimationTimeRoutine(Animator anim, float time, Action callback)
	{
		while(anim != null && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < time)
		{
//			Trace.Msg (anim.GetCurrentAnimatorStateInfo (0).normalizedTime);
			yield return false;
		}
		callback ();
	}

	public float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
	{
		Vector2 diference = vec2 - vec1;
		float sign = (vec2.y < vec1.y)? -1.0f : 1.0f;
		return Vector2.Angle(Vector2.right, diference) * sign;
	}

	public Vector2 ClampPositionToBounds(Vector2 position, Bounds bounds, float offset = 0)
	{
		float newX = position.x;
		float newY = position.y;

		if(position.x > bounds.max.x - offset)
		{
			newX = bounds.max.x - offset;
		}
		else if(position.x < bounds.min.x+ offset)
		{
			newX = bounds.min.x + offset;
		}

		if(position.y > bounds.max.y - offset)
		{
			newY = bounds.max.y - offset;
		}
		else if (position.y < bounds.min.y+ offset)
		{
			newY = bounds.min.y + offset;
		}

		Vector2 newPosition = new Vector2 (newX, newY);

		return newPosition;
	}

    public void DoAfter(float seconds, Action callback)
    {
        if(callback != null)
            StartCoroutine (DoAfterRoutine (seconds, callback));
    }

	private IEnumerator DoAfterRoutine(float seconds, Action callback)
	{
		Trace.Msg ("Do After Started, seconds: "+seconds);

		float elapsed = 0;

		while(elapsed <= seconds)
		{
			elapsed += Time.deltaTime;
			yield return true;
//			Trace.Msg ("waiting" + elapsed.ToString());
		}

		if(callback != null)
			callback ();
	}

    public void WaitForCondition(Func<bool> condition, Action action)
    {
        StartCoroutine(WaitForConditionRoutine(condition, action));
    }

    private IEnumerator WaitForConditionRoutine(Func<bool> condition, Action action)
    {
        while(!condition())
        {
            Trace.Msg("Waiting for condition!");
            yield return 0;
        }

        Trace.Msg("Waiting for condition done! Doing action!");
        action();
    }


	//public static class Util
	//{
	public static T GetRandomEnum<T>()
	{
		System.Array array = System.Enum.GetValues(typeof(T));
		T value = (T)array.GetValue(UnityEngine.Random.Range(0, array.Length));
		return value;
	}


	public static K GetVariableByName<T, K>(T parent, string propertyName)
	{
		K temp = (K)typeof(T).GetField(propertyName).GetValue(parent);

		return temp;
	}

	public static void SetVariableByName<T, K>(T parent, string propertyName, K value)
	{
		typeof(T).GetField(propertyName).SetValue(parent, value);
	}

	//	double temp = (double)typeof(MyClass).GetProperty("PropertyName").GetValue(myClassInstance, null);

	public static int[] SecondsToDaysHoursMinutesSeconds(int seconds)
	{
		int numdays = (int)Mathf.Floor(seconds / 86400);
		int numhours = (int)Mathf.Floor((seconds % 86400) / 3600);
		int numminutes = (int)Mathf.Floor(((seconds % 86400) % 3600) / 60);
		int numseconds = (int)((seconds % 86400) % 3600) % 60;
		//		return numdays + " days " + numhours + " hours " + numminutes + " minutes " + numseconds + " seconds";
		return new int[]{ numdays, numhours, numminutes, numseconds };
	}
	//}
}
public static class StringLoggingExtensions
{
	/// <summary>
	/// Sets the color of the text according to the parameter value.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="color">Color.</param>
	public static string Colored(this string message, Colors color)
	{
		return string.Format("<color={0}>{1}</color>", color.ToString(), message);
	}

	/// <summary>
	/// Sets the color of the text according to the traditional HTML format parameter value.
	/// </summary>
	/// <param name="message">Message</param>
	/// <param name="color">Color</param>
	public static string Colored(this string message, Color color)
	{
		//		return string.Format("<color={0}>{1}</color>", ColorUtility.ToHtmlStringRGB(color), message);
		string htmlString ="#" + ColorUtility.ToHtmlStringRGB (color);
		return Colored (message, htmlString);
	}

	/// <summary>
	/// Sets the color of the text according to the traditional HTML format parameter value.
	/// </summary>
	/// <param name="message">Message</param>
	/// <param name="color">Color</param>
	public static string Colored(this string message, string colorCode)
	{
		return string.Format("<color={0}>{1}</color>", colorCode, message);
	}

	/// <summary>
	/// Sets the size of the text according to the parameter value, given in pixels.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="size">Size.</param>
	public static string Sized(this string message, int size)
	{
		return string.Format ("<size={0}>{1}</size>", size, message);
	}

	/// <summary>
	/// Renders the text in boldface.
	/// </summary>
	/// <param name="message">Message.</param>
	public static string Bold(this string message)
	{
		return string.Format ("<b>{0}</b>", message);
	}

	/// <summary>
	/// Renders the text in italics.
	/// </summary>
	/// <param name="message">Message.</param>
	public static string Italics(this string message)
	{
		return string.Format ("<i>{0}</i>", message);
	}

    public static void Shuffle<T>(this IList<T> list)
    {
        var rng = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public enum Colors
{
	aqua,
	black,
	blue,
	brown,
	cyan,
	darkblue,
	fuchsia,
	green,
	grey,
	lightblue,
	lime,
	magenta,
	maroon,
	navy,
	olive,
	purple,
	red,
	silver,
	teal,
	white,
	yellow
}

