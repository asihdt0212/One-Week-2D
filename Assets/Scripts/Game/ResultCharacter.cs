using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCharacter : Charactor
{
    public int G_LenthSize = 0;

    List<GameObject> L_ResultCharaObj = new List<GameObject>();

    //答えオブジェクトを生成(初期化段階で実行) 引数：家に入れる人間の最大数
    public void CreateAnswerObject(int MaxhumanValue)
    {
        //初期化
        L_ResultCharaObj.Clear();

        //底辺のサイズを決める。
        int LenthSize = MaxhumanValue / 2;

        G_LenthSize = LenthSize;
        Debug.Log(LenthSize);
        //ループ回数を記録
        int ReturnCount = 0;

       

        while (LenthSize > 0)
        {
            for (int i = 0; i < LenthSize; i++)
            {
                //自分の複製を作って
                GameObject CreateObj = Instantiate(this.gameObject) as GameObject;

                //ずらしの値を生成
                Vector3 FirstOffSet = this.transform.localScale * 2;
                Vector3 OffSet = this.transform.localScale * 2;
                //大きさ分+の方へずらす

                FirstOffSet.x = OffSet.x + (ReturnCount * this.transform.localScale.x );
                OffSet.y = OffSet.y * ReturnCount;
                OffSet.z = -2.0f;

                CreateObj.transform.position += FirstOffSet + new Vector3(OffSet.x * i, OffSet.y, OffSet.z);

                //一時保存
                L_ResultCharaObj.Add(CreateObj);
            }

            LenthSize--;

            //無限ループに入らないように制限
            if (ReturnCount > 100)
            {
                Debug.LogError("Break!");
                break;
            }
            else
            {
                ReturnCount++;
            }
        }
        Debug.Log($"{ReturnCount}周しました。");
        Debug.Log($"中身の数は{L_ResultCharaObj.Count}です。");
        this.gameObject.GetComponent<SpriteRenderer>().sprite = null;

        //子データ登録
        foreach (var L_RCObj in L_ResultCharaObj)
        {
            L_RCObj.transform.parent = this.transform;
            L_RCObj.SetActive(false);
        }
    }
    //答えオブジェクトのアクティブ状態をオンにする //引数：答えオブジェクトのオンにするオブジェクト数
    public void AnswerObjectActiveOn(int AnwerValue)
    {
        if(L_ResultCharaObj.Count <= AnwerValue)
        {
            Debug.LogError("配列参照エラーです。L_ResultCharaObj.Count "+ L_ResultCharaObj.Count);
            return;
        }
        
        for(int i = 0;i < AnwerValue; i++)
        {
            L_ResultCharaObj[i].SetActive(true);
        }
    }
}
