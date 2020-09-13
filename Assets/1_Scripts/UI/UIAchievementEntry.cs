using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class UIAchievementEntry : MonoBehaviour {

	[SerializeField] Image icon;
	[SerializeField] Text nameText;
	[SerializeField] Achievement achievement;
    public Achievement Achievement{
        get{
            return achievement;
        }
    }

	void Awake()
	{
		icon.sprite = achievement.icon;
		nameText.text = achievement.name;
	}

	public void Init(bool earned)
	{
		gameObject.SetActive (earned);
//		if (earned)
//			gameObject.SetActive (false);
//		else
//			nameText.text = "not earned yet";
	}

	public void LoadWithEarnedArray(List<string> earnedArr)
	{
		if(earnedArr.Contains(achievement.shortCode))
		{
			Init (true);
		}
		else
		{
			Init (false);
		}
	}

	#if UNITY_EDITOR

	[Space]
	[SerializeField]bool refresh = false;
	void Update()
	{
		if(refresh)
		{
			Refresh ();
		}
	}
	public void Refresh()
	{
		refresh = false;

		icon.sprite = achievement.icon;
		nameText.text = achievement.name;
		name = achievement.shortCode;

	}

	#endif
}
