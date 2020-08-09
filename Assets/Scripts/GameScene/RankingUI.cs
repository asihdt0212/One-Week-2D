using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GameObject;

public class RankingUI : MonoBehaviour
{
    public static RankingUI instance;
    private GameObject rankingPanel;
    private Button closeRankingPanelButton;

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
        closeRankingPanelButton = Find(HierarchyPath_Game.RankingUICanvas.RankingPanelCloseRankingButton).AddComponent<Button>();

        //ボタン設定
        closeRankingPanelButton.onClick.RemoveAllListeners();
        closeRankingPanelButton.onClick.AddListener(() => { ShowRankingPanel(false); });

        //非表示
        rankingPanel.SetActive(false);
    }

    public void ShowRankingPanel(bool show)
    {
        rankingPanel.SetActive(show);
    }
}
