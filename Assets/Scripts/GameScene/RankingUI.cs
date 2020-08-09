using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GameObject;

public class RankingUI : MonoBehaviour
{
    public static RankingUI instance;
    private GameObject rankingPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        InitUI();
    }

    private void InitUI()
    {
        //オブジェクト取得
        rankingPanel = Find(HierarchyPath_Game.RankingUICanvas.RankingPanel).gameObject;

        //非表示
        rankingPanel.SetActive(false);
    }
}
