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
        timerLabel = Find(HierarchyPath_Game.GameUICanvas.TimerLabel).GetComponent<Text>();
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

}
