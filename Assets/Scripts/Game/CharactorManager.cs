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

    //ゲーム難易度
    public enum DifficultyLevel
    {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        
    }

    DifficultyLevel DifficultyLevel_ = DifficultyLevel.Level1;

    // Start is called before the first frame update
    void Start()
    {

        //動きの仮実装
        ListPattern = new List<Pattern>();
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


    }

    // Update is called once per frame
    void Update()
    {
        ListPattern[0].Move();
        ListPattern[1].Move();
    }
}
