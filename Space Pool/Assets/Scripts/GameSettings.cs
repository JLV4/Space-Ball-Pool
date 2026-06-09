using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "SpacePool/Game Settings")]
public class GameSettings : ScriptableObject
{
    public const float MIN_POWER = 20f;
    public const float MAX_POWER = 70f;
    public const float maxDragDistance = 100f;

    public const float DEFAULT_POWER = 25f;
    public const float MIN_BOUNCINESS = 0f;
    public const float MAX_BOUNCINESS = 1f;
    public const float DEFAULT_WALL_BOUNCINESS = .75f;

    [Range(0f, 1f)] public const float movingAlpha = 0.5f;
    [Range(MIN_BOUNCINESS, MAX_BOUNCINESS)] public float wallBounciness = 0.5f;

    public Nullable<bool> p1Stripes;

    public int p1Score;
    public int p2Score;
    public int currPlayer; // 1 for Player 1, 2 for Player 2
    public bool gameOver;

    [Header("Player Settings")]
    [Range(MIN_POWER, MAX_POWER)]
    //set the default value here
    public float maxShotPower = DEFAULT_POWER;

    //randomizes power
    public void RandomizeAll()
    {
        maxShotPower = UnityEngine.Random.Range(MIN_POWER, MAX_POWER);
        wallBounciness = UnityEngine.Random.Range(0f, 1f);
    }

    /// <summary>
    /// Resets all settings to their defined default values.
    /// </summary>
    public void ResetToDefaults()
    {
        maxShotPower = DEFAULT_POWER;
        wallBounciness = DEFAULT_WALL_BOUNCINESS;
        p1Score = 0;
        p2Score = 0;
    }
}