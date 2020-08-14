using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundDefine
{
    public struct SoundInfo
    {
        public string key;
        public string path;

        public SoundInfo(string k, string p)
        {
            key = k;
            path = p;
        }
    }


    //これより以下は使用するサウンドについて定義
    //ゲーム中BGM
    public static readonly SoundInfo BGM_GAME
        = new SoundInfo("BGM_GAME", Define.Audio_BGMGame);

    //カウントSE
    public static readonly SoundInfo SE_COUNT
        = new SoundInfo("SE_COUNT", Define.Audio_select01);

    //カウント最終SE
    public static readonly SoundInfo SE_COUNT_END
        = new SoundInfo("SE_COUNT_END", Define.Audio_surprise);
}
