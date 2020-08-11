using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GameObject;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    private Text timerLabel;

    private Button[] answerButtons;

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

    //UI初期化
    private void InitUI()
    {
        //オブジェクト取得
        timerLabel = Find(HierarchyPath_Game.GameUICanvas.TimerLabel).GetComponent<Text>();
        var answerButtonParent = Find(HierarchyPath_Game.GameUICanvas.AnswerButtonParent).transform;
        answerButtons = new Button[answerButtonParent.childCount];

        //ボタン設定
        for(int i = 0; i < answerButtons.Length; i++)
        {
            var num = i;
            answerButtons[i] = answerButtonParent.GetChild(i).GetComponent<Button>();
            answerButtons[i].transform.GetChild(0).GetComponent<Text>().text = i.ToString();
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => { Answer(num); });
            answerButtons[i].enabled = false;
        }

    }

    //タイマー表記更新
    public void TimerUpdate(float value)
    {
        timerLabel.text = Mathf.Round(value).ToString();
    }

    //タイマー表示切替
    public void ShowTimer(bool show)
    {
        timerLabel.gameObject.SetActive(show);
    }


    //人間の移動が完了したら呼び出して，解答ボタンを押せるようにする
    public void SetAnswerMode()
    {
        //解答ボタン有効に
        foreach (var b in answerButtons)
        {
            b.enabled = true;
        }
    }

     //解答ボタン挙動
    private void Answer(int num)
    {
        //解答
        Debug.Log($"Answer: {num}");

        //解答ボタン無効に
        foreach(var b in answerButtons)
        {
            b.enabled = false;
        }

        //答えチェック関数を呼び出す
        //******************************
    }

}
