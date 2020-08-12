﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorManager : Singleton<CharactorManager>
{
    [SerializeField]
    GameObject[] gameObjects;
    
    //Patternリスト
    List<MovePattern> ListPattern = new List<MovePattern>();
    //人間データの格納先
    List<Charactor> ListCharactor = new List<Charactor>();
    public GameObject TargetObj;
    //家データ
    private Home Home_;
    //キャラクターオブジェクトの格納先
    List<GameObject> ListCharaObj = new List<GameObject>();
    //ゲームの更新タイム
    private float G_Time = 0;

    //ゲーム難易度の要素
    private SelectMode SelectMode_ = new SelectMode();

    //Patternの作成パターン
    /*
    0　家にランダムで入る。
    1　家からランダムで出る。出来なかったとき　0を実行
    2　家に移動方向Rightで入る。
    3　家に移動方向Downで入る。
    4  家から移動方向Rightで出る。出来なかったとき　0を実行
    5  家から移動方向Upで出る。出来なかったとき　0を実行
    */
    public enum CreatePatternType
    {
        RandomHomeInType,
        RandomHomeOutType,
        HomeInRightType,
        HomeInDownType,
        HomeOutRightType,
        HomeOutUpType,
    }
    List<CreatePatternType> CreatePatternType_ = new List<CreatePatternType>();


    // Start is called before the first frame update
    void Start()
    {

        ////動きの仮実装
        //リスト初期化
        ListPattern.Clear();
        ListCharactor.Clear();
        ListCharaObj.Clear();

        CreatePatternType_.Clear();

        //時間の初期化
        G_Time = 0;
        //家データの作成
        Home_ = new Home(0);
        //ゲーム難易度の初期化?
        //ぐるぐるするなら、呼び出さないほうが良いかも?(上のnewで一応初期化は出来ていますが...)
        //SelectMode_.SelectModeInit();
        //ゲームの難易度を10へ設定
        SelectMode_.SetGameLevel(10);


        //ランダムにCreatePatternType_に成分を入れる。
        for (int i = 0;i < SelectMode_.GetCreateMaxHumanValue(); i++)
        {
            //キャラクターの行動種類のパターン変数を宣言
            CreatePatternType CreatePatternTypes;

            //乱数を生成
            float R_Value =  Random.Range(0f, 1f) * 59;

            //乱数に応じて行動種類を選択
            if (R_Value >= 0 && R_Value < 10)
            {
                CreatePatternTypes = CreatePatternType.RandomHomeInType;
            }
            else if (R_Value >= 10 && R_Value < 20)
            {
                CreatePatternTypes = CreatePatternType.RandomHomeOutType;
            }
            else if (R_Value >= 20 && R_Value < 30)
            {
                CreatePatternTypes = CreatePatternType.HomeInRightType;
            }
            else if (R_Value >= 30 && R_Value < 40)
            {
                CreatePatternTypes = CreatePatternType.HomeInDownType;
            }
            else if (R_Value >= 40 && R_Value < 50)
            {
                CreatePatternTypes = CreatePatternType.HomeOutRightType;
            }
            else if (R_Value >= 50 && R_Value < 60)
            {
                CreatePatternTypes = CreatePatternType.HomeOutUpType;
            }
            else
            {
                CreatePatternTypes = CreatePatternType.RandomHomeInType;
            }
            //キャラクターのパターンを入れる
            CreatePatternType_.Add(CreatePatternTypes);
        }

        
        for (int i = 0; i < SelectMode_.GetCreateMaxHumanValue(); i++)
        {
            //空のオブジェクト生成
            var CharactorObj = new GameObject();
            CharactorObj.name = "CharactorObj";

            //SpriteRendererをアタッチ
            CharactorObj.AddComponent<SpriteRenderer>();
            //Charactorをアタッチ＋初期化
            CharactorObj.AddComponent<Charactor>().Init();
            //オブジェクトの格納
            ListCharaObj.Add(CharactorObj);

            //Charactor Scriptをもらう
            ListCharactor.Add(CharactorObj.GetComponent<Charactor>());

            //Patternと動くオブジェクトを設定
            var M_Pattern = new MovePattern(i, CharactorObj.transform,TargetObj,SelectMode_.GetCharacterSppedValue());
            
            //Patternの設定
            switch (CreatePatternType_[i])
            {
                case CreatePatternType.RandomHomeInType:

                    //移動方向の決定
                    M_Pattern.RondomHomeInInit();

                    break;
                case CreatePatternType.RandomHomeOutType:

                    //移動方向の決定
                    M_Pattern.RondomHomeOutInit();

                    break;
                case CreatePatternType.HomeInRightType:

                    M_Pattern.HomeSelectInInit();

                    break;
                case CreatePatternType.HomeInDownType:

                    M_Pattern.HomeSelectInInit();

                    break;
                case CreatePatternType.HomeOutRightType:

                    M_Pattern.HomeSelectOutInit();

                    break;
                case CreatePatternType.HomeOutUpType:

                    M_Pattern.HomeSelectOutInit();

                    break;
                default:
                    break;
            }

            //移動方向の初期化
            M_Pattern.MoveAngleInit();
            
            //リストへ追加
            ListPattern.Add(M_Pattern);
            //親
            CharactorObj.gameObject.transform.parent= this.transform;

        }

    }
    

    // Update is called once per frame
    void Update()
    {
        G_Time += Time.deltaTime;
        for (int i = 0; i < SelectMode_.GetCreateMaxHumanValue(); i++)
        {
            //移動アップデート

            switch (ListPattern[i].GetActiveMove())
            {
                case Pattern.ActiveMove.Wait:
                    break;
                case Pattern.ActiveMove.MoveCheck:
                    //移動の実行
                    if (G_Time > ListPattern[i].GetMyNumber())
                    {
                        if (ListPattern[i].m_MoveFlag)
                        {
                            //人間が家にいるかチェック
                            ListPattern[i].HouseHumanCheck(Home_);
                        }
                        else
                        {
                            //移動開始時にもし、家が満タンだった時
                            if(GetHomeInCharactorCheck() >= SelectMode_.MaxHumanValue)
                            {
                                //入るパターンに変更
                                ListPattern[i].RondomHomeOutInit();
                            }
                            //満タンではない
                            else
                            {
                                ListPattern[i].SetAcitveMove(Pattern.ActiveMove.Move);
                            }
                            
                        }
                        
                    }


                    break;
                case Pattern.ActiveMove.Move:
                    //移動実行
                    ListPattern[i].Move();
                    
                    break;

                case Pattern.ActiveMove.End:

                    //出現から移動の終了時 false、
                    if (!ListPattern[i].m_MoveFlag)
                    {
                        Debug.Log("AddHuman");
                        //人間の加算処理。
                        Home_.AddHuman(ListPattern[i].Human);

                        //HomeCanvasUI.instance.SetHumanText(Home_.GetHumanValue());

                        //全員の移動が完了時 GameUI.SetAnswerMode()を呼び出す。
                    }

                    //待機状態に
                    ListPattern[i].SetAcitveMove(Pattern.ActiveMove.Wait);

                    //オブジェクトを消す
                    ListCharaObj[i].gameObject.SetActive(false);

                    //最後の奴の場合
                    if (i == (ListPattern.Count - 1))
                    {
                        Debug.Log("OK");
                        GameUI.instance.SetAnswerMode();
                    }
                    //それ以外
                    else
                    {
                        Debug.Log("NO" + i + " " + (ListPattern.Count - 1));
                    }

                    break;
                default:
                    break;
            }

           

            
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(Home_.GetHumanValue());
        }
    }


    public Home GetHome()
    {
        return Home_;
    }
    //家に入っている人間の数と、
    //家に向かっている人間の数を調べる。
    public int GetHomeInCharactorCheck()
    {
        int TotalHumanValue = 0;

        TotalHumanValue = Home_.GetHumanValue();

        int MoveCharactorValue = 0;

        foreach(var L_Pattern in ListPattern)
        {
            if (L_Pattern.GetActiveMove() == Pattern.ActiveMove.Move || L_Pattern.m_MoveFlag == false)
            {
                MoveCharactorValue++;
            }
        }

            return TotalHumanValue;
    }
}
