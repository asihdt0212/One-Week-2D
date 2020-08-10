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

    //移動中、待機中
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
    public Transform Mytransform;

    public override void Move()
    {
        Mytransform.DOMove(new Vector3(0, 10f, 0), 10.0f);

        Debug.Log("Move実行");
    }
}
[CreateAssetMenu(menuName = "Pattern/MovePattern2")]
public class MovePattern2 : Pattern
{

    public Transform Mytransform;

    public float FirstWaitTime; 

    public override void Move()
    {
        Mytransform.DOMove(new Vector3(0, -10f,0 ), 10.0f);

        Debug.Log("Move2実行");
    }
}
