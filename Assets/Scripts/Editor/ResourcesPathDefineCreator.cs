using UnityEngine;
using UnityEditor;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static EditorCommon;

//リソースのパスを定数で管理するクラスを作成する
public static class ResourcesPathDefineCreator
{
    //作成ファイルパス
    static readonly string FILE_PATH = "Assets/Scripts/Defines/ResourcePath.cs";

    [MenuItem("Tools/リソースファイルパスCS作成")]
    static void Create()
    {
        if(EditorCommon.CanCreate() == true)
        {
            CreateDefineFile();
        }

        EditorUtility.DisplayDialog("リソースパスCS", "作成完了", "OK");
    }

    //Define作成
    static void CreateDefineFile()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("//手動での変更禁止");
        builder.AppendLine("public static partial class Define");
        builder.AppendLine("{");

        string defineName = "";
        string filePath = "";

        foreach (var path in GetAllResourcesPath())
        {
            defineName = GetResourceFileDefineName(path);
            filePath = GetFilePathWithoutExtention(path);
            builder.Append("\t\t").AppendFormat(@"public readonly static string {0} = ""{1}"";", defineName, filePath).AppendLine();
        }

        builder.AppendLine("}");

        var directoryName = Path.GetDirectoryName(FILE_PATH);
        if(Directory.Exists(directoryName) == false)
        {
            Directory.CreateDirectory(directoryName);
        }

        //CS作成
        File.WriteAllText(FILE_PATH, builder.ToString(), Encoding.UTF8);
        AssetDatabase.ImportAsset(FILE_PATH);
    }

    //全リソースファイルパス取得
    public static IEnumerable<string> GetAllResourcesPath()
    {
        foreach (var resourceDirectory in Directory.GetDirectories("Assets", "Resources", SearchOption.AllDirectories).Select(item => item.Replace("\\", "/")))
        {
            foreach (var path in Directory.GetFiles(resourceDirectory, "*.*", SearchOption.AllDirectories).Select(item => item.Replace("\\", "/")))
            {
                //拡張子が定義されていないものはスルー
                if (string.IsNullOrEmpty(GetFileType(path)))
                {
                    continue;
                }
                yield return path;
            }
        }
    }

    //Resourceフォルダ内の拡張子を除いたパス
    static string GetFilePathWithoutExtention(string path)
    {
        //パス参照でResources/はいらないので削除
        path = Regex.Replace(path, @"^.*Resources/", "");
        //.(拡張子)削除
        path = Regex.Replace(path, @"\..*$", "");
        return path;
    }

    //定数名
    static string GetResourceFileDefineName(string path)
    {
        //リソース名   GetFileNameWithoutExtensionでファイル名のみ抜き出す
        var fileName = GetStrWithoutInvalidChars(Path.GetFileNameWithoutExtension(path));
        //フォルダが深い場合は親元がわかるように使用．
        var parentDirPath = GetStrWithoutInvalidChars(Directory.GetParent(path).Name);
        //拡張子の種類
        var extention = GetFileType(path);

        return string.Format("{0}_{1}", extention, fileName);
        //return string.Format("{0}_{1}_{2}", extention, parentDirPath,  fileName);
    }

    //拡張子に対応するファイルの種類名を取得
    static string GetFileType(string path)
        {
            string extension = Path.GetExtension(path);

            if (new List<string>() { ".anim" }.Contains(extension))
            {
                return "Animation";
            }
            else if (new List<string>() { ".wav", ".mp3", ".ogg" }.Contains(extension))
            {
                return "Audio";
            }
            else if (new List<string>() { ".prefab" }.Contains(extension))
            {
                return "Prefab";
            }
            else if (new List<string>() { ".ttf", ".otf", ".dfont" }.Contains(extension))
            {
                return "Font";
            }
            else if (new List<string>() { ".mat", ".material" }.Contains(extension))
            {
                return "Material";
            }
            else if (new List<string>() { ".fbx", ".obj", ".max", ".blend" }.Contains(extension))
            {
                return "Mesh";
            }
            else if (new List<string>() { ".mov", ".mpg", ".mpeg", ".mp4", ".avi", ".asf" }.Contains(extension))
            {
                return "Movie";
            }
            else if (new List<string>() { ".physicmaterial" }.Contains(extension))
            {
                return "PhysicMaterial";
            }
            else if (new List<string>() { ".shader" }.Contains(extension))
            {
                return "Shader";
            }
            else if (new List<string>() { ".txt", ".htm", ".html", ".xml", ".bytes", ".json", ".csv" }.Contains(extension))
            {
                return "Text";
            }
            else if (new List<string>() { ".psd", ".tif", ".tiff", ".jpg", ".tga", ".png", ".gif", ".bmp" }.Contains(extension))
            {
                return "Texture";
            }
            else if (new List<string>() { ".asset" }.Contains(extension))
            {
                return "Asset";
            }
            else if (new List<string>() { ".unity" }.Contains(extension))
            {
                return "Scene";
            }

            return "";
        }
}
