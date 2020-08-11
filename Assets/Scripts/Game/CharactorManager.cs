using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] gameObjects;
    public Pattern[] Patterns;
    //Patternリスト
    List<Pattern> ListPattern;
    //人間データの格納先
    List<Charactor> ListCharactor;
    public GameObject TargetObj;

    List<GameObject> ListCharaObj;
    //ゲーム難易度
    public enum DifficultyLevel
    {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
    }
    [Header("画面上に出現出来る人間の数")]
    public int MaxHumanValue = 7;

    DifficultyLevel DifficultyLevel_ = DifficultyLevel.Level1;

    // Start is called before the first frame update
    void Start()
    {

        ////動きの仮実装
        //リスト初期化
        ListPattern = new List<Pattern>();
        ListCharactor = new List<Charactor>();
        ListCharaObj = new List<GameObject>();

        for (int i = 0; i < MaxHumanValue;i++)
        {
            //空のオブジェクト生成
            var CharactorObj = new GameObject();
            CharactorObj.name = "CharactorObj";

            //SpriteRendererをアタッチ
            CharactorObj.AddComponent<SpriteRenderer>();
            //Charactorをアタッチ＋初期化
            CharactorObj.AddComponent<Charactor>().Init();

            //Charactor Scriptをもらう
            ListCharactor.Add(CharactorObj.GetComponent<Charactor>());

            //Patternと動くオブジェクトを設定
            var M_Pattern = new MovePattern();

            //移動方向の決定
            M_Pattern.InStartInit(Pattern.Angle.Up);
            //移動方向の決定
            M_Pattern.OutStartInit(Pattern.Angle.Up);
            //家データの設定
            M_Pattern.TargetObject = TargetObj;
            //移動方向の初期化
            M_Pattern.MoveAngleInit(CharactorObj.transform);
            //リストへ追加
            ListPattern.Add(M_Pattern);

        }

    }
    

    // Update is called once per frame
    void Update()
    {
       foreach(var L_Pattern in ListPattern)
        {
            L_Pattern.Move();

        }
    }
}
