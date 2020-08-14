﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GameObject;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    private Text roundLabel;

    private Button[] answerButtons;

    private Image maruImage;
    private Image batsuImage;

    //正解表示時間
    private float showClearSpan = 2.0f;
    //ミス表示時間
    private float showMissSpan = 2.0f;

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
        roundLabel = Find(HierarchyPath_Game.GameUICanvas.RoundLabel).GetComponent<Text>();
        var answerButtonParent = Find(HierarchyPath_Game.GameUICanvas.AnswerButtonParent).transform;
        answerButtons = new Button[answerButtonParent.childCount];
        maruImage = Find(HierarchyPath_Game.GameUICanvas.MaruImage).GetComponent<Image>();
        batsuImage = Find(HierarchyPath_Game.GameUICanvas.BatsuImage).GetComponent<Image>();

        //ボタン設定
        for(int i = 0; i < answerButtons.Length; i++)
        {
            var num = i;
            answerButtons[i] = answerButtonParent.GetChild(i).GetComponent<Button>();
            answerButtons[i].transform.GetChild(0).GetComponent<Text>().text = i.ToString();
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => { Answer(num); });
            answerButtons[i].interactable = false;
        }

        //非表示
        maruImage.gameObject.SetActive(false);
        batsuImage.gameObject.SetActive(false);

    }

    //ラウンド表記更新
    public void SetRoundText(int value)
    {
        roundLabel.text = $"Round {value}";
        //roundLabel.text = Mathf.Round(value).ToString();
    }

    //タイマー表示切替
    public void ShowRoundLabel(bool show)
    {
        roundLabel.gameObject.SetActive(show);
    }


    //人間の移動が完了したら呼び出して，解答ボタンを押せるようにする
    public void SetAnswerMode()
    {
        //解答ボタン有効に
        foreach (var b in answerButtons)
        {
            b.interactable = true;
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
            b.interactable = false;
        }

        //答えチェック関数を呼び出す
        GameManager.instance.CheckAnswer(num);
    }

    //クリア時○とか表示、一定時間経ったら次のラウンドへ
    public IEnumerator RoundClear(System.Action callback)
    {
        SoundManager.Instance.SoundSEPlay(SoundDefine.SE_CORRECT.key);
        maruImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(showClearSpan);
        maruImage.gameObject.SetActive(false);
        callback();
    }

    //失敗時ミス表示、一定時間経ったらランキング表示
    public IEnumerator FinishGame(System.Action callback)
    {
        SoundManager.Instance.SoundSEPlay(SoundDefine.SE_INCORRECT.key);
        batsuImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(showMissSpan);
        batsuImage.gameObject.SetActive(false);

        callback();
    }

}
