using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //BGM
        SoundManager.Instance.SoundBGMPlay(0);
        //データ初期化
        UserDataManager.instance.ResetData();
    }
}
