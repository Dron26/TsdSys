using System;
using System.IO;
using CodeBase.Infrastracture.Datas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace CodeBase.Infrastracture
{
    public class LogViewer : MonoBehaviour
    {
        [SerializeField] private Button _viweLog;
        [SerializeField] private Button _resetLog;
        [SerializeField] private TMP_Text logText;
        [SerializeField] private ScrollRect scrollRect;

        private SaveLoadService _saveLoadService;
        private string logFileName = "log.txt";

        public void Init(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            AddListeners();
        }

        public void Reset()
        {
            ClearLogFile();
        }

        private void ClearLogFile()
        {
            string filePath = Path.Combine(Application.persistentDataPath, logFileName);
            File.WriteAllText(filePath, "");
            Debug.LogWarning("Log file is cleared.");
        }

        public void ResetLogFile()
        {
            // logText.text = "";
            string filePath = Path.Combine(Application.persistentDataPath, logFileName);

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    logText.text += line + "\n";
                }

                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }
            else
            {
                Debug.LogWarning("Log file does not exist.");
            }
        }

        private void AddListeners()
        {
            _viweLog.onClick.AddListener(ClearLogFile);
            _resetLog.onClick.AddListener(ResetLogFile);
        }

        private void RemuveListeners()
        {
            _viweLog.onClick.RemoveListener(ClearLogFile);
            _resetLog.onClick.RemoveListener(ResetLogFile);
        }

        private void OnDisable()
        {
            RemuveListeners();
        }
    }
}