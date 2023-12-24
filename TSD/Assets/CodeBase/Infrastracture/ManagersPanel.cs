using System;
using System.Collections.Generic;
using CodeBase.Infrastracture;
using CodeBase.Infrastracture.Datas;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ManagersPanel:MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _resetEquipmentButton;
    [SerializeField] private Button _resetEmployeePassButton;
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _applyEquimpmentButton;
    [SerializeField] private Button _resetInputTextButton;
    [SerializeField] private Button _exitButton;
    
    [SerializeField] private GameObject _passPanel;
    [SerializeField] private GameObject _equipmentPanel;
    [SerializeField] private GameObject _employeesList;
    [SerializeField] private GameObject _employeeItem;
    [SerializeField] private GameObject _viewport;
    
    [SerializeField] private TMP_InputField _newPassTextInput;
    [SerializeField] private TMP_Text _textlogin;
    [SerializeField] private TMP_Text _textEquipment;
    
    private SaveLoadService _saveLoadService;
    private List<Employee> _employees = new();
    Dictionary<GameObject, Employee> _data = new Dictionary<GameObject, Employee>();
    private Employee _selectedEmployee;
    private GameObject _tempPanel;
    private WarningPanel _warningPanel;
    public Action OnExit;

    public void Init(SaveLoadService saveLoadService, WarningPanel warningPanel)
    {
        _saveLoadService = saveLoadService;
        _warningPanel = warningPanel;
        AddListeners();
        FillEmployeesList();
    }

    public void Reset()
    {
        _resetEquipmentButton.interactable = false;
        _resetEmployeePassButton.interactable = false;
        _applyButton.interactable = false;
        _applyEquimpmentButton.interactable = false;
        _resetInputTextButton.interactable = false;
        _passPanel.SetActive(false);
        _equipmentPanel.SetActive(false);
        _newPassTextInput.interactable = false;
        _newPassTextInput.text = "";
        _data=new Dictionary<GameObject, Employee>();
        Destroy(_tempPanel);
    }
    
    private void ResetInput()
    {
        SentLogMessage("Выполнен сброс логина/пароля");
        ResetInputPass();
    }

    private void ResetInputPass()
    {
        _newPassTextInput.text = "";
        _newPassTextInput.Select();
        _newPassTextInput.ActivateInputField();
    }

    private void FillEmployeesList()
    {
        _tempPanel = Instantiate(_employeesList, _viewport.transform);
        _tempPanel.SetActive(true);
        
        _employees=_saveLoadService.GetEmployees();
        
        foreach (Employee employee in _employees)
        {
            GameObject button = Instantiate(_employeeItem, _tempPanel.transform);
            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            text.text = employee.Login;
            button.GetComponent<Button>().onClick.AddListener(() => GetEmployee(button));
            _data.Add(button,employee);
        }
    }

    private void GetEmployee(GameObject button)
    {
        _selectedEmployee = _data[button];
        if (_selectedEmployee.HaveBox)
        {
            _resetEquipmentButton.interactable = true;
        }
        _resetEmployeePassButton.interactable = true;
    }
    
    private void ResetEmployeePass()
    {
        _resetEquipmentButton.interactable = false;
        _passPanel.SetActive(true);
        _resetEmployeePassButton.interactable = false;
        _applyButton.interactable = true;
        _resetInputTextButton.interactable = true;
        _newPassTextInput.interactable = true;
        _newPassTextInput.Select();
        _newPassTextInput.ActivateInputField();
    }

    private void ResetEquipment()
    {
        _resetEquipmentButton.interactable = false;
        _resetEmployeePassButton.interactable = false;
        _equipmentPanel.SetActive(true);
        _textlogin.text=_selectedEmployee.Login;
        _textEquipment.text=_selectedEmployee.Box.Key+"/ "+_selectedEmployee.Box.Equipment.ShortSerial;
    }

    private void CheckSelectedEmployee()
    {
        
    }
    
    private void ApplyAction()
    {
        if (_newPassTextInput.text!=null||_newPassTextInput.text!="")
        {
            _selectedEmployee.Pass = _newPassTextInput.text;
            _saveLoadService.SetCurrentEmployee(_selectedEmployee);
            SentLogMessage("Пароль изменен");
            ClosePassPanel();
        }   
        else
        {
            _warningPanel.ShowWindow(WindowNames.EmptyPassword.ToString());
            ResetInputPass();
        }
    }
    
    private void ApplyEquipmentAction()
    {
        _saveLoadService.SetReturnBox(_selectedEmployee.GetBox());
        _saveLoadService.SetCurrentEmployee(_selectedEmployee);
        _warningPanel.ShowWindow(WindowNames.EmptyPassword.ToString());
        _textlogin.text = "";
        _textEquipment.text = "";
    }

    private void ClosePassPanel()
    {
        _newPassTextInput.text = "";
        _applyButton.interactable = false;
        _newPassTextInput.interactable = false;
        _passPanel.SetActive(false);
        _resetInputTextButton.interactable = false;
        _resetEquipmentButton.interactable = false;
        _resetEmployeePassButton.interactable = false;
    }
   
    private void AddListeners()
    {
        _resetEquipmentButton.onClick.AddListener(ResetEquipment);
        _resetEmployeePassButton.onClick.AddListener(ResetEmployeePass);
        _resetInputTextButton.onClick.AddListener(ResetInput);
        _applyButton.onClick.AddListener(ApplyAction);
        _applyEquimpmentButton.onClick.AddListener(ApplyEquipmentAction);
        _exitButton.onClick.AddListener(Exit);
    }
    
    private void RemuveListeners()
    {
        _resetEquipmentButton.onClick.RemoveListener(ResetEquipment);
        _resetEmployeePassButton.onClick.RemoveListener(ResetEmployeePass);
        _resetInputTextButton.onClick.RemoveListener(ResetInput);
        _applyButton.onClick.RemoveListener(ApplyAction);
        _applyEquimpmentButton.onClick.RemoveListener(ApplyEquipmentAction);
        _exitButton.onClick.RemoveListener(Exit);
    }

    private void Exit()
    {
        OnExit.Invoke();
    }

    private void SentLogMessage(string message)
    {
        _saveLoadService.SentLogInfo(message);
    }

    private void OnDisable()
    {
        RemuveListeners();
    }

    public void SwithState(bool state)
    {
        _panel.SetActive(state);
    }
    
}