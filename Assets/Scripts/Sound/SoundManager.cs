﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundDefine;

public class  SoundManager : Singleton<SoundManager>
{
    
    AudioSource MyBGMAudioSource;
    AudioSource MySEAudioSource;
    //音楽データ作成用
    SoundData SoundSEData_;
    SoundData SoundBGMData_;

    public enum Kind
    {
        SE,
        BGM,
    }

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        SoundSEData_ = new SoundData();
        SoundBGMData_ = new SoundData();

        //オーディオソースを追加;
        MyBGMAudioSource = this.gameObject.AddComponent<AudioSource>();
        MySEAudioSource = this.gameObject.AddComponent<AudioSource>();

        //Loop設定
        MyBGMAudioSource.loop = true;

        //Soundのロード
        LoadSoundResource();

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
    }

    void LoadSound(SoundInfo info, Kind kind)
    {
        if(kind == Kind.BGM) { SoundBGMData_.LoadSoundData(info.path, info.key); }
        else { SoundSEData_.LoadSoundData(info.path, info.key); }
    }
    
    /// </書き足す所>
    /// 
    //SEを流す
    public void SoundSEPlay(string KeyName)
    {
        var SoundData = SoundSEData_.GetSoundData(KeyName);

        if (SoundData == null)
        {
            Debug.LogError(KeyName+"の中身が"+SoundData+"です。（SE）");
            return;
        }

        //ワンショットでならす
        MySEAudioSource.PlayOneShot(SoundData);
    }
    //BGMを流す
    public void SoundBGMPlay(string KeyName)
    {
        MyBGMAudioSource.Stop();

        var SoundData = SoundBGMData_.GetSoundData(KeyName);

        if(SoundData == null)
        {
            Debug.LogError(KeyName + "の中身が" + SoundData + "です。（BGM）");
            return;
        }
        //設定BGMの変更
        MyBGMAudioSource.clip = SoundData;
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
    

}