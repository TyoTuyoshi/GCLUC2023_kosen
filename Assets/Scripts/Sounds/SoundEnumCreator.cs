using System.Collections.Generic;
using System.IO;
using UnityEditor;

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
    public enum SeEnum
    {{
        {0}
    }}
}}
";

        private const string Path = "Assets/Scripts/Sounds/{0}";

        public static void CreateSource(IEnumerable<string> labels, string fileName)
        {
            var code = string.Format(CodeTemplate, string.Join(",\r\n", labels));

            var path = string.Format(Path, fileName);
            if (File.Exists(path)) File.Delete(path);
            File.WriteAllText(path, code);
            
            AssetDatabase.Refresh();
        }
    }
}