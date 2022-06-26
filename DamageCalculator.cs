using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator
{
    public static int DamageBasedOnHeight(int baseDamage)
    {
        ScoreManager SM = GameObject.FindObjectOfType<ScoreManager>();
        return baseDamage + ((int)(SM.score / 10f));
    }
}
