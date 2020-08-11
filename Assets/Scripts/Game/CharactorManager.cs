﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] gameObjects;
    public Pattern[] Patterns;
    //Patternリスト
    List<Pattern> ListPattern = new List<Pattern>();
    //人間データの格納先
    List<Charactor> ListCharactor = new List<Charactor>();
    public GameObject TargetObj;

    protected Home Home_;

    [SerializeField]
    Text HoemHpText;

    List<GameObject> ListCharaObj = new List<GameObject>();

    protected SelectMode SelectMode_; 

    // Start is called before the first frame update
    void Start()
    {
        HoemHpText.text = "0";
        ////動きの仮実装
        //リスト初期化
        ListPattern.Clear();
        ListCharactor.Clear();
        ListCharaObj.Clear();
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
            var M_Pattern = new MovePatternLeft();

            //移動方向の決定
            //M_Pattern.OutStartInit(Pattern.Angle.Down);
            //団体人数
            M_Pattern.People = 1;
            //家データの設定
            M_Pattern.TargetObject = TargetObj;
            //移動方向の初期化
            M_Pattern.MoveAngleInit(CharactorObj.transform);
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
            
            if(ListPattern[i].GetActiveMove() == Pattern.ActiveMove.Move) ListPattern[i].Move();

            //動いているオブジェクトが、Endになった時
            if(ListPattern[i].GetActiveMove() == Pattern.ActiveMove.End)
            {
                //出現から移動の終了時 false、
                if (!ListPattern[i].m_MoveFlag)
                {
                    Debug.Log("AddHuman");
                    //人間の加算処理。
                    Home_.AddHuman(ListPattern[i].People);

                    //待機状態に
                    ListPattern[i].SetAcitveMove(Pattern.ActiveMove.Wait);

                    ListCharaObj[i].gameObject.SetActive(false);

                    HoemHpText.text = Home_.GetHumanValue().ToString();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(Home_.GetHumanValue());
        }
    }
}
