using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public abstract class Pattern : ScriptableObject
{
    //キャラクター状態 
    public enum ActiveMove {
        Wait,//待機（出現していない）
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


    //グループ人数
    [Range(1, 1)]
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
    private float MoveTime = 5.0f;
    //移動出現方向
    private Angle PatternType_ = Angle.Up;
    private Vector3 MoveAngle_ = Vector3.zero;
    //ターゲットから離れている長さ
    const float LenthValue = 4.0f;
    //何番目か
    private int MyNumber = 0;
    public MovePattern(int Number,Transform Transform ,GameObject TargetObj)
    {
        //自分の番号
        MyNumber = Number;

        //移動させるTransformデータ
        Mytransform = Transform;

        //家データ
        TargetObject = TargetObj;
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
            Mytransform.position = TargetObject.transform.position + (FirstPosi* LenthValue);
  
        }
        else
        {
            //ターゲット（家）の位置を入れる。
            Mytransform.position = TargetObject.transform.position;
        }
    }
    //入る方向Left設定
    public void RondomHomeInInit()
    {
        Debug.Log("ランダムIN生成");
        //乱数でLeftかUpを決める
        float RandomValue = Random.Range(0f, 1f)*10;

        //Left Upの選択
        if(RandomValue > 5)
        {
            PatternType_ = Angle.Right;
        }
        else if(RandomValue < 5)
        {
            PatternType_ = Angle.Down;
        }
        else
        {
            //再帰でももう一で設定する。
            RondomHomeInInit();
            return;
        }
        //入る選択
        m_MoveFlag = false;
        //アクティブ状態を移動に変更
        ActiveMove_ = ActiveMove.Move;
    }
    //出るほうの処理を行います。　引数移動方向
    public void RondomHomeOutInit()
    {
        Debug.Log("ランダムOUT生成");


        //生成できるかチェック。
        if (!CharactorManager.Instance.GetHome().GainHuman(Human))
        {
            Debug.Log("キャラクター生成できませんでした。");
            //生成できなかったので入るほうを作成
            RondomHomeInInit();
            return;
        }

        //乱数でLeftかUpを決める
        float RandomValue = Random.Range(0f, 1f) * 10;

        //Left Upの選択
        if (RandomValue > 5)
        {
            PatternType_ = Angle.Right;
        }
        else if (RandomValue < 5)
        {
            PatternType_ = Angle.Down;
        }
        else
        {
            //再帰でももう一で設定する。
            RondomHomeOutInit();
            return;
        }
 
        //出る方
        m_MoveFlag = true;
        //アクティブ状態を移動に変更
        ActiveMove_ = ActiveMove.Move;
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
    //Pattern１
    public override void Move()
    {
        //入るほうの処理
        if (!m_MoveFlag)
        {
            Mytransform.DOMove(new Vector3(TargetObject.transform.position.x, TargetObject.transform.position.y, Mytransform.position.z), MoveTime)
                .SetDelay(MyNumber*3)
                .OnComplete(() => End());
        }
        //出る方の処理
        else
        {
            Mytransform.DOMove(new Vector3(Mytransform.position.x + MoveAngle_.x, Mytransform.position.y + MoveAngle_.y, Mytransform.position.z + MoveAngle_.z), MoveTime)
                .SetDelay(MyNumber * 3)
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
