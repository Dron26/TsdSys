using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Infrastracture
{
    public class SaveLoadFile : MonoBehaviour
    {
        public void SaveToJson<T>(string fileName, T file)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var data = JsonConvert.SerializeObject(file, Formatting.Indented, settings);
            var path = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                File.WriteAllText(path, data);
                Debug.Log("Success save!" + data);
            }
            catch (Exception e)
            {
                Debug.LogError("SaveToJson \n " + data + "\n:" + e);
                throw;
            }
        }

        public T LoadFromJson<T>(string fileName)
        {
            var path = Path.Combine(Application.persistentDataPath, fileName);

            // if (Directory.Exists(path))
            // {
            //     var result = JsonConvert.DeserializeObject<T>(null!);
            //     return result;
            // }

            try
            {
                var result = JsonConvert.DeserializeObject<T>(ReadJson(path));
                return result;
            }
            catch (Exception e)
            {
                var result = JsonConvert.DeserializeObject<T>(null!);
                Debug.LogError("LoadFromJson" + e + "Add" + result);
                return result;
                // throw;
            }
        }


        string ReadJson(string path)
        {
            string filePath = Path.Combine(Application.persistentDataPath, path);
            StreamReader streamReader = new StreamReader(filePath);
            return streamReader.ReadToEnd().TrimEnd();
        }
    }
}