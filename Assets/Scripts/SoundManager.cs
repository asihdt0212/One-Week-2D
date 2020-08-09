using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    AudioSource MyAudioSource;
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


        MyAudioSource.PlayOneShot(SoundData);
    }
    //BGMを流す
    public void SoundBGMPlay(string KeyName)
    {
        MyAudioSource.Stop();

        var SoundData = SoundBGMData_.GetSoundData(KeyName);

        if(SoundData == null)
        {
            Debug.LogError(KeyName+"キーネームは設定されていません(BGM");
            return;
        }

        MyAudioSource.clip = SoundData;

        MyAudioSource.Play();
    }

    //音楽を流す
    public void SoundPlay()
    {
        MyAudioSource.Play();
    }

    //音楽を止める
    public void SoundStop()
    {
        MyAudioSource.Stop();
    }
}
