using System;
using CodeBase.Infrastracture.Datas;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastracture.UserManagerPanel
{
    public class EmployeeRegistrationMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _registrationPanel;
        [SerializeField] private EmployeeValidator _employeeValidator;
        [SerializeField] private Image _CheckUpLogin;
        [SerializeField] private Image _CheckDownLogin;
        [SerializeField] private Image _CheckUpPass;
        [SerializeField] private Image _CheckDownPass;

        private string _textButtn = ": подтвердил логин/пароль ";
        private SaveLoadService _saveLoadService;

        public Action OnLogged;
        public Action OnLoggedAdmin { get; set; }


        public void Init(SaveLoadService saveLoadService, WarningPanel warningPanel)
        {
            _saveLoadService = saveLoadService;
            AddListeners();
            _employeeValidator.Init(_saveLoadService, warningPanel);
        }

        public void Work()
        {
            _employeeValidator.Work();
            Reset();
        }

        public void Reset()
        {
            _CheckDownLogin.enabled = true;
            _CheckUpLogin.enabled = false;
            _CheckDownPass.enabled = true;
            _CheckUpPass.enabled = false;
            _employeeValidator.Reset();
        }

        private void OnInputCorrectPassword()
        {
            SentLogMessage(_saveLoadService.Employee.Login+"//"+_textButtn);
            
            _CheckDownPass.enabled = false;
            _CheckUpPass.enabled = true;
            OnLogged?.Invoke();
            Reset();
        }

        private void OnIsLogged()
        {
            _CheckDownLogin.enabled = false;
            _CheckUpLogin.enabled = true;
        }

        public void SwithPanelState(bool state)
        {
            _registrationPanel.gameObject.SetActive(state);
        }

        private void AddListeners()
        {
            _employeeValidator.IsLogged += OnIsLogged;
            _employeeValidator.InputCorrectPassword += OnInputCorrectPassword;
        }

        private void RemuveListeners()
        {
            _employeeValidator.IsLogged -= OnIsLogged;
            _employeeValidator.InputCorrectPassword -= OnInputCorrectPassword;
        }
        
        private void SentLogMessage
            (string message)
        {
            _saveLoadService.SentLogInfo(message);
        }

        private void OnDisable()
        {
            RemuveListeners();
        }
    }
}