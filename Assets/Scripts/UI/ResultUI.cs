using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GameObject;

public class ResultUI : MonoBehaviour
{
    public static ResultUI instance;
    private GameObject resultPanel;
    private Button retryButton;
    private Button titleButton;
    private Button rankingButton;

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

        InitUI();
    }

    private void InitUI()
    {
        //オブジェクト取得
        resultPanel = Find(HierarchyPath_Game.ResultUICanvas.ResultPanel).gameObject;
        resultPanel = Find(HierarchyPath_Game.ResultUICanvas.ResultPanel).gameObject;
        retryButton = Find(HierarchyPath_Game.ResultUICanvas.ResultPanelRetryButton).AddComponent<Button>();
        titleButton = Find(HierarchyPath_Game.ResultUICanvas.ResultPanelTitleButton).AddComponent<Button>();
        rankingButton = Find(HierarchyPath_Game.ResultUICanvas.ResultPanelRankingButton).AddComponent<Button>();

        //ボタン設定
        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(() => { StartCoroutine(Retry()); });
        titleButton.onClick.RemoveAllListeners();
        titleButton.onClick.AddListener(() => { StartCoroutine(Title()); });
        rankingButton.onClick.RemoveAllListeners();
        rankingButton.onClick.AddListener(() => { StartCoroutine(Ranking()); });

        //非表示
        resultPanel.SetActive(false);
    }


    //リトライボタン
    private IEnumerator Retry()
    {
        yield return null;

        GameManager.instance.InitializeGame();
    }

    //タイトルボタン
    private IEnumerator Title()
    {
        yield return null;

        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    //ランキングボタン
    private IEnumerator Ranking()
    {
        yield return null;

        //RankingUI.instance.ShowRankingPanel(true);
        RankingManager.instance.ShowRanking(true);
    }


    //リザルトパネル表示
    public void ShowResultPanel(bool show)
    {
        resultPanel.SetActive(show);
        if (show == true) 
        {
            //タイマー非表示
            GameUI.instance.ShowTimer(false);
        }
    }
}
