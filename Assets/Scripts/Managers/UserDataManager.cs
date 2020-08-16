using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//データ管理
public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;

    public struct UserData
    {
        public bool init;   //新規作成時にtrueに変更．
        public int score;
        public int highScore;
        public int currentRound;
    }

    private UserData data = default;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        //ユーザーデータ読み込み
        LoadUserData();
    }

    private void LoadUserData()
    {
         if(data.init == false)
        {
            ResetData();
        }
    }

    public UserData GetUserData()
    {
        return data;
    }

    public void NextRound()
    {
        data.currentRound++;
    }

    //初期化
    public void ResetData()
    {
        data = new UserData();
        data.init = true;
        //初期ラウンドを１と設定
        data.currentRound = 1;
    }
}
