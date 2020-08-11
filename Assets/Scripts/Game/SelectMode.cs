using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//難易度選択
public class SelectMode : MonoBehaviour
{
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
    public int MaxHumanValue = 9;
    //難易度設定
    DifficultyLevel DifficultyLevel_ = DifficultyLevel.Level1;

    public SelectMode()
    {   //難易度設定
        DifficultyLevel_ = DifficultyLevel.Level1;
    }
    //難易度選択
    public SelectMode(DifficultyLevel difficulty)
    {
        DifficultyLevel_ = difficulty;
    }
}
