using System;
using UnityEngine;



[Serializable]
public class Score : Resource<long>
{
    public Score(long initialValue) : base(initialValue)
    {

    }

    public DifficultyLevel Difficulty(){
        if (Value >= 1000000000000) // 1 trillion (1t)
        {
            return DifficultyLevel.Extreme;
        }
        else if (Value >= 1000000000) // 1 billion (1b)
        {
            return DifficultyLevel.VeryHard;
        }
        else if (Value >= 1000000) // 1 million (1m)
        {
            return DifficultyLevel.Hard;
        }
        else if (Value >= 1000) // 1 thousand (1k)
        {
            return DifficultyLevel.Medium;
        }
        else
        {
            return DifficultyLevel.Easy; // No suffix for values less than 1000
        }
    }
}