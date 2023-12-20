using System;
using System.Collections;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

namespace CodeBase.Infrastracture
{
    public class SenterInfo : MonoBehaviour
    {
        private string Action;
        private string Log;
        private string Login;
        private string Pass;
        private string Tsd;
        private string Key;
        private string Time;

        private string BASE_URL =
            "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdRpi0F76k8qqLInTcm1JOi6RfPPDCbQXob1tDsxt_JRykCvg/formResponse";

        private string LOG_URL =
            "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdJY2MKpPaQr2ZMwplrWkbfkL-zbiBY3G9Gdnx1sY3O96R8mg/formResponse";

        public void SaveDataToFile(string filename, string content)
        {
            string path = Path.Combine(Application.persistentDataPath, filename);

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(content);
                }

                Debug.Log("Data saved to: " + path);
            }
            catch (Exception e)
            {
                Debug.LogError("Error saving data to file: " + e.Message);
            }
        }

        public void SaveDataToTxt()
        {
            string data = $"Action: {Action},Login: {Login}, Pass: {Pass}, Tsd: {Tsd}, Key: {Key}, Time: {Time}";
            AppendDataToFile("data.txt", data);
        }

        public void SaveLogDataToTxt()
        {
            string data = $"Action: {Log}";
            AppendDataToFile("log.txt", data);
        }

        public void AppendDataToFile(string filename, string content)
        {
            string path = Path.Combine(Application.persistentDataPath, filename);

            try
            {
                using (StreamWriter
                       writer = new StreamWriter(path, true)) // Второй параметр true для дописывания в файл
                {
                    writer.WriteLine(content); // Дописываем данные в новую строку
                }

                Debug.Log("Data appended to: " + path);
            }
            catch (Exception e)
            {
                Debug.LogError("Error appending data to file: " + e.Message);
            }
        }

        public void Send(SentData data)
        {
            Action = data.Action;
            Login = data.Login;
            Key = data.Key;
            Time = data.Time;
            Tsd = data.ShortNumber;

            SaveDataToTxt();
            StartCoroutine(Post(Login,  Key,Tsd, Time));
        }

        public void SendLog(string log)
        {
            Log = log;
            DateTime currentTime = DateTime.Now;
            SaveLogDataToTxt();
            StartCoroutine(PostLog(Log, currentTime));
        }

        IEnumerator Post(string login,string key, string tsd,  string time)
        {
            UnityWebRequest www = UnityWebRequest.Post(BASE_URL, "");

            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            string postData =
                $"entry.2099086841={login}&entry.1553343995={key}&entry.1338377421={tsd}&entry.1737371516={time}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            www.uploadHandler = new UploadHandlerRaw(byteArray);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }

            www.Dispose();
        }


        IEnumerator PostLog(string action, DateTime currentTime)
        {
            UnityWebRequest www = UnityWebRequest.Post(LOG_URL, "");

            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            string postData = $"entry.969425331={action + "  " + currentTime}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            www.uploadHandler = new UploadHandlerRaw(byteArray);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }

            www.Dispose();
        }
    }

    public class SentData
    {
        public string Action { get; set; }
        public string Login { get; set; }
        
        public string ShortNumber { get; set; }
        public string Key { get; set; }
        public string Time { get; set; }

        public SentData(string action, string login, string key,[CanBeNull] string shortNumber, string time)
        {
            Action = action;
            Login = login;
            Key = key;
            ShortNumber = shortNumber;
            Time = time;
        }
    }
}