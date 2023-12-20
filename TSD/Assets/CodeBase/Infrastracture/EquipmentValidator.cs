using System;
using System.Collections.Generic;
using CodeBase.Infrastracture.Datas;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.Infrastracture
{
    public class EquipmentValidator : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _buttonsPanel;
        [SerializeField] private GameObject _viewport;
        [SerializeField] private TMP_Text _equipmentNumber;
        [FormerlySerializedAs("_keyNumber")] [SerializeField] private TMP_Text _boxNumber;
        [SerializeField] private GameObject _freeBox;
        [SerializeField] private Button _buttonApply;
        private GameObject _tempPanel;
        private List<Box> _boxes;
        public Action OnTakeKey;
        public Action OnTakeTsd;
        private string _applyText = ": подтвердил выбор ";
        private string _key;
        private string _tsd;

        public bool _canTakeKey = false;
        public bool _canTakeTsd = false;
        private SaveLoadService _saveLoadService;
        private Data _dataBase;
        private List<GameObject> _freeBoxes = new();
        private List<GameObject> _busyBoxes = new();
        private Employee _employee;
        private Box _box;

        public void Init(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            AddListeners();
        }

        public void Work()
        {
            _boxes = _saveLoadService.GetBoxes();
            _buttonApply.interactable = false;
            FillEquipments();
        }

        public void Reset()
        {
            _boxes = new List<Box>();
            _freeBoxes = new();
            _busyBoxes = new();
            _canTakeKey = false;
            _canTakeTsd = false;
            Destroy(_tempPanel);
        }

        private void FillEquipments()
        {
            _freeBox.gameObject.SetActive(true);
            _tempPanel = Instantiate(_buttonsPanel, _viewport.transform);
            _tempPanel.SetActive(true);

            foreach (var box in _boxes)
            {
                GameObject button = Instantiate(_freeBox, _tempPanel.transform);

                TMP_Text text = button.GetComponentInChildren<TMP_Text>();
                text.text = box.Key;
                button.GetComponent<Button>().onClick.AddListener(() => ShowInfo(text.text));

                if (box.Busy)
                {
                    button.SetActive(false);
                    _busyBoxes.Add(button);
                }
                else
                {
                    button.SetActive(true);
                    _freeBoxes.Add(button);
                }
            }
        }

        private void ShowInfo(string text)
        { 
            _employee = _saveLoadService.Employee;
            _box = _boxes[Convert.ToInt32(text) - 1];
            _boxNumber.text = _box.Key;
            _equipmentNumber.text = _box.Equipment.ShortSerial;
            _buttonApply.interactable = true;
            SentLogMessage(_employee+" выбрал ячейку : "+_boxNumber+" сканер : "+_equipmentNumber.text );
        }

        private void OnButtonClick()
        {
            _employee = _saveLoadService.Employee;
            _employee.SetBox(_box);
            
            SentLogMessage(_employee +_applyText+" ячейки : "+_employee.Box.Key+" сканера : "+_employee.Box.Equipment.ShortSerial);
            SentDataMessage(_employee +_applyText+" ячейки : "+_employee.Box.Key+" сканера : "+_employee.Box.Equipment.ShortSerial);
            
            _saveLoadService.SetCurrentEmployee(_employee);
            
            _buttonApply.interactable = false;
            OnTakeKey?.Invoke();
        }

        public void SwithState(bool state)
        {
            _panel.SetActive(state);
        }

        private void AddListeners()
        {
            _buttonApply.onClick.AddListener(OnButtonClick);
        }

        private void RemuveListeners()
        {
            _buttonApply.onClick.RemoveListener(OnButtonClick);
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