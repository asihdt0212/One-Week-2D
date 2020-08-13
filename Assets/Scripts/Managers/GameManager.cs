using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //タイマー用変数 制限時間
    private readonly float limitTime = 60;
    private float timeDelta = 0;

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

        //初期化
        InitializeGame();
    }

     //初期化処理
    public void InitializeGame()
    {
        SetState(State.Game);
        ResultUI.instance.ShowResultPanel(false);
        //制限時間初期化
        timeDelta = limitTime;
        GameUI.instance.ShowTimer(true);

        //キャラクターマネージャー初期化
        StartCoroutine(CharactorManager.Instance.InitializeCharacterManager());
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
                    //TimerUpdate(); 
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
        timeDelta -= Time.deltaTime;
        if (GameUI.instance != null)
        {
            GameUI.instance.TimerUpdate(timeDelta);
        }

        if(timeDelta <= 0)
        {
            timeDelta = 0;
            GameUI.instance.TimerUpdate(limitTime);
            //リザルトへ
            ShowResult();
        }
    }



    //リザルト表示
    private void ShowResult()
    {
        SetState(State.Result);
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



    //解答チェック
    public void CheckAnswer(int answer)
    {
        int homeHumanCount = CharactorManager.Instance.GetHome().GetHumanValue() ;     //CharactorManagerのHome_から人間の数を取得する
        HomeCanvasUI.instance.SetHumanText(homeHumanCount, () => {
            //カウントが終わったら正誤チェックをする
            if (answer == homeHumanCount)
            {
                //正解
                Debug.LogError("正解！");
            }
            else
            {
                //ミス タイム減らす？
                Debug.LogError("ミス！");
            }

        });
    }
}
