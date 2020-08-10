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
    public int MaxHumanValue = 1;

    DifficultyLevel DifficultyLevel_ = DifficultyLevel.Level1;

    // Start is called before the first frame update
    void Start()
    {

        ////動きの仮実装
        //リスト初期化
        ListPattern = new List<Pattern>();
        ListCharactor = new List<Charactor>();

       //Patternと動くオブジェクトを設定
       var M_Pattern = new MovePattern();
        M_Pattern.Mytransform = gameObjects[0].transform;
        //リストへ追加
        ListPattern.Add(M_Pattern);

        ListPattern[0].Move();

        //Patternと動くオブジェクトを設定
        var M_Pattern2 = new MovePattern2();
        M_Pattern2.Mytransform = gameObjects[1].transform;
        //リストへ追加
        ListPattern.Add(M_Pattern2);

        ListPattern[1].Move();

        //空のオブジェクト生成
        var CharactorObj = new GameObject();
        CharactorObj.name = "CharactorObj";

        //SpriteRendererをアタッチ
        CharactorObj.AddComponent<SpriteRenderer>();
        //Charactorをアタッチ＋初期化
        CharactorObj.AddComponent<Charactor>().Init();
        //Charactor Scriptをもらう
        ListCharactor.Add(CharactorObj.GetComponent<Charactor>());

    }

    // Update is called once per frame
    void Update()
    {
        ListPattern[0].Move();
        ListPattern[1].Move();
    }
}
