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
        = new SoundInfo("BGM_GAME", Define.Audio_FeelingCool);
        //= new SoundInfo("BGM_GAME", Define.Audio_BGMGame);

    //カウントSE
    public static readonly SoundInfo SE_COUNT
        = new SoundInfo("SE_COUNT", Define.Audio_select01);

    //カウント最終SE
    public static readonly SoundInfo SE_COUNT_END
        = new SoundInfo("SE_COUNT_END", Define.Audio_surprise);

    //正解SE
    public static readonly SoundInfo SE_CORRECT
        = new SoundInfo("SE_CORRECT", Define.Audio_SECorrect);

    //ミスSE
    public static readonly SoundInfo SE_INCORRECT
        = new SoundInfo("SE_INCORRECT", Define.Audio_SEIncorrect);

    //スタートSE
    public static readonly SoundInfo SE_START
        = new SoundInfo("SE_START", Define.Audio_SEStart);
    //移動SE 軍隊系　(複数人)
    public static readonly SoundInfo SE_Walk
        = new SoundInfo("SE_Walk", Define.Audio_walkarmy1);
    //移動SE 2 通常　（一人）
    public static readonly SoundInfo SE_Walk2
        = new SoundInfo("SE_Walk2", Define.Audio_walkingofagiant);
}
