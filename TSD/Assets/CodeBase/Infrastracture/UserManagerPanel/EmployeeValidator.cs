using System;
using System.Collections.Generic;
using CodeBase.Infrastracture.Datas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastracture.UserManagerPanel
{
    public class EmployeeValidator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputLoginField;
        [SerializeField] private TMP_InputField _inputPassField;
        [SerializeField] private Button _resetInput;

        private string _loginText = "попытка ввода логина : ";
        private string _passText = " ввел пароль : ";
        private WarningPanel _warningPanel;
        private List<Employee> _employees;
        public Action IsLogged;
        public Action InputCorrectPassword;
        public string Employee => _eployee;

        private string _eployee;
        private string _pass;
        private string _inputText;
        private SaveLoadService _saveLoadService;
        private bool _isReseted;
        private bool _startRegistrationLogin;
        private bool _isPassInputed;
        private bool _isLogInputed;
        private bool _startRegistrationPass;
        private Employee _newEmployee;

        public void Init(SaveLoadService saveLoadService, WarningPanel warningPanel)
        {
            _saveLoadService = saveLoadService;
            _warningPanel = warningPanel;
        }

        public void Work()
        {
            AddListeners();
            _employees = _saveLoadService.GetEmployees();
            _inputLoginField.interactable = true;
            _inputLoginField.Select();
            _inputLoginField.ActivateInputField();
            _newEmployee = _employees[0];
        }

        public void Reset()
        {
            _isReseted = true;
            _inputPassField.interactable = false;
            _inputLoginField.text = "";
            _inputPassField.text = "";
            _inputLoginField.interactable = true;
            _inputLoginField.Select();
            _inputLoginField.ActivateInputField();
            _isReseted = false;
        }
        
        private void ResetInput()
        {
            SentLogMessage("Выполнен сброс ввода логина/пароля");
            Reset();
        }

        public void ValidateLogin()
        {
            if (!_isReseted)
            {
                Debug.Log(_inputLoginField.text);

                foreach (var employee in _employees)
                {
                    if (_inputLoginField.text == employee.Login && _isLogInputed == false)
                    {
                        SentLogMessage(_loginText  + _inputLoginField.text);
                        SentLogMessage("Логин верный");
                        
                        _saveLoadService.SentLogInfo(_loginText + "//" + _inputLoginField.text);
                        _eployee = _inputLoginField.text;
                        _isLogInputed = true;
                        IsLogged?.Invoke();
                        _inputLoginField.interactable = false;
                        _inputPassField.interactable = true;
                        _inputPassField.Select();
                        _inputPassField.ActivateInputField();
                    }

                    
                }

                if (!_isLogInputed)
                {
                    SentLogMessage("Логин неверный");
                }
            }
        }

        private void ValidatePass()
        {
            if (!_isReseted)
            {
                foreach (var employee in _employees)
                {
                    if (_inputPassField.text == employee.Pass && _isPassInputed == false)
                    {
                        _pass = _inputPassField.text;
                        
                        SentLogMessage(_eployee + _passText  + _pass);
                        SentLogMessage("Пароль верный");
                        
                        SentLogMessage(_eployee +" вошел в систему : "+" пароль :"+_pass);
                        
                        _isPassInputed = true;
                        _isLogInputed = false;
                        _isPassInputed = false;
                        InputCorrectPassword?.Invoke();
                        _newEmployee = employee;

                        if (_newEmployee != _saveLoadService.Employee)
                        {
                            _saveLoadService.SetCurrentEmployee(_newEmployee);
                        }
                        

                        break;
                    }
                }

                if (!_isPassInputed)
                {
                    SentLogMessage("Пароль неверный");
                }
            }
        }

        private void AddListeners()
        {
            _inputLoginField.onValueChanged.AddListener(delegate { ValidateLogin(); });
            _inputPassField.onValueChanged.AddListener(delegate { ValidatePass(); });
            _resetInput.onClick.AddListener(ResetInput);
        }

        private void RemuveListeners()
        {
            _inputLoginField.onValueChanged.RemoveListener(delegate { ValidateLogin(); });
            _inputPassField.onValueChanged.RemoveListener(delegate { ValidatePass(); });
            _resetInput.onClick.RemoveListener(ResetInput);
        }
        

        private void SentLogMessage(string message)
        {
            _saveLoadService.SentLogInfo(message);
        }
        
        private void SentDataMessage(string message)
        {
            _saveLoadService.SentDataInfo(message);
        }
        
        

        private void OnDisable()
        {
            RemuveListeners();
        }
    }
}