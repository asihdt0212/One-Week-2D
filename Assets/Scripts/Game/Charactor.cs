﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Charactor : MonoBehaviour
{
    //画像の変更に使うものだったもの
    public enum ChangeSpriteType
    {
        Wait,
        Wait2,
        Banzai,
        Run,
    }
    //読みこんだspriteデータの格納先
    List<Sprite> CharactorSprite;
    private SpriteRenderer _renderer = default;

    //生成したゲームオブジェクトの一時保存先
    List<GameObject> CreateObjArray = new List<GameObject>();

    //初期化
    public void Init()
    {
        this.gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0);

        CharactorSprite = new List<Sprite>();
       //画像データ読みこみ
       Sprite[] LoadSprite = Resources.LoadAll<Sprite>("308");

        if (LoadSprite != null)
        {
            //Debug.Log("308 読み込み完了");
            Debug.Log("画像数" + LoadSprite.Length);
            //画像の追加
            for(int i = 0;i < LoadSprite.Length; i++)
            {
                CharactorSprite.Add(LoadSprite[i]);
            }
            _renderer = this.GetComponent<SpriteRenderer>();
            _renderer.sprite = CharactorSprite.FirstOrDefault();
            //this.GetComponent<SpriteRenderer>().sprite = CharactorSprite[0];
        }
        else
        {
            Debug.Log("308 読み込み出来ませんでした");
        }
    }
    //人数分画像を生成。
    public void CreateCharacter(int CreateHumanvalue)
    {
        //生成したゲームオブジェクトの一時保存先の初期化
        CreateObjArray.Clear();

        for (int i = 0; i < CreateHumanvalue - 1; i++)
        {
            Debug.Log("実行 "+ CreateHumanvalue);
            //自分の複製を作って
            GameObject CreateObj = Instantiate(this.gameObject) as GameObject;

            //ずらしの値を生成
            Vector3 OffSet = this.transform.localScale*2;
            OffSet.y = 0.0f;
            OffSet.z = 0.0f;

            //偶数の時 
            if ((i % 2) == 0){
                //大きさ分+の方へずらす
                CreateObj.transform.position += OffSet * ((i / 2)+1);
            }
            //奇数の時
            else
            {
                //大きさ分-の方へずらす
                CreateObj.transform.position -= OffSet * ((i / 2) + 1);
            }

            //一時保存
            CreateObjArray.Add(CreateObj);

        }
        //保存した分
        foreach(var SetObj in CreateObjArray)
        {
            //子データになる。
            SetObj.transform.parent = transform;
        }
    }
    public void ChangeSprite(ChangeSpriteType spriteType)
    {
        //this.GetComponent<SpriteRenderer>().sprite = CharactorSprite[(int)spriteType];
        _renderer.sprite = CharactorSprite[(int)spriteType];
        /*
        switch (spriteType)
        {
            case ChangeSpriteType.Wait:

                

                break;
            case ChangeSpriteType.Wait2:
                break;
            case ChangeSpriteType.Banzai:
                break;
            case ChangeSpriteType.Run:
                break;
            default:
                break;
        }
        */
    }

}
