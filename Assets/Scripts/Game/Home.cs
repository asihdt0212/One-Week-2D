using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    //待機中の人数
    private int TotalHuman = 0;
    //コンストラクタ
    public Home()
    {
        TotalHuman = 0;
    }
    //コンストラクタ、初めに入っている人数
    public Home(int FirstHumanValue)
    {
        TotalHuman = FirstHumanValue;
    }
    //人間の追加
    public void AddHuman(int AddHumanValue)
    {
        TotalHuman += AddHumanValue;
    }
    //人間が減る//Trueは無事に人間の量が減らせました。
    //false　は減らせなかった。
    public bool GainHuman(int GainHumanValue)
    {
        //減らした値が０未満はfalse;
        if ((TotalHuman - GainHumanValue) < 0)
        {
            return false;
        }
        TotalHuman -= GainHumanValue;
        return true;
    }
    //人間の数の取得
    public int GetHumanValue()
    {
        return TotalHuman;
    }
}
