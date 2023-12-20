using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Infrastracture.Datas;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.Infrastracture
{
    public class EquipmentReturnMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _buttonApply;
        [SerializeField] private TMP_Text _employeeField;
        [SerializeField] private TMP_Text _keyField;
        [SerializeField] private TMP_Text _tsdField;
        [SerializeField] private TMP_InputField _inputReturnField;
        [SerializeField] private Button _resetInput;

        [SerializeField] private Image _CheckUp;
        [SerializeField] private Image _CheckDown;
        
        public Action OnReturnEnd;
        private SaveLoadService _saveLoadService;
        private bool _isReseted;
        private Employee _employee;
        private bool _isSerialNumberInputed;
        private bool _isinfoInputed;
        public Action OnReturnApply;
        private WarningPanel _warningPanel;

        public void Init(SaveLoadService saveLoadService, WarningPanel warningPanel)
        {
            _saveLoadService = saveLoadService;
            _warningPanel = warningPanel;
            AddListeners();
        }

        public void Work()
        {
            _employee = _saveLoadService.Employee;
            _inputReturnField.Select();
            _inputReturnField.ActivateInputField();
            _inputReturnField.interactable = true;
            _buttonApply.interactable = false;
            
            _CheckDown.enabled = true;
            _CheckUp.enabled = false;
        }

        private void OnApplyButtonClick()
        {
            SentDataMessage(_employee.Login+ " подтвердил возврат сканера:"+_employee.Box.Equipment.ShortSerial+" в ячейку: "+_employee.Box.Key);
            
            _saveLoadService.SetReturnBox(_employee.GetBox());
            _saveLoadService.SetCurrentEmployee(_employee);
            
            Reset();
            SwitchReturnMenuState(false);
            OnReturnApply?.Invoke();
        }

        public void SetData()
        {
            _employeeField.text = _employee.Login;
            _keyField.text = _employee.Box.Key;
            _tsdField.text = _employee.Equipment.ShortSerial;
        }

        public void Reset()
        {
            _isReseted = true;
            _employeeField.text = "";
            _keyField.text = "";
            _tsdField.text = "";
            _inputReturnField.text = "";
            _inputReturnField.Select();
            _inputReturnField.ActivateInputField();
            _inputReturnField.interactable = true;
            
            _CheckDown.enabled = true;
            _CheckUp.enabled = false;
            
            _isReseted = false;
        }
        
        private void ResetInput()
        {
            SentLogMessage(_employee.Login+ "Выполнил сброс ввода QR сканера");
            Reset();
        }

        public void ValidateReturn()
        {
            if (_isReseted == false && _isinfoInputed == false)
            {
                _isinfoInputed = true;
                StartCoroutine(CheckInput());
            }
        }


        private IEnumerator CheckInput()
        {
            yield return new WaitForSeconds(2f);

            if (_inputReturnField.text == _employee.Box.Equipment.ShortSerial && _isSerialNumberInputed == false)
            {
                SentLogMessage(_employee.Login+ ": отсканировал верный QR");
                
                _isSerialNumberInputed = true;
                OnReturnEnd?.Invoke();
                _inputReturnField.interactable = false;
                _buttonApply.interactable = true;
                _CheckDown.enabled = false;
                _CheckUp.enabled = true;
            }


            if (_isSerialNumberInputed == false)
            {
                List<Employee> employees = _saveLoadService.GetEmployees();

                try
                {
                    foreach (var employee in employees)
                    {
                        if (employee != _employee && _inputReturnField.text == employee.Equipment.ShortSerial)
                        {
                            _warningPanel.ShowWindow(WindowNames.CanNotReturnOtherEquipment.ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
                SentLogMessage(_employee.Login+ ": отсканировал неверный QR");
            }

            _isinfoInputed = false;
        }

        public void SwitchReturnMenuState(bool state)
        {
            _panel.gameObject.SetActive(state);
            
            if (state)
            {
                SetData();
            }
        }

        private void AddListeners()
        {
            _buttonApply.onClick.AddListener(OnApplyButtonClick);
            _inputReturnField.onValueChanged.AddListener(delegate { ValidateReturn(); });
            _resetInput.onClick.AddListener(ResetInput);
        }

        private void RemuveListeners()
        {
            _inputReturnField.onValueChanged.RemoveListener(delegate { ValidateReturn(); });
            _buttonApply.onClick.RemoveListener(OnApplyButtonClick);
            _resetInput.onClick.RemoveListener(ResetInput);
        }

       

        private void SentLogMessage(string message)
        {
            _saveLoadService.SentLogInfo(message);
        }
        
        private void SentDataMessage(string message)
        {
            _saveLoadService.SentLogInfo(message);
        }

        private void OnDisable()
        {
            RemuveListeners();
        }
    }
}