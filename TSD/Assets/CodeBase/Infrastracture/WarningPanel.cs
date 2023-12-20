using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Infrastracture.Datas;
using UnityEngine;

namespace CodeBase.Infrastracture
{
    public class WarningPanel : MonoBehaviour
    {
        [SerializeField] private Window _window;
        SaveLoadService _saveLoadService;

        private List<string> _messages = new List<string>();
        private Dictionary<WindowNames, Action> _windowNames;
        private List<string> _texts;
        private string _equipmentNumber;
        private string _cellNumber;
        private string _employeeName;
        private string _textMassage="!!!Warning!!! Ответ сотруднику: ";

        public void Init(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }
        
        public void Work()
        {
            FillText();
            FillDictionary();
        }

        private void FillDictionary()
        {
            _windowNames = new Dictionary<WindowNames, Action>();
            _windowNames.Add(WindowNames.CanNotTakeEquipment, ShowCanNotTakeEquipment);
            _windowNames.Add(WindowNames.CanNotReturnEquipment, CanNotReturnEquipment);
            _windowNames.Add(WindowNames.CanNotReturnOtherEquipment, CanNotReturnOtherEquipment);
            _windowNames.Add(WindowNames.CanReturnEquipment, CanReturnEquipment);
            _windowNames.Add(WindowNames.CanNotTakeAnyEquipment, CanNotTakeAnyEquipment);
            _windowNames.Add(WindowNames.EmptyPassword, OnEmptyPassword);
        }


        private void FillText()
        {
            _texts = new List<string>();
            _texts.Add("Для получения нового сканера, необходимо сдать ");
            _texts.Add(" должен хранится ");
            _texts.Add(" зарегистрирован на сотруднике: ");
            _texts.Add("Разместите ");
            _texts.Add("Сканер № ");
            _texts.Add(" в ячейке № ");
            _texts.Add("На сотруднике: ");
            _texts.Add(" нет зарегистрированных сканеров ");
            _texts.Add("Пароль не может быть пустым ");
            
        }

        public void ShowWindow(string name)
        {
            Data data = _saveLoadService.Database;
            _equipmentNumber = data.Employee.Equipment.ShortSerial;
            _cellNumber = data.Employee.Box.Key;
            _employeeName = data.Employee.Login;

            if (_windowNames.ContainsKey((WindowNames)Enum.Parse(typeof(WindowNames), name)))
            {
                _windowNames[(WindowNames)Enum.Parse(typeof(WindowNames), name)]();
            }
        }

        private void ShowCanNotTakeEquipment()
        {
            string text = _texts[0] + _texts[4] + _equipmentNumber;
           
            SetText(text);
        }

        private void CanNotReturnEquipment()
        {
            string text = _texts[4] + _equipmentNumber + _texts[1] + _texts[5] + _cellNumber;
            
            SetText(text);
        }

        private void CanNotReturnOtherEquipment()
        {
            string text = _texts[4] + _equipmentNumber + _texts[2] + _employeeName;
            
            SetText(text);
        }

        private void CanReturnEquipment()
        {
            string text = _texts[3] + _texts[4] + _equipmentNumber + _texts[5] + _cellNumber;
            
            SetText(text);
        }
        
        private void CanNotTakeAnyEquipment()
        {
            string text = _texts[6] + _employeeName  + _texts[7];
            
            SetText(text);
        }
        
        private void OnEmptyPassword()
        {
            string text = _texts[8];
            
            SetText(text);
        }

        private void SetText(string text)
        {
            _window.gameObject.SetActive(true);
            SentLogMessage(_textMassage+text);
            _window.SetText(text);
        }
        public void SwithState(bool state)
        {
            gameObject.SetActive(state);
        }

        private void SentLogMessage(string message)
        {
            _saveLoadService.SentLogInfo(message);
        }
        
    }
}

enum WindowNames
{
    CanNotTakeEquipment,
    CanNotReturnEquipment,
    CanNotReturnOtherEquipment,
    CanReturnEquipment,
    CanNotTakeAnyEquipment,
    EmptyPassword
}