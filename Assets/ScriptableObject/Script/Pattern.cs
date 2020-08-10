using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public abstract class Pattern : ScriptableObject
{
    public enum Angle
    {
        Up,
        RightUp,
        Right,
        RightDown,
        Down,
        LeftDown,
        Left,
        LeftUp,
    }
    //出現タイミング
    float Time = 1.0f;
    //グループ人数
    [Range(1, 5)]
    public int People = 1;

    //移動しきるか待機しきるか
    public bool m_MoveFlag = false;
    //移動出現方向
    Angle PatternType_ = Angle.Up;


    public abstract void Move();
 
}
public static class PatternExtension
{
    public static void Init(this Pattern behaviour)
    {
        Debug.Log("Initialize");
    }
}
[CreateAssetMenu(menuName = "Pattern/MovePattern")]
public class MovePattern : Pattern
{
    public RectTransform rectTran;

    public override void Move()
    {
        Debug.Log("Move実行");
    }
}
[CreateAssetMenu(menuName = "Pattern/MovePattern2")]
public class MovePattern2 : Pattern
{
    public RectTransform rectTran;

    public override void Move()
    {
        Debug.Log("Move2実行");
    }
}
