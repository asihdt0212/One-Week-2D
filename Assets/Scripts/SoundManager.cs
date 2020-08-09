using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  SoundManager : Singleton<SoundManager>
{
    [SerializeField]
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
    public void BGMVolumeChange()
    {

    }

}
