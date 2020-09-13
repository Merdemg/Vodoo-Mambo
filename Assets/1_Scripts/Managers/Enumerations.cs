public enum Direction
{
	North,
	East,
	South,
	West
}

public enum GameplayState
{
	Stopped,
	Initiating,
	Playing,
	Paused,
    Suspended // Example: Bomb exploding, interaction disabled, timer disabled but movements are active
}

public enum ScoreType
{
	NotScored,
	Pinch,
	Blackhole,
	Explode,
	GameEndExplode
}

public enum TutorialType
{
	Pinch,
	SpellBomb,
	SpellLevelup,
	SpellBlackhole,
	None
}