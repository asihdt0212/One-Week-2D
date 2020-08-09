using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//Sound関係のデータの格納データ型
public class SoundData
{
    /*
    //BGM格納先
    List<AudioClip> SoundData_;
    //BGMのキーネーム
    List<string> SoundKeyName;
    */
    //検索キー　と、Soundデータのリストデータ
    Dictionary<string, AudioClip> SoundData_;
   
    
    //コンストラクタ
    public SoundData()
    {
        //リストデータの初期化
        SoundData_ = new Dictionary<string, AudioClip>();
    }

    //Soundのロード。引数は読みこみたいデータのファイルパス。呼び出すときのキーの名前
    public void LoadSoundData(string FilePass,string KeyName)
    {
        //同じキーがないかチェック
        foreach (KeyValuePair<string, AudioClip> SoundKeyItem in SoundData_)
        {
            if (SoundKeyItem.Key == KeyName)
            {
                Debug.LogError("同じキータイプがあります。");
                return;
            }
        }
        var SoundAudio = Resources.Load<AudioClip>(FilePass) as AudioClip;

        if(SoundAudio == null)
        {
            Debug.LogError(FilePass + "NULLError");
        }

        SoundData_.Add(KeyName, SoundAudio);

        Debug.Log("作成しました。("+KeyName+")");

    }
    //Soundデータの取得
    public AudioClip GetSoundData(string KeyName)
    {
        foreach (KeyValuePair<string, AudioClip> SoundKeyItem in SoundData_)
        {
            if(SoundKeyItem.Key == KeyName)
            {
                Debug.Log("見つけた");
                return SoundKeyItem.Value;
            }
        }
        Debug.Log("中身ない");
        return null;
    }
}
