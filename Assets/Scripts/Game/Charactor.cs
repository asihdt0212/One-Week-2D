using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour
{
    public enum ChangeSpriteType
    {
        Wait,
        Wait2,
        Banzai,
        Run,
    }
    List<Sprite> CharactorSprite;
    
    public void Init()
    {
        this.gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0);

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
            this.GetComponent<SpriteRenderer>().sprite = CharactorSprite[0];
        }
        else
        {
            Debug.Log("308 読み込み出来ませんでした");
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
