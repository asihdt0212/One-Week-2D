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

    }
    public int GetHumanValue()
    {
        return TotalHuman;
    }
}
