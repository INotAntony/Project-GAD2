﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the outcome of a dance off between 2 dancers, determines the strength of the victory form -1 to 1
/// 
/// TODO:
///     Handle GameEvents.OnFightRequested, resolve based on stats and respond with GameEvents.FightCompleted
///         This will require a winner and defeated in the fight to be determined.
///         This may be where characters are set as selected when they are in a dance off and when they leave the dance off
///         This may also be where you use the BattleLog to output the status of fights
///     This may also be where characters suffer mojo (hp) loss when they are defeated
/// </summary>
public class FightManager : MonoBehaviour
{
    public Color drawCol = Color.gray;

    public float fightAnimTime = 2;

    private void OnEnable()
    {
        GameEvents.OnFightRequested += Fight;
    }

    private void OnDisable()
    {
        GameEvents.OnFightRequested -= Fight;
    }

    public void Fight(FightEventData data)
    {
        StartCoroutine(Attack(data.lhs, data.rhs));
    }

    IEnumerator Attack(Character lhs, Character rhs)
    {
        lhs.isSelected = true;
        rhs.isSelected = true;
        lhs.GetComponent<AnimationController>().Dance();
        rhs.GetComponent<AnimationController>().Dance();

        yield return new WaitForSeconds(fightAnimTime);

        float outcome = 0;
        //defaulting to draw 
        Character winner = lhs, defeated = rhs;
        Debug.LogWarning("Attack called, needs to use character stats to determine winner with win strength from 1 to -1. This can most likely be ported from previous brief work.");
        float outome = Random.Range(-1.0f, 1.0f);

        float formulaL = (outcome * lhs.rhythm) + lhs.style / lhs.luck;
        formulaL = Mathf.Clamp(formulaL, -1.0f, 1.0f);

        float formulaR = (outcome * rhs.rhythm) + rhs.style / rhs.luck;
        formulaR = Mathf.Clamp(formulaR, -1.0f, 1.0f);

        Character win, defeat;

        if (formulaR > formulaL)
        {
            winner = rhs;
            defeated = lhs;
            outcome = formulaR;

           winner.charName.GetFullCharacterName();
            = winner.myTeam.teamColor;

            BattleLog.Log(new DefaultLogMessage("sup fam", Color.blue));
        }
        else
        {
            winner = lhs;
            defeated = rhs;
            outcome = formulaL;
        }
           


        var results = new FightResultData(winner, defeated, outcome);

        lhs.isSelected = false;
        rhs.isSelected = false;
        GameEvents.FightCompleted(results);

        yield return null;
    }
}
