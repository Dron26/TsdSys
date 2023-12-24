using System.Collections.Generic;
using CodeBase.Infrastracture.Datas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class EmployeeViwer : MonoBehaviour
{
    [SerializeField] private Button _viweEmployee;
    [SerializeField] private Button _reset;
    [SerializeField] private TMP_Text _employeeList;
    [SerializeField] private ScrollRect scrollRect;

    private SaveLoadService _saveLoadService;

    public void Init(SaveLoadService saveLoadService)
    {
        _saveLoadService = saveLoadService;
        AddListeners();
    }

    public void Reset()
    {
        ClearLogFile();
    }

    public void ResetLogFile()
    {
        List<Employee> _employees = _saveLoadService.Database.GetEmployees();

        foreach (var employee in _employees)
        {
            _employeeList.text += employee.Login + employee.Box + employee.Equipment.ShortSerial+"\n";
        }

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    private void AddListeners()
    {
        _viweEmployee.onClick.AddListener(ClearLogFile);
        _reset.onClick.AddListener(ResetLogFile);
    }

    private void ClearLogFile()
    {
        _employeeList.text = "";
    }

    private void RemuveListeners()
    {
        _viweEmployee.onClick.RemoveListener(ClearLogFile);
        _reset.onClick.RemoveListener(ResetLogFile);
    }

    private void OnDisable()
    {
        RemuveListeners();
    }
}