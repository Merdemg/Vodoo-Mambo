using UnityEngine;
using System.Collections;

public class SoundsManager : MonoBehaviour
{
	public static SoundsManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	public float inGameMusicVolume = .7f;
	[Header("Music")]
	public AudioClip mainMenuMusic;
	public AudioClip ingameMusic;
	public AudioClip endgameMusic;

	[Header("UI")]
	public AudioClip genericClick;
	public AudioClip menuPopUp;
	public AudioClip ballFill;

    public AudioClip[] ballFills;
    public AudioClip ballFillLevelDone;

	[Header("Ingame UI")]
	public AudioClip[] shamanYell;
	public AudioClip readyStart;
	public AudioClip lastSeconds;

	// In Game

	// Balls
	[Header("Balls")]
	public AudioClip[] mergeLevels;
	public AudioClip[] ballSpawns;
    public AudioClip[] mergeByCombo;
	// Bomb
	[Header("Bomb")]
	public AudioClip bombSpawn;
	public AudioClip bombActivate;
	public AudioClip[] bombExplosions;
	// LevelUp
	[Header("Level Up")]
	public AudioClip levelupSpawn;
	public AudioClip levelupActivate;
	// Blackhole
	[Header("Blackhole")]
	public AudioClip blackholeSpawn;
    public AudioClip blackholeEnd;
    public AudioClip[] blackholeSucks;
    [Header("UI")]
    public AudioClip achievementPanel;
    public AudioClip hiscorePanel;


}

