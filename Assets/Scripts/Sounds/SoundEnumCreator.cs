using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Sounds
{
    /// <summary>
    ///     クリップを検索してenumを自動生成する
    /// </summary>
    public static class SoundEnumCreator
    {
        private const string CodeTemplate = @"
namespace Sounds
{{
    public enum {0}
    {{
        {1}
    }}
}}
";

        private const string Path = "Assets/Scripts/Sounds/{0}";

        private static readonly HashSet<char> InValidChars = new()
            { '-', '[', ']', '{', '}', '(', ')', '"', '\'', '\\', '/', ',', '.', '&', '%' };

        private static bool IsValidName(string name)
        {
            if (!name.Any(c => InValidChars.Contains(c))) return true;

            Debug.LogError($"{name}には使用できない文字が含まれていたため処理されませんでした。名前を変更して再度試してください。");
            return false;
        }

        public static void CreateSource(IEnumerable<string> labels, string fileName)
        {
            var code = string.Format(CodeTemplate, fileName.Split(".").First(),
                string.Join(",\r\n", labels.Where(IsValidName)));

            var path = string.Format(Path, fileName);
            if (File.Exists(path)) File.Delete(path);
            File.WriteAllText(path, code);

            AssetDatabase.Refresh();
        }
    }
}