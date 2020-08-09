using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Reo
{
    //リソースのパスを定数で管理するクラスを作成する
    public static class ResourcesPathDefineCreator
    {
        //Unityエディタメニュー
        const string MENU_ITEM_NAME = "Tools/リソースファイルパス作成";

        //作成ファイルパス
        const string FILE_FULL_PATH = "Assets/Scripts/Defines/ResourcePaths.cs";


        //作成ファイル名取得
        //true 拡張子あり　false 拡張子なし
        static string GetFileName(bool isExtention = true)
        {
            return (isExtention == true)
                ? Path.GetFileName(FILE_FULL_PATH)
                : Path.GetFileNameWithoutExtension(FILE_FULL_PATH);
        }

        //Unityエディタ上メニューからソースコードを作成する
        [MenuItem(MENU_ITEM_NAME)]
        public static void Create()
        {
            if (CanCreate() == true)
            {
                CreateDefineFile();
            }

            EditorUtility.DisplayDialog(GetFileName(), "作成完了", "OK");
        }

        //作成できるかどうか
        [MenuItem(MENU_ITEM_NAME, true)]
        public static bool CanCreate()
        {
            //シーン実行中であったり，コンパイル中であったりするならば作成させないようにする
            return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
        }

        //作成処理
        static void CreateDefineFile()
        {
            var builder = new StringBuilder();

            //↓生成するソースコード部分///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            builder.AppendLine("//スクリプトから作成するため手動での変更禁止");
            builder.AppendLine("public static partial class Define");
            builder.AppendLine("{");

            string defineName = "";
            string filePath = "";

            //取得できたすべてのリソースについて定義を行う
            foreach (var resPath in GetAllResourcesPath())
            {
                defineName = GetResourceFileDefineName(resPath);
                filePath = GetFilePathWithoutExtention(resPath);
                builder.Append("\t\t").AppendFormat(@"public const string {0} = ""{1}"";", defineName, filePath).AppendLine();
            }

            builder.AppendLine("}");

            //↑生成するソースコード部分///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var directoryName = Path.GetDirectoryName(FILE_FULL_PATH);
            //指定場所にCSファイルがない場合は新たに作成する
            if (Directory.Exists(directoryName) == false)
            {
                Directory.CreateDirectory(directoryName);
            }

            //ソースコード書き込み
            File.WriteAllText(FILE_FULL_PATH, builder.ToString(), Encoding.UTF8);
            //更新したファイルのインポート
            AssetDatabase.ImportAsset(FILE_FULL_PATH);

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
            //リソース名
            var fileName = GetStrWithoutInvalidChars(Path.GetFileNameWithoutExtension(path));
            //フォルダが深い場合は親元がわかるように使用．
            var parentDirPath = GetStrWithoutInvalidChars(Directory.GetParent(path).Name);
            //拡張子の種類
            var extention = GetFIleTypeFromExtention(path);

            return string.Format("{0}_{1}", extention, fileName);
            //return string.Format("{0}_{1}_{2}", extention, parentDirPath,  fileName);
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
        static string GetStrWithoutInvalidChars(string str) 
        {
            Array.ForEach(INVALID_CHARS, item => str = str.Replace(item, string.Empty));
            return str;
        }


        //全てのリソースファイルのパスを取得
        static IEnumerable<string> GetAllResourcesPath()
        {
            foreach (var resourcesDirectory in Directory.GetDirectories("Assets", "Resources", SearchOption.AllDirectories).Select(item => item.Replace("\\", "/")))
            {
                foreach (var path in Directory.GetFiles(resourcesDirectory, "*.*", SearchOption.AllDirectories).Select(item => item.Replace("\\", "/")))
                {
                    //拡張子が定義されていないものであった場合はスルー
                    if (string.IsNullOrEmpty(GetFIleTypeFromExtention(path)))
                    {
                        continue;
                    }
                    yield return path;
                }
            }
        }

        //拡張子に対応するファイルの種類名を取得する
        static string GetFIleTypeFromExtention(string path)
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
                return "Preafb";
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
            else if(new List<string>() { ".txt", ".htm", ".html", ".xml", ".bytes", ".json", ".csv" }.Contains(extension))
            {
                return "Text";
            }
            else if(new List<string>() { ".psd", ".tif", ".tiff", ".jpg", ".tga", ".png", ".gif", ".bmp" }.Contains(extension))
            {
                return "Texture";
            }
            else if(new List<string>() { ".asset" }.Contains(extension))
            {
                return "Asset";
            }
            else if (new List<string>() { ".unity" }.Contains(extension))
            {
                return "Scene";
            }
            else if (new List<string>() { ".asihdt" }.Contains(extension))
            {
                return "Asihdt";
            }

            return "";
        }
    }
}

