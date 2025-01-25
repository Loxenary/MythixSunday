using System;
using UnityEngine;

[Serializable]
public class Score : Resource<long>
{
    public Score(long initialValue) : base(initialValue)
    {

    }

    public int Difficulty(){
        if (Value >= 1000000000000) // 1 trillion (1t)
        {
            return 5;
        }
        else if (Value >= 1000000000) // 1 billion (1b)
        {
            return 4;
        }
        else if (Value >= 1000000) // 1 million (1m)
        {
            return 3;
        }
        else if (Value >= 1000) // 1 thousand (1k)
        {
            return 2;
        }
        else
        {
            return 1; // No suffix for values less than 1000
        }
    }
}