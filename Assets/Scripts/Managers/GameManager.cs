using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //タイマー用変数 制限時間
    private float limitTime = 60;

    public enum State
    {
        Idle,
        Game,
        Result,
    }

    [SerializeField, Header("ステート")]
    private State state = default;

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

        //ゲーム開始
        SetState(State.Game);
    }


    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                {
                    //待機中
                }
                break;

            case State.Game:
                {
                    //タイマー
                    TimerUpdate(); 
                }
                break;

            case State.Result:
                {
                    //リザルト
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShowResult();
        }
    }


  /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void TimerUpdate()
    {
        limitTime -= Time.deltaTime;
        if (GameUI.instance != null)
        {
            GameUI.instance.TimerUpdate(limitTime);
        }

        if(limitTime <= 0)
        {
            limitTime = 0;
            GameUI.instance.TimerUpdate(limitTime);
            //リザルトへ
            SetState(State.Result);
        }
    }



    //リザルト表示
    private void ShowResult()
    {

    }

    public void SetState(State s)
    {
        Debug.Log("SetStage" + s);
        state = s;

        switch (state)
        {
            case State.Idle:
                {
                    //待機中
                }
                break;

            case State.Game:
                {
                    //ゲーム開始
                }
                break;

            case State.Result:
                {
                    //リザルトパネル表示
                    ResultUI.instance.ShowResultPanel(true);
                }
                break;
        }
    }
}
