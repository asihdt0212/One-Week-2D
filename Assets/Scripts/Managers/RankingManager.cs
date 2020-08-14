using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    //ランキング表示
    public void ShowRanking(bool show)
    {
        var data = UserDataManager.instance.GetUserData();
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(data.currentRound);
    }
}
