/***************************************************************************\
Project:      Daily Rewards
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using NiobiumStudios;

/** 
 * This is just a snippet of code to integrate Daily Rewards into your project
 * 
 * Copy / Paste the code below
 **/
public class IntegrationDailyRewards : MonoBehaviour
{
    void OnEnable()
    {
        DailyRewards.instance.onClaimPrize += OnClaimPrizeDailyRewards;
    }

    void OnDisable()
    {
		DailyRewards.instance.onClaimPrize -= OnClaimPrizeDailyRewards;
    }

    // this is your integration function. Can be on Start or simply a function to be called
    public void OnClaimPrizeDailyRewards(int day)
    {
       //This returns a Reward object
		Reward myReward = DailyRewards.instance.GetReward(day);

        if(myReward.unit == Define.Rewards.Coins)
        {
            GameController.Instance.Player.GameData.Coins += myReward.reward;
        }
        else if(myReward.unit == Define.Rewards.Diamonds)
        {
            GameController.Instance.Player.GameData.Diamonds += myReward.reward;
        }

        GameController.Instance.Player.GameData.Save();
    }
}