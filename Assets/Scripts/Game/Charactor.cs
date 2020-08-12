using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            this.GetComponent<SpriteRenderer>().sprite = CharactorSprite[0];
        }
        else
        {
            Debug.Log("308 読み込み出来ませんでした");
        }
    }
    //人数分画像を生成。
    public void CreateCharacter(int CreateHumanvalue)
    {
        for (int i = 1; i < CreateHumanvalue; i++)
        {
            Debug.Log("実行");
            //自分の複製を作って
            GameObject CreateObj = Instantiate(this.gameObject) as GameObject;

            //ずらしの値を生成
            Vector3 OffSet = this.transform.localScale*2;
            OffSet.y = 0.0f;

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

            //子データになる。
            CreateObj.transform.parent = this.transform;

        }
    }
    public void ChangeSprite(ChangeSpriteType spriteType)
    {
        this.GetComponent<SpriteRenderer>().sprite = CharactorSprite[(int)spriteType];
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
