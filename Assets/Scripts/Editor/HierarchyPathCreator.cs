using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using static EditorCommon;

public static class HierarchyPathCreator
{
    const string MENU_ITEM_NAME = "Tools/ヒエラルキーパスクリエーター";
    //ファイルパス{シーン名}
    const string FORMAT_FILE_PATH = "Assets/Scripts/Defines/HierarchyPath_{0}.cs";
    //ファイル名{シーン名}
    const string FILE_NAME = "HierarchyPath_{0}";

    [MenuItem(MENU_ITEM_NAME)]
    public static void Create()
    {
        if (CanCreate() == true)
        {
            CreateDefineFile();
        }

        EditorUtility.DisplayDialog(string.Format(FORMAT_FILE_PATH, "PathCS"), "作成完了", "OK"); ;
    }


    public static void CreateDefineFile()
    {
        //現在のシーン名
        string sceneName = SceneManager.GetActiveScene().name;

        var builder = new StringBuilder();
        builder.Append("public static class ").AppendFormat(FILE_NAME, sceneName).AppendLine();
        builder.AppendLine("{");


        //構造体にまとめられるか
        bool canBeStruct = false;
        Transform beforeTrans = null;
        Transform checkTrans = null;

        string lastStructName = default;
        string nowStructName = default;

        foreach (var obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (var _obj in obj.GetComponentsInChildren<Transform>(true))
            {
                beforeTrans = (beforeTrans == null) ? _obj : beforeTrans;
                checkTrans = (beforeTrans == _obj) ? null : _obj;
                var rootName = GetRootObjName(_obj.gameObject);
                lastStructName = nowStructName;
                nowStructName = rootName;

                //構造体定義開始のため、一つ前の構造体を作りかけていたら閉じる
                if (lastStructName != nowStructName && lastStructName != default)
                {
                    builder.AppendLine("\t}");
                }
                canBeStruct = (beforeTrans == checkTrans) ? true : false;

                if (canBeStruct == false)
                {

                    if (nowStructName != lastStructName)
                    {
                        //新しく構造体定義を行う
                        builder.AppendLine($"\tpublic struct {rootName}");
                        builder.AppendLine("\t{");
                    }
                    var _path = GetHierarchyPath(_obj.gameObject);
                    var defineName = GetDefineName(_path);
                    //ルートのみRootと名付ける（構造体と同名は定義できない）,それ以外については構造体になっている親の名前は取り除く
                    defineName = (defineName == nowStructName) ? "Root" : defineName.Remove(0, nowStructName.Count());
                    builder.AppendLine($"\t\tpublic const string {defineName} = \"{_path}\";");
                }
                else
                {
                    //作成中の構造体を,このオブジェクトを最後にして完成させる
                    var _path = GetHierarchyPath(_obj.gameObject);
                    var defineName = GetDefineName(_path);
                    //ルートのみRootと名付ける（構造体と同名は定義できない）
                    defineName = (defineName == nowStructName) ? "Root" : defineName.Remove(0, nowStructName.Count());
                    builder.AppendLine($"\t\tpublic const string {defineName} = \"{_path}\";");
                    builder.AppendLine("\t}");
                }
            }
        }

        builder.AppendLine("\t}");
        builder.AppendLine("}");

        var FILE_PATH = string.Format(FORMAT_FILE_PATH, sceneName);
        var directoryName = Path.GetDirectoryName(FILE_PATH);
        //指定場所にCSファイルがない場合は新たに作成する
        if (Directory.Exists(directoryName) == false)
        {
            Directory.CreateDirectory(directoryName);
        }
        //ソースコード書き込み
        File.WriteAllText(FILE_PATH, builder.ToString(), Encoding.UTF8);
        //更新したファイルのインポート
        AssetDatabase.ImportAsset(FILE_PATH);
    }

    //ヒエラルキーにおけるオブジェクトのパスを取得
    static string GetHierarchyPath(GameObject _obj)
    {
        string _path = _obj.name;
        Transform _parent = _obj.transform.parent;
        while (_parent != null)
        {
            _path = _parent.name + "/" + _path;
            //一つ上の親に切り替え
            _parent = _parent.parent;
        }
        return _path;
    }

    //ヒエラルキーにおける一番上の親の名前を取得する
    static string GetRootObjName(GameObject _obj)
    {
        string rootName = GetHierarchyPath(_obj);
        // /以下は除去、空白は消して詰める
        rootName = rootName.Split('/').First();
        rootName = GetStrWithoutInvalidChars(rootName);
        return rootName;

    }

    //変数名取得
    static string GetDefineName(string path)
    {
        var defineName = GetStrWithoutInvalidChars(path);
        return defineName;
    }
}

