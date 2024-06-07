using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EloCalculate : MonoBehaviour
{
    [SerializeField] private static float baseAmt;
    private static float winMult;
    private static float lossMult;
    private static float bonusAmt;

    public static int CalculateElo(Rank rank, bool hasBonus, bool victory)
    {
        calculateMultipliers(rank); //calculates the multipliers based on the player rank inputted
        if (!hasBonus) { bonusAmt = 0f; } //removes bonus if no bonus

        int elo = (int) (  (baseAmt + (hasBonus ? bonusAmt : 0)) * (victory ? winMult : lossMult)  );
        return elo;
    }

    private static void calculateMultipliers(Rank rank)
    {
        switch (rank)
        {
            case Rank.Fledgeling:
                winMult = 2;
                lossMult = 0;
                bonusAmt = 10f;
                break;
            case Rank.Apprentice:
                winMult = 1.8f;
                lossMult = 0.5f;
                bonusAmt = 10f;
                break;
            case Rank.Advanced:
                winMult = 1.5f;
                lossMult = 0.5f;
                bonusAmt = 8f;
                break;
            case Rank.Expert:
                winMult = 1.25f;
                lossMult = 0.75f;
                bonusAmt = 8f;
                break;
            case Rank.Speedster:
                winMult = 1f;
                lossMult = 1f;
                bonusAmt = 6f;
                break;
            case Rank.Stunter:
                winMult = 1f;
                lossMult = 1.25f;
                bonusAmt = 5f;
                break;
            case Rank.Master:
                winMult = 0.75f;
                lossMult = 1.25f;
                bonusAmt = 5f;
                break;
            case Rank.Godspeed:
                winMult = 0.5f;
                lossMult = 1.5f;
                bonusAmt = 4f;
                break;
            default:
                break;
        }
    }
}
