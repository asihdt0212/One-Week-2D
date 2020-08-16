using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//難易度選択
public class SelectMode : MonoBehaviour
{
    
   //家の中の最大人数
    public int MaxHumanValue = 9;
    //生成するキャラの数
    private int CreateMaxHumanValue = 0;
    //speed計算のに使う値
    private float CharacterSppedValue = 1.0f;

    //生成するキャラのデフォルト値
    const int DefaultCharacterValue = 5;
    //speedのデフォルト値
    const float DefaultApeedValue = 1.0f;
    //レベルが上がるごとに加算される、生成キャラ数の参照値
    const float AddCharacterValue = 0.5f;
    //レベルが上がるごとにspeedが上がる参照値
    const float AddSpeedValue = 0.25f;
   
    //基本全体のキャラクターの移動時間のデフォルト値
    //MoveTime（全体のキャラクターの移動時間）/　Sppedvalue = 移動にかかる時間で計算（全体のキャラの移動時間）を求める
    const float MoveTime= 5.0f;

    //難易度設定
    private int GameLevelValue = 1;
    //コンストラクタ
    public SelectMode()
    {
        //初期化
        SelectModeInit();
    }
    //初期化
    public void SelectModeInit()
    {
        //難易度設定
        GameLevelValue = 1;
        //何かしらの変数でレベルの初期開始レベルを変えるかも？
        //レベルの値をここで SetGameLevel(レベルの値)を入れる。

        //難易度の更新
        ModeLevelUpdate();
    }
    //レベルを上げる
    public void LevelUp()
    {
        //レベルを１あげる
        GameLevelValue++;
        //難易度更新
        ModeLevelUpdate();
    }
    //難易度選択
    public void SetGameLevel(int SetSelectLevel)
    {
        //Level設定
        GameLevelValue = SetSelectLevel;

        //難易度更新
        ModeLevelUpdate();

    }
    //難易度の更新
    public void  ModeLevelUpdate()
    {
        //キャラの生成数の更新
        CreateMaxHumanValue = DefaultCharacterValue + (int)(AddCharacterValue * GameLevelValue);
        //キャラクターの移動時間の更新
        CharacterSppedValue = MoveTime / (DefaultApeedValue + (AddSpeedValue * GameLevelValue));
    }
    //生成キャラクター数の取得
    public int GetCreateMaxHumanValue()
    {
        return CreateMaxHumanValue;
    }
    //キャラクターの移動時間の取得
    public float GetCharacterSppedValue()
    {
        return CharacterSppedValue;
    }
}
