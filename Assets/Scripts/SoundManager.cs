using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  SoundManager : Singleton<SoundManager>
{
    
    AudioSource MyBGMAudioSource;
    AudioSource MySEAudioSource;
    //音楽データ作成用
    SoundData SoundSEData_;
    SoundData SoundBGMData_;


    // Start is called before the first frame update
    void Start()
    {
        //初期化
        SoundSEData_ = new SoundData();
        SoundBGMData_ = new SoundData();

        //オーディオソースを追加;
        MyBGMAudioSource = this.gameObject.AddComponent<AudioSource>();
        MySEAudioSource = this.gameObject.AddComponent<AudioSource>();
        //Soundのロード
        LoadSoundResource();

    }

    /// <書き足す所>

    //ロードするSoundデータを入れる。
    private void LoadSoundResource()
    {
        //記入例
        SoundSEData_.LoadSoundData("","");
        //\記入例

        SoundBGMData_.LoadSoundData("", "");

    }
    
    /// </書き足す所>
    /// 
    //SEを流す
    public void SoundSEPlay(string KeyName)
    {
        var SoundData = SoundSEData_.GetSoundData(KeyName);

        if (SoundData == null)
        {
            Debug.LogError(KeyName + "キーネームは設定されていません(SE)");
            return;
        }


        MySEAudioSource.PlayOneShot(SoundData);
    }
    //BGMを流す
    public void SoundBGMPlay(string KeyName)
    {
        MyBGMAudioSource.Stop();

        var SoundData = SoundBGMData_.GetSoundData(KeyName);

        if(SoundData == null)
        {
            Debug.LogError(KeyName+"キーネームは設定されていません(BGM");
            return;
        }

        MyBGMAudioSource.clip = SoundData;

        MyBGMAudioSource.Play();
    }
    
    //BGMを流す
    public void BGMPlay()
    {
        MyBGMAudioSource.Play();
    }
    //BGMを止める
    public void BGMStop()
    {
        MyBGMAudioSource.Stop();
    }
    //BGMをポーズ
    public void BGMPause()
    {
        MyBGMAudioSource.Pause();
    }
    //BGMをポーズ解除
    public void BGMUnPause()
    {
        MyBGMAudioSource.UnPause();
    }
    //SEのプレイ
    public void SEPlayer()
    {
        MySEAudioSource.Play();
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
