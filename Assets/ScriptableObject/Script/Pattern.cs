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
    [Range(1, 5)]
    public int People = 1;

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
public class MovePatternLeft : Pattern
{
    //動かすために必要
    public Transform Mytransform;
    //最初の待機時間？
    public float FirstWaitTime = 0.0f;
    //目指す移動位置
    public GameObject TargetObject;
    //移動時間
    public float MoveTime = 5.0f;
    //移動出現方向
    public Angle PatternType_ = Angle.Up;
    Vector3 MoveAngle_ = Vector3.zero;
    //ターゲットから離れている長さ
    public float LenthValue = 8;
    //移動前の初期化 //動かすオブジェクトの位置データ参照データを引数。
    public void MoveAngleInit(Transform transform)
    {
        
        //オブジェクトのTransform参照データの受け渡し。
        Mytransform = transform;
        //オブジェクト位置を変える
        Mytransform.position = TargetObject.transform.position;

        Debug.Log(Mytransform.position);
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
            /*
            //Valueが超えた時は移動方向の種類分減らす
            if(Value > MoveAngle.Count - 1)
            {

                Debug.Log(Value + ">"+  (MoveAngle.Count - 1));
                //超えた分
                Value = Value - (MoveAngle.Count);
            }
            */
            //Debug.Log("PatternType_" + PatternType_ + " "+ Value);
            //値の入れ物
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
          

            Debug.Log(FirstPosi);
            //オブジェクトの位置を変える
            Mytransform.position = TargetObject.transform.position + (FirstPosi* LenthValue);
            //Debug.Log(Mytransform.position);
        }
        else
        {
            //ターゲット（家）の位置を入れる。
            Mytransform.position = TargetObject.transform.position;
        }
    }
    //入る方向Left設定
    public void HomeInInit()
    {
        //Left  or Down
        PatternType_ = Angle.Left;

        m_MoveFlag = false;

        ActiveMove_ = ActiveMove.Move;
    }
    //出るほうの処理を行います。　引数移動方向
    public void HomeOutInit(Home home_data)
    {
        //Right or Up
        if (!home_data.GainHuman(People))
        {
            Debug.Log("キャラクター生成できませんでした。");
            ActiveMove_ = ActiveMove.Wait;
            return;
        }
        //Right or Up
        PatternType_ = Angle.Right;

        PatternType_ = Angle.Up;

        m_MoveFlag = true;

        ActiveMove_ = ActiveMove.Move;
    }

    //Pattern１
    public override void Move()
    {
        //入るほうの処理
        if (!m_MoveFlag)
        {
            Mytransform.DOMove(new Vector3(TargetObject.transform.position.x, TargetObject.transform.position.y, Mytransform.position.z), MoveTime)
                   .OnComplete(() => End());
        }
        //出る方の処理
        else
        {
            Mytransform.DOMove(new Vector3(Mytransform.position.x + MoveAngle_.x, Mytransform.position.y + MoveAngle_.y, Mytransform.position.z + MoveAngle_.z), MoveTime)
                   .OnComplete(() => End());
        }
        Debug.Log("Move実行");
    }
    
    //Pattern2 移動開始まで時間待つ
    public override void Move2()
    {

        Mytransform.DOMove(new Vector3(Mytransform.position.x + MoveAngle_.x, Mytransform.position.y + MoveAngle_.y, Mytransform.position.z + MoveAngle_.z), MoveTime)
            .SetDelay(1f)
            .OnComplete(() => End() );

        Debug.Log("Move2実行");
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
//単純な出入りのパターン
[CreateAssetMenu(menuName = "Pattern/MovePattern")]
public class MovePatternRight : Pattern
{
    //動かすために必要
    public Transform Mytransform;
    //最初の待機時間？
    public float FirstWaitTime = 0.0f;
    //目指す移動位置
    public GameObject TargetObject;
    //移動時間
    public float MoveTime = 5.0f;
    //移動出現方向
    public Angle PatternType_ = Angle.Up;
    Vector3 MoveAngle_ = Vector3.zero;
    //ターゲットから離れている長さ
    public float LenthValue = 8;
    //移動前の初期化 //動かすオブジェクトの位置データ参照データを引数。
    public void MoveAngleInit(Transform transform)
    {

        //オブジェクトのTransform参照データの受け渡し。
        Mytransform = transform;
        //オブジェクト位置を変える
        Mytransform.position = TargetObject.transform.position;

        Debug.Log(Mytransform.position);
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
            /*
            //Valueが超えた時は移動方向の種類分減らす
            if(Value > MoveAngle.Count - 1)
            {

                Debug.Log(Value + ">"+  (MoveAngle.Count - 1));
                //超えた分
                Value = Value - (MoveAngle.Count);
            }
            */
            //Debug.Log("PatternType_" + PatternType_ + " "+ Value);
            //値の入れ物
            Vector3 FirstPosi = Vector3.zero;


            foreach (KeyValuePair<Angle, Vector3> M_Angle in MoveAngle)
            {
                //同じキーがあるか検索
                if ((int)M_Angle.Key == Value)
                {
                    //入れ物に、仮の場所データを入れる
                    FirstPosi = M_Angle.Value;
                    Debug.Log(M_Angle.Key + "発見" + FirstPosi);
                }
            }


            Debug.Log(FirstPosi);
            //オブジェクトの位置を変える
            Mytransform.position = TargetObject.transform.position + (FirstPosi * LenthValue);
            //Debug.Log(Mytransform.position);
        }
        else
        {
            //ターゲット（家）の位置を入れる。
            Mytransform.position = TargetObject.transform.position;
        }
    }
    //入る方向Left設定
    public void HomeInInit()
    {
        //Left  or Down
        PatternType_ = Angle.Right;

        m_MoveFlag = false;

        ActiveMove_ = ActiveMove.Move;
    }
    //出るほうの処理を行います。　引数移動方向
    public void HomeOutInit(Home home_data)
    {
        //Right or Up
        if (!home_data.GainHuman(People))
        {
            Debug.Log("キャラクター生成できませんでした。");
            ActiveMove_ = ActiveMove.Wait;
            return;
        }
        //Right or Up
        PatternType_ = Angle.Left;

        m_MoveFlag = true;

        ActiveMove_ = ActiveMove.Move;
    }

    //Pattern１
    public override void Move()
    {
        //入るほうの処理
        if (!m_MoveFlag)
        {
            Mytransform.DOMove(new Vector3(TargetObject.transform.position.x, TargetObject.transform.position.y, Mytransform.position.z), MoveTime)
                   .OnComplete(() => End());
        }
        //出る方の処理
        else
        {
            Mytransform.DOMove(new Vector3(Mytransform.position.x + MoveAngle_.x, Mytransform.position.y + MoveAngle_.y, Mytransform.position.z + MoveAngle_.z), MoveTime)
                   .OnComplete(() => End());
        }
        Debug.Log("Move実行");
    }

    //Pattern2 移動開始まで時間待つ
    public override void Move2()
    {

        Mytransform.DOMove(new Vector3(Mytransform.position.x + MoveAngle_.x, Mytransform.position.y + MoveAngle_.y, Mytransform.position.z + MoveAngle_.z), MoveTime)
            .SetDelay(1f)
            .OnComplete(() => End());

        Debug.Log("Move2実行");
    }
    //終了処理
    public void End()
    {
        if (ActiveMove_ == ActiveMove.Move)
        {
            //終了処理後
            ActiveMove_ = ActiveMove.End;

        }

    }
}
//単純な出入りのパターン
[CreateAssetMenu(menuName = "Pattern/MovePattern")]
public class MovePatternUp : Pattern
{
    //動かすために必要
    public Transform Mytransform;
    //最初の待機時間？
    public float FirstWaitTime = 0.0f;
    //目指す移動位置
    public GameObject TargetObject;
    //移動時間
    public float MoveTime = 5.0f;
    //移動出現方向
    public Angle PatternType_ = Angle.Up;
    Vector3 MoveAngle_ = Vector3.zero;
    //ターゲットから離れている長さ
    public float LenthValue = 8;
    //移動前の初期化 //動かすオブジェクトの位置データ参照データを引数。
    public void MoveAngleInit(Transform transform)
    {

        //オブジェクトのTransform参照データの受け渡し。
        Mytransform = transform;
        //オブジェクト位置を変える
        Mytransform.position = TargetObject.transform.position;

        Debug.Log(Mytransform.position);
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
            /*
            //Valueが超えた時は移動方向の種類分減らす
            if(Value > MoveAngle.Count - 1)
            {

                Debug.Log(Value + ">"+  (MoveAngle.Count - 1));
                //超えた分
                Value = Value - (MoveAngle.Count);
            }
            */
            //Debug.Log("PatternType_" + PatternType_ + " "+ Value);
            //値の入れ物
            Vector3 FirstPosi = Vector3.zero;


            foreach (KeyValuePair<Angle, Vector3> M_Angle in MoveAngle)
            {
                //同じキーがあるか検索
                if ((int)M_Angle.Key == Value)
                {
                    //入れ物に、仮の場所データを入れる
                    FirstPosi = M_Angle.Value;
                    Debug.Log(M_Angle.Key + "発見" + FirstPosi);
                }
            }


            Debug.Log(FirstPosi);
            //オブジェクトの位置を変える
            Mytransform.position = TargetObject.transform.position + (FirstPosi * LenthValue);
            //Debug.Log(Mytransform.position);
        }
        else
        {
            //ターゲット（家）の位置を入れる。
            Mytransform.position = TargetObject.transform.position;
        }
    }
    //入る方向Left設定
    public void HomeInInit()
    {
     
       
        PatternType_ = Angle.Down;
        //入るflagに
        m_MoveFlag = false;

        ActiveMove_ = ActiveMove.Move;
    }
    //出るほうの処理を行います。　引数移動方向
    public void HomeOutInit(Home home_data)
    {
        //Right or Up
        if (!home_data.GainHuman(People))
        {
            Debug.Log("キャラクター生成できませんでした。");
            ActiveMove_ = ActiveMove.Wait;
            return;
        }

        PatternType_ = Angle.Up;
        //出るフラグに
        m_MoveFlag = true;

        ActiveMove_ = ActiveMove.Move;
    }

    //Pattern１
    public override void Move()
    {
        //入るほうの処理
        if (!m_MoveFlag)
        {
            Mytransform.DOMove(new Vector3(TargetObject.transform.position.x, TargetObject.transform.position.y, Mytransform.position.z), MoveTime)
                   .OnComplete(() => End());
        }
        //出る方の処理
        else
        {
            Mytransform.DOMove(new Vector3(Mytransform.position.x + MoveAngle_.x, Mytransform.position.y + MoveAngle_.y, Mytransform.position.z + MoveAngle_.z), MoveTime)
                   .OnComplete(() => End());
        }
        Debug.Log("Move実行");
    }

    //Pattern2 移動開始まで時間待つ
    public override void Move2()
    {

        Mytransform.DOMove(new Vector3(Mytransform.position.x + MoveAngle_.x, Mytransform.position.y + MoveAngle_.y, Mytransform.position.z + MoveAngle_.z), MoveTime)
            .SetDelay(1f)
            .OnComplete(() => End());

        Debug.Log("Move2実行");
    }
    //終了処理
    public void End()
    {
        if (ActiveMove_ == ActiveMove.Move)
        {
            //終了処理後
            ActiveMove_ = ActiveMove.End;

        }

    }
}
