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
        [FormerlySerializedAs("_dataLoader")] [SerializeField] private AdminPanel _adminPanel;
        [SerializeField] private EquipmentReturnMenu _equipmentReturnMenu;
        [SerializeField] private WarningPanel _warningPanel;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _additionalButton;
        [SerializeField] private AdditionalMenu _additionalMenu;

        private bool _isLoggedEmployee = false;
        private bool _isLoggedAdmin = false;
        private bool _isRegistrationEnd = false;
        private bool _isReturnEnd = false;
        private bool _isGetEquipmentSelect = false;
        public Action OnButtonClick;
         public Action<bool> OnEnterAdmin;
        
        
        public void Init(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _adminPanel.Init(_saveLoadService);
            _warningPanel.Init(_saveLoadService);
            _employeeRegistrationMenu.Init(_saveLoadService, _warningPanel);
            _switchMenu.Init(_saveLoadService, _warningPanel);
            _equipmentRegistrationMenu.Init(_saveLoadService);
            _equipmentReturnMenu.Init(_saveLoadService, _warningPanel);
            _additionalMenu.Init(_saveLoadService,this,_warningPanel);
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

            _employeeRegistrationMenu.SwithPanelState(true);
            _equipmentRegistrationMenu.SwitchValidatorState(false);
            _additionalMenu.Work();
            _switchMenu.SwithState(false);
           
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
            _adminPanel.Reset();
            _employeeRegistrationMenu.Reset();
            _equipmentRegistrationMenu.Reset();
            _equipmentReturnMenu.Reset();
            
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
            _employeeRegistrationMenu.SwithPanelState(false);
            _switchMenu.SwithState(true);
            _additionalButton.gameObject.SetActive(true);
        }

        private void OnLoggedEmployee()
        {
            _isLoggedEmployee = true;
            _employeeRegistrationMenu.SwithPanelState(false);
            _switchMenu.SwithState(true);
        }

        private void OnLoggedManager()
        {
            _isLoggedAdmin = true;
            _employeeRegistrationMenu.SwithPanelState(false);
            _switchMenu.SwithState(true);
            _additionalButton.gameObject.SetActive(true);
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
            _employeeRegistrationMenu.SwithPanelState(true);
            Reset();
        }

        private void OnApplyRegistration()
        {
            _equipmentRegistrationMenu.SwitchValidatorState(false);
            _employeeRegistrationMenu.Reset();
            _employeeRegistrationMenu.SwithPanelState(true);
            Reset();
        }

        private void OnEndRegistration()
        {
            _isRegistrationEnd = true;
            _backButton.gameObject.SetActive(false);
        }

        private void OnReturnEquipment()
        {
            _additionalButton.gameObject.SetActive(false);
            _switchMenu.SwithState(false);
            _equipmentReturnMenu.SwitchReturnMenuState(true);
        }

        private void OnGetEquipment()
        {
            _additionalButton.gameObject.SetActive(false);
            _isGetEquipmentSelect = true;
            _switchMenu.SwithState(false);
            _equipmentRegistrationMenu.SwitchValidatorState(true);
        }

        private void OnLoaded(List<Employee> employees, List<Box> boxes)
        {
            _saveLoadService.SetDatabase(employees, boxes);
        }
        
       
        private void AddListeners()
        {
            _adminPanel.Loaded += OnLoaded;
            _backButton.onClick.AddListener(OnBackButtonClick);
            _employeeRegistrationMenu.OnLogged += CheckPermission;
            _employeeRegistrationMenu.OnLoggedAdmin += OnLoggedAdmin;
            _equipmentRegistrationMenu.OnApplyRegistration += OnApplyRegistration;
            _equipmentRegistrationMenu.OnRegistrationEnd += OnEndRegistration;
            _switchMenu.OnGetEquipment += OnGetEquipment;
            _switchMenu.OnReturnEquipment += OnReturnEquipment;
            _equipmentReturnMenu.OnReturnApply += OnReturnApply;
            _equipmentReturnMenu.OnReturnEnd += OnReturnEnd;
            _additionalButton.onClick.AddListener(OnAdditionalButtonClick);
        }

        private void RemuveListeners()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClick);
            _adminPanel.Loaded -= OnLoaded;
            _employeeRegistrationMenu.OnLogged -= CheckPermission;
            _employeeRegistrationMenu.OnLoggedAdmin -= OnLoggedAdmin;
            _equipmentRegistrationMenu.OnApplyRegistration -= OnApplyRegistration;
            _equipmentRegistrationMenu.OnRegistrationEnd -= OnEndRegistration;
            _switchMenu.OnGetEquipment -= OnGetEquipment;
            _switchMenu.OnReturnEquipment -= OnReturnEquipment;
            _equipmentReturnMenu.OnReturnApply -= OnReturnApply;
            _equipmentReturnMenu.OnReturnEnd -= OnReturnEnd;
            _additionalButton.onClick.RemoveListener(OnAdditionalButtonClick);
        }

        private void OnAdditionalButtonClick()
        {
            OnEnterAdmin?.Invoke(_isLoggedAdmin);
            _additionalButton.gameObject.SetActive(false);
            _switchMenu.SwithState(false);
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