using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundDefine;

public class  SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    AudioSource MyBGMAudioSource;
    [SerializeField]
    AudioSource MySEAudioSource;

    [SerializeField]
    AudioClip[] MyBGMData_;
    [SerializeField]
    AudioClip[] MySEData_;

    //音楽データ作成用
    SoundData SoundSEData_ = new SoundData();
    SoundData SoundBGMData_ = new SoundData();

    public enum Kind
    {
        SE,
        BGM,
    }

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        SoundSEData_.Init();
        SoundBGMData_.Init();

        //オーディオソースを追加;
        //MyBGMAudioSource = this.gameObject.AddComponent<AudioSource>();
        //MySEAudioSource = this.gameObject.AddComponent<AudioSource>();

        //Loop設定
        MyBGMAudioSource.loop = true;

        //Soundのロード
        LoadSoundResource();

        Play();

    }

    /// <書き足す所>

    //ロードするSoundデータを入れる。
    private void LoadSoundResource()
    {
        //記入例
        SoundSEData_.LoadSoundData("Sound/SE/KomoriSE/anime01/anime01/anime_flying", "SE1");
        SoundSEData_.LoadSoundData(Define.Audio_1000hznoise, "SE2");
        //\記入例

        SoundBGMData_.LoadSoundData("Sound/BGM/ReoMusic/スペースコロニー", "BGM1");
        SoundSEData_.LoadSoundData(Define.Audio_地下アジト, "BGM2");


        //ゲームBGM
        LoadSound(BGM_GAME, Kind.BGM);
        //カウントSE
        LoadSound(SE_COUNT, Kind.SE);
        //ラストカウントSE
        LoadSound(SE_COUNT_END, Kind.SE);
        //正解SE
        LoadSound(SE_CORRECT, Kind.SE);
        //ミスSE
        LoadSound(SE_INCORRECT, Kind.SE);
        //スタートSE
        LoadSound(SE_START, Kind.SE);
        //歩行SEのロード
        LoadSound(SE_Walk, Kind.SE);
        //歩行SEのロード
        LoadSound(SE_Walk2, Kind.SE);
        //歩行SEのロード
        LoadSound(SE_Walk3, Kind.SE);
        //正解時歓声
        LoadSound(SE_Clear, Kind.SE);
        //解答時SE
        LoadSound(SE_Answer, Kind.SE);
    }

    void LoadSound(SoundInfo info, Kind kind)
    {
        if(kind == Kind.BGM) { SoundBGMData_.LoadSoundData(info.path, info.key); }
        else { SoundSEData_.LoadSoundData(info.path, info.key); }
    }
    public void SoundSEPlay(int number)
    {
        //ワンショットでならす
        MySEAudioSource.PlayOneShot(MySEData_[number]);
    }
    public void SoundBGMPlay(int number)
    {
        MyBGMAudioSource.Stop();
        //設定BGMの変更
        MyBGMAudioSource.clip = MyBGMData_[number];
        //BGMを流す
        MyBGMAudioSource.Play();
    }
    /// </書き足す所>
    /// 
    //SEを流す
    public void SoundSEPlay(string KeyName)
    {
        var SoundData_ = SoundSEData_.GetSoundData(KeyName);

        if (SoundData_ == null)
        {
            Debug.LogError(KeyName+"の中身が"+SoundData_+"です。（SE）");
            return;
        }

        //ワンショットでならす
        MySEAudioSource.PlayOneShot(SoundData_);
    }
    //BGMを流す
    public void SoundBGMPlay(string KeyName)
    {
        MyBGMAudioSource.Stop();

        var SoundData_ = SoundBGMData_.GetSoundData(KeyName);

        if(SoundData_ == null)
        {
            Debug.LogError(KeyName + "の中身が" + SoundData_ + "です。（BGM）");
            return;
        }
        //設定BGMの変更
        MyBGMAudioSource.clip = SoundData_;
        //BGMを流す
        MyBGMAudioSource.Play();
    }
    
    //BGMを流す
    public void Play()
    {
        MyBGMAudioSource.Play();
    }
    //BGMを止める
    public void Stop()
    {
        MyBGMAudioSource.Stop();
    }
    //BGMをポーズ
    public void Pause()
    {
        MyBGMAudioSource.Pause();
    }
    //BGMをポーズ解除
    public void UnPause()
    {
        MyBGMAudioSource.UnPause();
    }
    //SEの止める
    public void SEStop()
    {
        MySEAudioSource.Stop();
    }
    //BGMのボリューム変更// 0～１
    public void BGMVolumeChange(float value)
    {
        //0～1超えないようにする。
        float value_ = Mathf.Clamp(value, 0, 1);
        //ボリューム設定
        MyBGMAudioSource.volume = value_;
    }
    //SEのボリューム変更// 0～1
    public void SEVolumeChange(float value)
    {
        //0～1超えないようにする。
        float value_ = Mathf.Clamp(value, 0, 1);
        //ボリューム設定
        MySEAudioSource.volume = value_;
    }
    //再生速度の変更
    public void ChangeSESoundSpeed(float PitchValue)
    {
        //範囲外にならないように
        var P_Value = Mathf.Clamp(PitchValue, -3.0f, 3.0f);
        //変更
        MySEAudioSource.pitch = P_Value;
    }
    //再生速度の変更2 //移動時間のからピッチ速度を割り出す。
    public void ChangeSESoundSpeed2(float MoveTime)
    {
        float P_Value = MoveTime / SelectMode.MoveTime;
        //範囲外にならないように
        
        //変更
        MySEAudioSource.pitch =0.70f + (1.0f -  P_Value);
    }
    //再生速度のリセット
    public void SESoundSpeedReset()
    {
        const int ResetPitch = 1;
        MySEAudioSource.pitch = ResetPitch;
    }


    //音量設定
    public void ChangeVolume(Kind kind, float volume)
    {
        if (kind == Kind.BGM) { MyBGMAudioSource.volume = volume; }
        else  { MySEAudioSource.volume = volume; }
    }

}
