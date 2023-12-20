using System;
using System.Collections.Generic;
using CodeBase.Infrastracture.Datas;
using CodeBase.Infrastracture.UserManagerPanel;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.Infrastracture
{
    public class Programm : MonoBehaviour
    {
        private SaveLoadService _saveLoadService;
        [SerializeField] private EmployeeRegistrationMenu _employeeRegistrationMenu;
        [SerializeField] private EquipmentRegistrationMenu _equipmentRegistrationMenu;
        [SerializeField] private SwithMenu _switchMenu;
        [SerializeField] private DataLoader _dataLoader;
        [SerializeField] private EquipmentReturnMenu _equipmentReturnMenu;
        [SerializeField] private WarningPanel _warningPanel;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private ControllPanel _controllPanel;
        [SerializeField] private Button _controllPanelButton;

        private bool _isLoggedEmployee = false;
        private bool _isLoggedAdmin = false;
        private bool _isRegistrationEnd = false;
        private bool _isReturnEnd = false;
        private bool _isGetEquipmentSelect = false;

        public void Init(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _dataLoader.Init(_saveLoadService);
            _warningPanel.Init(_saveLoadService);
            _employeeRegistrationMenu.Init(_saveLoadService, _warningPanel);
            _switchMenu.Init(_saveLoadService, _warningPanel);
            _equipmentRegistrationMenu.Init(_saveLoadService);
            _equipmentReturnMenu.Init(_saveLoadService, _warningPanel);
            _controllPanel.Init(_saveLoadService,_warningPanel);
            AddListeners();
            SentLogMessage("Программа инизиализированна");
        }

        public void Work()
        {
            _warningPanel.Work();
            _employeeRegistrationMenu.Work();
            _switchMenu.Work();
            _equipmentRegistrationMenu.Work();
            _equipmentReturnMenu.Work();

            _employeeRegistrationMenu.SwithState(true);
            _equipmentRegistrationMenu.SwitchValidatorState(false);

            _switchMenu.SwithState(false);
            _settingsPanel.SetActive(false);
            _controllPanel.SwithState(false);
            SentLogMessage("Программа Запущена");
        }

        private void Reset()
        {
            SentLogMessage("Программа Сброшена");
            _isLoggedEmployee = false;
            _isLoggedAdmin = false;
            _isRegistrationEnd = false;
            _isReturnEnd = false;
            _isGetEquipmentSelect = false;
            _backButton.gameObject.SetActive(true);
            _dataLoader.Reset();
            _employeeRegistrationMenu.Reset();
            _equipmentRegistrationMenu.Reset();
            _equipmentReturnMenu.Reset();
            
            _settingsPanel.SetActive(false);
            _settingsButton.gameObject.SetActive(false);
            _controllPanel.Reset();
            _controllPanel.SwithState(true);
            _controllPanelButton.gameObject.SetActive(true);
            
            Work();
        }

        private void OnBackButtonClick()
        {
            if (_isLoggedEmployee)
            {
                if (_isGetEquipmentSelect == false)
                {
                    if (_isReturnEnd == false)
                    {
                        Reset();
                    }
                }
                else
                {
                    if (_isRegistrationEnd == false)
                    {
                        Reset();
                    }
                }
            }
        }

        private void OnLoggedAdmin()
        {
            _isLoggedAdmin = true;
            _employeeRegistrationMenu.SwithState(false);
            _switchMenu.SwithState(true);
            _settingsButton.gameObject.SetActive(true);
            _settingsButton.interactable=true;
            _controllPanelButton.gameObject.SetActive(true);
            _controllPanelButton.interactable = true;
        }

        private void OnLoggedEmployee()
        {
            _isLoggedEmployee = true;
            _employeeRegistrationMenu.SwithState(false);
            _switchMenu.SwithState(true);
        }

        private void OnLoggedManager()
        {
            _isLoggedAdmin = true;
            _employeeRegistrationMenu.SwithState(false);
            _switchMenu.SwithState(true);
            _controllPanelButton.gameObject.SetActive(true);
        }
        private void CheckPermission()
        {
            if (_saveLoadService.Employee.Permission=="0")
            {
                SentLogMessage(" Вход с правами сотрудника ");
                OnLoggedEmployee();
            }
            else if (_saveLoadService.Employee.Permission=="1")
            {
                SentLogMessage(" Вход с правами менеджера ");
                OnLoggedManager();
            }
            else if (_saveLoadService.Employee.Permission=="2")
            {
                SentLogMessage(" Вход с правами администратора ");
                OnLoggedAdmin();
            }
        }

        private void OnReturnEnd()
        {
            _isReturnEnd = true;
            _backButton.gameObject.SetActive(false);
        }

        private void OnReturnApply()
        {
            _employeeRegistrationMenu.Reset();
            _employeeRegistrationMenu.SwithState(true);
            Reset();
        }

        private void OnApplyRegistration()
        {
            _equipmentRegistrationMenu.SwitchValidatorState(false);
            _employeeRegistrationMenu.Reset();
            _employeeRegistrationMenu.SwithState(true);
            Reset();
        }

        private void OnEndRegistration()
        {
            _isRegistrationEnd = true;
            _backButton.gameObject.SetActive(false);
        }

        private void OnReturnEquipment()
        {
            _controllPanelButton.gameObject.SetActive(false);
            _switchMenu.SwithState(false);
            _equipmentReturnMenu.SwitchReturnMenuState(true);
        }

        private void OnGetEquipment()
        {
            _controllPanelButton.gameObject.SetActive(false);
            _isGetEquipmentSelect = true;
            _switchMenu.SwithState(false);
            _equipmentRegistrationMenu.SwitchValidatorState(true);
        }

        private void OnLoaded(List<Employee> employees, List<Box> boxes)
        {
            _saveLoadService.SetDatabase(employees, boxes);
        }
        private void OpenControllPanel()
        {
            _controllPanel.SwithState(true);
        }
        
        private void OpenSettingsPanel()
        {
           _settingsPanel.SetActive(true);
        }

        private void AddListeners()
        {
            _dataLoader.Loaded += OnLoaded;
            _backButton.onClick.AddListener(OnBackButtonClick);
            _employeeRegistrationMenu.OnLogged += CheckPermission;
            _employeeRegistrationMenu.OnLoggedAdmin += OnLoggedAdmin;
            _equipmentRegistrationMenu.OnApplyRegistration += OnApplyRegistration;
            _equipmentRegistrationMenu.OnRegistrationEnd += OnEndRegistration;
            _switchMenu.OnGetEquipment += OnGetEquipment;
            _switchMenu.OnReturnEquipment += OnReturnEquipment;
            _equipmentReturnMenu.OnReturnApply += OnReturnApply;
            _equipmentReturnMenu.OnReturnEnd += OnReturnEnd;
            _controllPanelButton.onClick.AddListener(OpenControllPanel);
            _settingsButton.onClick.AddListener(OpenSettingsPanel);
        }

        private void RemuveListeners()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClick);
            _dataLoader.Loaded -= OnLoaded;
            _employeeRegistrationMenu.OnLogged -= CheckPermission;
            _employeeRegistrationMenu.OnLoggedAdmin -= OnLoggedAdmin;
            _equipmentRegistrationMenu.OnApplyRegistration -= OnApplyRegistration;
            _equipmentRegistrationMenu.OnRegistrationEnd -= OnEndRegistration;
            _switchMenu.OnGetEquipment -= OnGetEquipment;
            _switchMenu.OnReturnEquipment -= OnReturnEquipment;
            _equipmentReturnMenu.OnReturnApply -= OnReturnApply;
            _equipmentReturnMenu.OnReturnEnd -= OnReturnEnd;
            _controllPanelButton.onClick.RemoveListener(OpenControllPanel);
            _settingsButton.onClick.RemoveListener(OpenSettingsPanel);
        }
        

        private void SentLogMessage(string message)
        {
            _saveLoadService.SentLogInfo(message);
        }

        private void OnDisable()
        {
            RemuveListeners();
        }
    }
}