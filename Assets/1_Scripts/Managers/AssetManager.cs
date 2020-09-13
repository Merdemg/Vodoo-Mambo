using UnityEngine;
using System.Collections;

public class AssetManager : MonoBehaviour
{
	#region Singleton
	public static AssetManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

	public Sprite[] ballSpritesByLevel;
	public Sprite[] ballIconSpritesByLevel;
	public GameObject ballMergePrefab;
//	public GameObject[] ballMerges;

	public AnimatorOverrideController[] ballMergeAnimatorOverrides;



}

