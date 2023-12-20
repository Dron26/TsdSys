using System.Collections.Generic;
using CodeBase.Infrastracture.Datas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastracture
{
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _resultPanel;
        [SerializeField] private TMP_Text _employeeField;
        [SerializeField] private TMP_Text _keyField;
        [SerializeField] private TMP_Text _tsdField;


        private string _employee;
        private string _tsd;
        private Data _data;
        [SerializeField] private Button _buttonApply;
        private Dictionary<int, int> _equipments;
        private Dictionary<string, string> _employees;
        private SaveLoadService _saveLoadService;

        public void Init(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void Work()
        {
            Reset();
        }


        public void SetData()
        {
            _data = _saveLoadService.Database;
            SwithState(true);
            _employeeField.text = _data.Employee.Login;
            _keyField.text = _data.Employee.Box.Key;
            _tsdField.text = _data.Employee.Box.Equipment.ShortSerial;
        }

        public void SwithState(bool state)
        {
            _resultPanel.gameObject.SetActive(state);
        }

        public void Reset()
        {
            _employeeField.text = "";
            _keyField.text = "";
            _tsdField.text = "";
        }
    }
}