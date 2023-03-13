using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils
{
#if UNITY_EDITOR
    /// <summary>
    ///     クリップを検索してenumを自動生成する
    /// </summary>
    public static class EnumCreator
    {
        private const string CodeTemplate = @"
namespace AutoGenerate
{{
    public enum {0}
    {{
{1}
    }}
}}
";
        private static readonly HashSet<char> InValidChars = new()
            { '-', '[', ']', '{', '}', '(', ')', '"', '\'', '\\', '/', ',', '.', '&', '%' };

        private static bool IsValidName(string name)
        {
            if (!name.Any(c => InValidChars.Contains(c))) return true;

            Debug.LogError($"{name}には使用できない文字が含まれていたため処理されませんでした。名前を変更して再度試してください。");
            return false;
        }

        public static void CreateSource(IEnumerable<string> labels, string path)
        {
            var code = string.Format(CodeTemplate, path.Split("/").Last().Split(".").First(),
                string.Join(",\r\n", labels.Where(IsValidName)));

            if (File.Exists(path)) File.Delete(path);
            File.WriteAllText(path, code);

            AssetDatabase.Refresh();
        }
    }
#endif
}