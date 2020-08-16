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

    private int currentRound = 1;

    //ラウンドコール時間
    private float roundCallDelay = 2.0f;

    public enum State
    {
        Idle,
        RoundCall,      //ラウンドコール
        Game,           //ゲーム開始　
        Result,         //リザルト
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
    }

    private void Start()
    {
        //初期化
        InitializeGame();

        //BGM
        SoundManager.Instance.SoundBGMPlay(1);
    }

    //初期化処理
    public void InitializeGame()
    {
        //SetState(State.Game);
        SetState(State.RoundCall);
        ResultUI.instance.ShowResultPanel(false);
        //制限時間初期化
        //timeDelta = limitTime;

        //人数カウントラベル非表示
        HomeCanvasUI.instance.ShowHumanText(false);

        //ラウンド表記
        //var data = UserDataManager.instance.GetUserData();
        //GameUI.instance.SetRoundText(data.currentRound);
        //GameUI.instance.ShowRoundLabel(true);
        //GameUI.instance.ShowTimer(true);

        //キャラクターマネージャー初期化
        //StartCoroutine(CharactorManager.Instance.InitializeCharacterManager());

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

            case State.RoundCall:
                {
                    //ラウンドコール　
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

        /*
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
    */



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

            case State.RoundCall:
                {
                    //家の位置の初期化
                    CharactorManager.Instance.HomeResetPosi();
                    //ラウンドコール
                    StartCoroutine(RoundCall());
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

    IEnumerator RoundCall()
    {
        //ラウンド表記
        var data = UserDataManager.instance.GetUserData();
        GameUI.instance.SetRoundText(data.currentRound);
        GameUI.instance.ShowRoundLabel(true);

        //Start表記
        StartCoroutine(GameUI.instance.ShowStartLabel(roundCallDelay));

        yield return new WaitForSeconds(roundCallDelay);

        //キャラクターマネージャー初期化
        StartCoroutine(CharactorManager.Instance.InitializeCharacterManager());
        //ゲーム開始
        SetState(State.Game);
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
                StartCoroutine(GameUI.instance.RoundClear(()=> {
                    //次のラウンドへ
                    UserDataManager.instance.NextRound();
                    InitializeGame();
                }));
            }
            else
            {
                //ミス ゲームオーバー
                Debug.LogError("ミス！");
                StartCoroutine(GameUI.instance.FinishGame(() => { SetState(State.Result);}));
                //SetState(State.Result);
            }

        });
    }
}
