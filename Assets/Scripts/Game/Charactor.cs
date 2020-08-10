using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour
{
    List<Sprite> CharactorSprite;
    
    public void Init()
    {
        CharactorSprite = new List<Sprite>();
       //画像データ読みこみ
       Sprite[] LoadSprite = Resources.LoadAll<Sprite>("308");

        if (LoadSprite != null)
        {
            Debug.Log("308 読み込み完了");
            Debug.Log("画像数" + LoadSprite.Length);
            //画像の追加
            for(int i = 0;i < LoadSprite.Length; i++)
            {
                CharactorSprite.Add(LoadSprite[i]);
            }
                
            
        }
        else
        {
            Debug.Log("308 読み込み出来ませんでした");
        }
    }

}
