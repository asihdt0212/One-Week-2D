using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorManager : Singleton<CharactorManager>
{
    [SerializeField]
    GameObject[] gameObjects;
    
    //Patternリスト
    List<Pattern> ListPattern = new List<Pattern>();
    //人間データの格納先
    List<Charactor> ListCharactor = new List<Charactor>();
    public GameObject TargetObj;
    //家データ
    private Home Home_;
    
    List<GameObject> ListCharaObj = new List<GameObject>();

    protected SelectMode SelectMode_;

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

        //ランダムにCreatePatternType_に成分を入れる。
        for(int i = 0;i < 9; i++)
        {
            CreatePatternType CreatePatternTypes;

            float R_Value =  Random.Range(0f, 1f) * 59;

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

            CreatePatternType_.Add(CreatePatternTypes);
        }

        
        //

        SelectMode_ = new SelectMode();

        

        Home_ = new Home();

        for (int i = 0; i < SelectMode_.MaxHumanValue; i++)
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
            var M_Pattern = new MovePattern(i, CharactorObj.transform,TargetObj);

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
        for (int i = 0; i < SelectMode_.MaxHumanValue; i++)
        {
            //移動アップデート
            
            ListPattern[i].Move();

            //動いているオブジェクトが、Endになった時
            if(ListPattern[i].GetActiveMove() == Pattern.ActiveMove.End)
            {
                //出現から移動の終了時 false、
                if (!ListPattern[i].m_MoveFlag)
                {
                    Debug.Log("AddHuman");
                    //人間の加算処理。
                    Home_.AddHuman(ListPattern[i].Human);

                    //待機状態に
                    ListPattern[i].SetAcitveMove(Pattern.ActiveMove.Wait);

                    ListCharaObj[i].gameObject.SetActive(false);

                    //HomeCanvasUI.instance.SetHumanText(Home_.GetHumanValue());

                    //全員の移動が完了時 GameUI.SetAnswerMode()を呼び出す。
                }
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
}
