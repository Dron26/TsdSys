using System;
using System.Collections.Generic;
using System.IO;
using CodeBase.Infrastracture.Datas;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InfoViwer : MonoBehaviour
{
    [SerializeField] private Button _showData;
    [SerializeField] private Button _showLog;
    [SerializeField] private Button _showEmployees;
    [SerializeField] private Button _clear;
    [SerializeField] private Button _exit;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private TMP_Text _info;
    [SerializeField] private GameObject _panel;

    private string textBase = "Состояние парка ТСД";
    public Action OnExit;

    private SaveLoadService _saveLoadService;

    public void Init(SaveLoadService saveLoadService)
    {
        _saveLoadService = saveLoadService;
        AddListeners();
        _info.gameObject.SetActive(false);
    }

    public void Reset()
    {
        _infoText.text = "";
    }

    public void ShowEmployees()
    {
        _panel.SetActive(true);
        _info.gameObject.SetActive(true);
        _info.text = textBase;
        List<Employee> _employees = _saveLoadService.Database.GetEmployees();

        foreach (var employee in _employees)
        {
            _infoText.text += employee.Login + employee.Box + employee.Equipment.ShortSerial + "\n";
        }

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void ShowInfo(string dataFileName)
    {
        _panel.SetActive(true);
        _info.gameObject.SetActive(false);
        string filePath = Path.Combine(Application.persistentDataPath, dataFileName);

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                _infoText.text += line + "\n";
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
        _showData.onClick.AddListener(() => ShowInfo(Const.DataInfo));
        _showLog.onClick.AddListener(() => ShowInfo(Const.LogInfo));
        _showEmployees.onClick.AddListener(ShowEmployees);
        _clear.onClick.AddListener(Reset);
        _exit.onClick.AddListener(Exit);
    }

    private void RemuveListeners()
    {
        _showData.onClick.RemoveListener(() => ShowInfo(Const.DataInfo));
        _showLog.onClick.RemoveListener(() => ShowInfo(Const.LogInfo));
        _showEmployees.onClick.RemoveListener(ShowEmployees);
        _clear.onClick.RemoveListener(Reset);
        _exit.onClick.RemoveListener(Exit);
    }

    private void Exit()
    {
        Reset();
        _info.gameObject.SetActive(false);
        OnExit?.Invoke();
        _panel.SetActive(false);
    }
    public void SwithState(bool state)
    {
        _panel.SetActive(state);
    }


    private void OnDisable()
    {
        RemuveListeners();
    }
}