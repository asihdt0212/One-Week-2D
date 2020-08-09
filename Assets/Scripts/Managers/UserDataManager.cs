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
            data = new UserData();
            data.init = true;
        }
    }

    public UserData GetUserData()
    {
        return data;
    }
}
