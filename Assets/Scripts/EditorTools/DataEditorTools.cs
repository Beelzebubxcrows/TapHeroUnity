#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
 
    public static class DataEditorTools
    {
        private static  string FILE_PATH = Application.persistentDataPath + "/gameData.json";
        
        [MenuItem("Tools/WipeOutData")]
        public static void WipeOutData()
        {
            if (!File.Exists(FILE_PATH))
            {
                return;
            }
            File.Delete(FILE_PATH);
        }
        
        [MenuItem("Tools/PrintDataInConsole")]
        public static void PrintDataInConsole()
        {
            Debug.LogError(ReadSavedGameDataFile());
        }
        
        
        private static string ReadSavedGameDataFile()
        {
            if (!File.Exists(FILE_PATH))
            {
                return "FILE DOESN'T EXIST.";
            }
            var reader = new StreamReader(FILE_PATH);
            var text = reader.ReadToEnd();
            reader.Close();
            return text;
        }
    }
    
}
#endif