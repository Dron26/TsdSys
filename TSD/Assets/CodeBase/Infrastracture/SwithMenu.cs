using System;
using CodeBase.Infrastracture.Datas;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastracture
{
    public class SwithMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _getEquipmentButton;
        [SerializeField] private Button _returnEquipmentButton;

        public Action OnGetEquipment;
        public Action OnReturnEquipment;
        private SaveLoadService _saveLoadService;

        private string _textGetEquipment = " : пытается получить сканер";
        private string _textReturnEquipment = " : пытается вернуть сканер";
        private WarningPanel _warningPanel;

        public void Init(SaveLoadService saveLoadService, WarningPanel warningPanel)
        {
            _saveLoadService = saveLoadService;
            _warningPanel = warningPanel;
            AddListeners();
        }

        public void Work()
        {
            SwithState(false);
        }

        public void HandleGetEquipmentButton()
        {
            Employee employee = _saveLoadService.Employee;

            if (employee.HaveEquipment)
            {
                _warningPanel.ShowWindow(WindowNames.CanNotTakeEquipment.ToString());
            }
            else
            {
                SentLogMessage(_saveLoadService.Employee+_textGetEquipment);
                OnGetEquipment.Invoke();
            }
        }

        public void HandleReturnEquipmentButton()
        {
            Employee employee = _saveLoadService.Employee;

            if (!employee.HaveEquipment)
            {
                _warningPanel.ShowWindow(WindowNames.CanNotTakeEquipment.ToString());
            }
            else
            {
                SentLogMessage(_saveLoadService.Employee + _textReturnEquipment);
                OnReturnEquipment.Invoke();
            }
        }

        public void SwithState(bool state)
        {
            _panel.gameObject.SetActive(state);
        }

        private void AddListeners()
        {
            _getEquipmentButton.onClick.AddListener(HandleGetEquipmentButton);
            _returnEquipmentButton.onClick.AddListener(HandleReturnEquipmentButton);
        }

        private void RemuveListeners()
        {
            _getEquipmentButton.onClick.RemoveListener(HandleGetEquipmentButton);
            _returnEquipmentButton.onClick.RemoveListener(HandleReturnEquipmentButton);
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