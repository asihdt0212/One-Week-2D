using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GameObject;

public class TitleUI : MonoBehaviour
{
    //ゲーム開始ボタン
    private Button startButton;
    //ローディングパネル
    private GameObject loadingPanel;

    [SerializeField, Header("ロード待機時間")]
    private float loadingSpan = 1.0f;

    private void Start()
    {
        InitUI();
    }

    //UI設定
    private void InitUI()
    {
        //オブジェクト取得
        startButton = Find(HierarchyPath_Title.TitleUICanvas.StartButton).AddComponent<Button>();
        loadingPanel = Find(HierarchyPath_Title.TitleUICanvas.LoadingPanel).gameObject;

        //ボタン設定
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => { StartCoroutine(StartGame()); });

        //非表示設定
        loadingPanel.SetActive(false);
    }

    private IEnumerator StartGame()
    {
        loadingPanel.SetActive(true);

        SoundManager.Instance.SoundSEPlay(5);

        yield return new WaitForSeconds(loadingSpan);

        SceneManager.LoadScene("Game");
    }
}
