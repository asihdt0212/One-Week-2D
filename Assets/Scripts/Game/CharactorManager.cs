using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //初期化
    public IEnumerator InitializeCharacterManager()
    {
        //生成済みのキャラクターオブジェクトなど全て消去
        foreach(Transform _obj in this.transform)
        {
            Destroy(_obj.gameObject);
        }

        yield return null;

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
        for (int i = 0; i < SelectMode_.GetCreateMaxHumanValue(); i++)
        {
            //キャラクターの行動種類のパターン変数を宣言
            CreatePatternType CreatePatternTypes;

            //乱数を生成
            //float R_Value =  Random.Range(0f, 1f) * 59;
            int R_Value = Random.Range(0, System.Enum.GetValues(typeof(CreatePatternType)).Length);
            CreatePatternTypes = (CreatePatternType)R_Value;
            /*
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

             */
            //キャラクターのパターンを入れる
            CreatePatternType_.Add(CreatePatternTypes);
        }

        //このゲームで使用する全パターンについて、人間（グループ）を生成する
        //for (int i = 0; i < SelectMode_.GetCreateMaxHumanValue(); i++)
        for(int i = 0; i < CreatePatternType_.Count; i++)
        {
            CreateCharacter(i);
        }
    }

    void Start()
    {
        //InitializeCharacterManager();
    }

    void CreateCharacter(int i)
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
        var M_Pattern = new MovePattern(i, CharactorObj.transform, TargetObj, SelectMode_.GetCharacterSppedValue());

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
        //人間に人数をランダムで入れる
        int R_humanValue = Random.Range(1, 4);

        Debug.Log($"生成された人数：{R_humanValue}");

        //人間のが数を入れる。
        M_Pattern.Human = R_humanValue;
        //数分　人間オブジェクを生成
        ListCharactor[i].CreateCharacter(R_humanValue);

        //移動方向の初期化
        M_Pattern.MoveAngleInit();

        //リストへ追加
        ListPattern.Add(M_Pattern);
        //親
        CharactorObj.transform.SetParent(this.transform);
        //CharactorObj.gameObject.transform.parent = this.transform;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log($"今向かっている人間の数は:{GetHomeInCharactorCheck()}");
        }
        G_Time += Time.deltaTime;
        
        //for (int i = 0; i < SelectMode_.GetCreateMaxHumanValue(); i++)
        for(int i = 0; i < ListPattern.Count; i++)
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
                        //出るか入るかどちらのフラグが立っているかのチェック
                        if (ListPattern[i].m_MoveFlag)
                        {
                            
                            //人間が家にいるかチェック
                            ListPattern[i].HouseHumanCheck(Home_);
                        }
                        else
                        {
                            //移動開始時にもし、家が満タンだった時 または　家の人間の数と入ろうとしている人間がオーバーしているとき
                            if (GetHomeInCharactorCheck() >= SelectMode_.MaxHumanValue ||
                                GetHomeInCharactorCheck() + ListPattern[i].Human > SelectMode_.MaxHumanValue)
                            {
                                
                                //出るパターンに変更
                                ListPattern[i].RondomHomeOutInit();
                                //移動位置の変更
                                ListPattern[i].MoveAngleInit();
                            }
                            //満タンではない
                            else
                            {
                                //
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
                        Debug.Log("AddHuman + " + ListPattern[i].Human);
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
                        Debug.Log("Last OK");
                        GameUI.instance.SetAnswerMode();
                    }
                    //それ以外
                    else
                    {
                        //Debug.Log("NO" + i + " " + (ListPattern.Count - 1));
                    }

                    break;
                default:
                    break;
            }

           

            
        }
        
            //Debug.Log(Home_.GetHumanValue());
        
    }


    public Home GetHome()
    {
        return Home_;
    }
    //家に入っている人間の数と、
    //家に向かっている人間の数を調べる。
    public int GetHomeInCharactorCheck()
    {
        //return値の格納変数を用意//家にいる人数分を入れる
        int TotalHumanValue = 0; 

        //今移動中で家に入ろうとしているのキャラを入れる変数
        int MoveCharactorValue = 0;

        //今家に入ろうと動いているキャラを取得
        foreach(var L_Pattern in ListPattern)
        {
            if (L_Pattern.GetActiveMove() == Pattern.ActiveMove.Move && L_Pattern.m_MoveFlag == false)
            {
                //人間分かううんとする。
                MoveCharactorValue+= L_Pattern.Human;
            }
        }
        //カウント分+
        //Debug.Log(MoveCharactorValue);
        TotalHumanValue += MoveCharactorValue;

        return TotalHumanValue;
    }

}
