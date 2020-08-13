using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public abstract class Pattern : ScriptableObject
{
    //キャラクター状態 
    public enum ActiveMove {
        Wait,//待機（出現していない）
        MoveCheck,//出る移動ができるかのチェック
        Move,//移動中
        End,//移動完了時（完了後消す）
    }

    //移動方向
    public enum Angle
    {
        Up = 0,
        RightUp = 1,
        Right = 2,
        RightDown = 3,
        Down = 4,
        LeftDown = 5,
        Left = 6,
        LeftUp = 7,
    }
    //移動方向と、それに対する、移動ベクトル値
    public Dictionary<Angle, Vector3> MoveAngle = new Dictionary<Angle, Vector3>()
    {
        { Angle.Up,new Vector3(0,1,0) },
        { Angle.RightUp,new Vector3(1,1,0) },
        { Angle.Right,new Vector3(1,0,0) },
        { Angle.RightDown,new Vector3(1,-1,0) },
        { Angle.Down,new Vector3(0,-1,0) },
        { Angle.LeftDown,new Vector3(-1,-1,0) },
        { Angle.Left,new Vector3(-1,0,0) },
        { Angle.LeftUp,new Vector3(-1,1,0) },
    };



    public int Human = 1;

    //出現から移動 false、待機から移動消える true
    public bool m_MoveFlag = false;

    //キャラクターの状態
    protected ActiveMove ActiveMove_ = ActiveMove.Wait;

    //キャラクターの状態を返す。
    public ActiveMove GetActiveMove()
    {
        return ActiveMove_;
    }
    //状態の設定
    public void SetAcitveMove(ActiveMove  active)
    {
        ActiveMove_ = active;
    }

    public abstract void Move();

    public abstract void Move2();

}
public static class PatternExtension
{
    public static void Init(this Pattern behaviour)
    {
        Debug.Log("Initialize");
    }
}
//単純な出入りのパターン
[CreateAssetMenu(menuName = "Pattern/MovePattern")]
public class MovePattern : Pattern
{
    //動かすために必要
    private Transform Mytransform;
    //最初の待機時間？
    private float FirstWaitTime = 0.0f;
    //目指す移動位置
    private GameObject TargetObject;
    //移動時間
    private float MoveTime = 2.0f;
    //移動出現方向
    private Angle PatternType_ = Angle.Up;
    private Vector3 MoveAngle_ = Vector3.zero;
    //ターゲットから離れている長さ
    const float LengthValue = 10.0f;
    //何番目か
    private int MyNumber = 0;
    public MovePattern(int Number,Transform Transform ,GameObject TargetObj,float SetMoveTime)
    {
        //自分の番号
        MyNumber = Number;

        //移動させるTransformデータ
        Mytransform = Transform;

        //家データ
        TargetObject = TargetObj;

        //移動時間
        Debug.Log(SetMoveTime);
        MoveTime =  SetMoveTime;

        //
        MyNumber = (int)(MyNumber * SetMoveTime);
    }

    //移動前の初期化 //動かすオブジェクトの位置データ参照データを引数。
    public void MoveAngleInit()
    {
        //移動ベクトル初期化
        MoveAngle_ = Vector3.zero;

        foreach (KeyValuePair<Angle, Vector3> M_Angle in MoveAngle)
        {
            //キーが同じデータ
            if (PatternType_ == M_Angle.Key)
            {
                //移動ベクトルを入れる
                MoveAngle_ = M_Angle.Value;
                //Debug.Log(PatternType_ +" "+M_Angle.Value);
            }

        }

        //初期位置設定
        if (!m_MoveFlag)
        {
            ////逆側の位置を求めに行く
            //逆方向は+-4した値
            int Value = ((int)PatternType_ + 4 > MoveAngle.Count - 1) ? (int)PatternType_ + 4 - (MoveAngle.Count) : (int)PatternType_ + 4;

            Vector3 FirstPosi = Vector3.zero;

            foreach (KeyValuePair<Angle, Vector3> M_Angle in MoveAngle)
            {
                //同じキーがあるか検索
                if((int)M_Angle.Key == Value)
                {
                    //入れ物に、仮の場所データを入れる
                    FirstPosi = M_Angle.Value;
                    Debug.Log(M_Angle.Key+ "発見" + FirstPosi);
                }
            }

            //オブジェクトの位置を変える
            Mytransform.position = GetObjPosition(TargetObject.transform.position, FirstPosi, LengthValue);
            //Mytransform.position = TargetObject.transform.position + (FirstPosi* LengthValue);
  
        }
        else
        {
            //ターゲット（家）の位置を入れる。
            Mytransform.position = TargetObject.transform.position;
        }
    }

    //ベースの座標から調整した座標を返す
    private Vector3 GetObjPosition(Vector3 basePos, Vector3 firstPosi, float lengthValue)
    {
        return basePos + (firstPosi * lengthValue);
    }

    //入る方向Left設定
    public void RondomHomeInInit()
    {
        Debug.Log("ランダムIN生成");
        //乱数でLeftかUpを決める
        //float RandomValue = Random.Range(0f, 1f)*10;
        int RandomValue = Random.Range(0, 10);
        //Left Downの選択
        //if(RandomValue > 5)
        if(RandomValue %2 == 0)
        {
            PatternType_ = Angle.Right;
        }
        //else if(RandomValue < 5)
        else
        {
            PatternType_ = Angle.Down;
        }
        //else
        //{
        //    //再帰でももう一で設定する。
        //    RondomHomeInInit();
        //    return;
        //}
        //入る選択
        m_MoveFlag = false;
        //アクティブ状態を移動に変更
        ActiveMove_ = ActiveMove.MoveCheck;
    }
    //出るほうの処理を行います。　引数移動方向
    public void RondomHomeOutInit()
    {
        Debug.Log("ランダムOUT生成");

        //乱数でLeftかUpを決める
        //float RandomValue = Random.Range(0f, 1f) * 10;
        int RandomValue = Random.Range(0, 10);
        //Left Upの選択
        //if (RandomValue > 5)
        if(RandomValue %2 == 0)
        {
            PatternType_ = Angle.Right;
        }
        //else if (RandomValue < 5)
        else
        {
            PatternType_ = Angle.Up;
        }
        //else
        //{
        //    //再帰でももう一で設定する。
        //    RondomHomeOutInit();
        //    return;
        //}
 
        //出る方
        m_MoveFlag = true;
        //アクティブ状態を移動Checkモードに変更
        ActiveMove_ = ActiveMove.MoveCheck;
    }
    //
    public void HomeSelectInInit()
    {
        RondomHomeInInit();
    }
    public void HomeSelectOutInit()
    {
        RondomHomeOutInit();
    }
    //家に人がいるかいないか　//
    public void HouseHumanCheck(Home home)
    {
        //出ていくフラグの時
        if (m_MoveFlag == true)
        {
            //入っている人間が　0人以下
            if(home.GetHumanValue() - Human <= 0)
            {

                //入っていく方の動きに変更
                RondomHomeInInit();
                //移動位置の変更
                MoveAngleInit();

            }
            else
            {
                //人間の数文減らす
                home.GainHuman(Human);

                ActiveMove_ = ActiveMove.Move;
            }
        }
    }
    //Pattern１
    public override void Move()
    {
        
        //入るほうの処理
        if (!m_MoveFlag)
        {
            Mytransform.DOMove(endValue: new Vector3(TargetObject.transform.position.x, TargetObject.transform.position.y, Mytransform.position.z), duration: MoveTime)
                .SetEase(Ease.Linear)
                .OnComplete(() => End());
        }
        //出る方の処理
        else
        {
            Mytransform.DOMove(endValue: new Vector3(Mytransform.position.x + (MoveAngle_.x * LengthValue), Mytransform.position.y + (MoveAngle_.y * LengthValue), Mytransform.position.z + (MoveAngle_.z * LengthValue)), duration: MoveTime)
                .SetEase(Ease.Linear)
                .OnComplete(() => End());
                
        }
        //Debug.Log("Move実行");
    }
    
    //Pattern2 移動開始まで時間待つ
    public override void Move2()
    {

        Mytransform.DOMove(new Vector3(Mytransform.position.x + MoveAngle_.x, Mytransform.position.y + MoveAngle_.y, Mytransform.position.z + MoveAngle_.z), MoveTime)
            .SetDelay(1f)
            .OnComplete(() => End() );

        //Debug.Log("Move2実行");
    }
    //自分の番号
    public int GetMyNumber()
    {
        return MyNumber;
    }
    //終了処理
    public void End()
    {
        if(ActiveMove_ == ActiveMove.Move)
        {
            //終了処理後
            ActiveMove_ = ActiveMove.End;

        }
    }
}
