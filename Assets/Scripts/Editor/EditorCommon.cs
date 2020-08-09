using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

//エディタ拡張用汎用処理クラス
public static class EditorCommon
{
    public static bool CanCreate()
    {
        //シーン実行中であったり，コンパイル中であったりするならば作成させないようにする
        return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
    }

    //無効文字
    static readonly string[] INVALID_CHARS =
    {
            " ",  "!",  "\"",   "#",    "$",
            "%",  "&", "\'", "(", ")",
            "_", "=", "^", "~",  "\\",
             "|", "[",  "]",  "{", "}",
            "@", "`", ":", "*", ";",
             "+", "/", "?", ".", "<",
              ">", ",",
        };

    //無効文字を除く文字列を取得
    public static string GetStrWithoutInvalidChars(string str, List<string> exceptionList = null)
    {
        List<string> removeChars = INVALID_CHARS.ToList();
        if (exceptionList != null)
        {
            exceptionList.ForEach(element => { removeChars.Remove(element); });
        }
        removeChars.ForEach(item => str = str.Replace(item, string.Empty));
        return str;
    }
}
