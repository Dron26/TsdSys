using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastracture.Datas
{
    public class DataLoader : MonoBehaviour
    {
        [SerializeField] private Button _apply;
        [SerializeField] private GameObject _panelOk;
        [SerializeField] private GameObject _panelNok;
        [SerializeField] private TMP_InputField _inputLoggPass;
        [SerializeField] private TMP_InputField _inputBox;
        [SerializeField] private LogViewer _logViewer;

        public Action<List<Employee>, List<Box>> Loaded;
        public bool isLoadedEmployees = false;
        public bool isLoadedEquipment = false;
        private SaveLoadService _saveLoadService;
        private List<Employee> _employees = new();
        private List<Equipment> _equipments = new();
        private List<Box> _boxes = new();

        public void LoadData(string _inputLoggPass, string _inputBox)
        {
            _panelOk.gameObject.SetActive(false);
            _panelNok.gameObject.SetActive(false);

            try
            {
                //"C:\Users\Andrey\Documents\LogPass.txt"
                //"C:\Users\Andrey\Documents\eq.txt"
                string[] lines = File.ReadAllLines(_inputLoggPass);

                foreach (string line in lines)
                {
                    string[] values = line.Split('-');

                    if (values.Length >= 3)
                    {
                        Employee employee = new Employee(values[0], values[1]);
                        employee.SetPermision(values[2]);
                        _employees.Add(employee);
                    }
                }

                SentLogMessage("Данные по сотрудникам загружены");
                isLoadedEmployees = true;
            }
            catch (Exception ex)
            {
                _panelNok.gameObject.SetActive(true);
                SentLogMessage("Ошибка загрузки данных по сотрудникам : " + ex.Message);
                isLoadedEmployees = false;
            }

            try
            {
                string[] lines = File.ReadAllLines(_inputBox);

                foreach (string line in lines)
                {
                    string[] values = line.Split('-');

                    if (values.Length >= 2)
                    {
                        Equipment _equipment = new Equipment(values[0].Trim());
                        _equipments.Add(_equipment);
                        Box box = new Box(values[1], _equipment);
                        _boxes.Add(box);
                    }
                }

                SentLogMessage("Данные о оборудовании загружены");
                isLoadedEquipment = true;
            }
            catch (Exception ex)
            {
                SentLogMessage("Ошибка загрузки данных о оборудовании: " + ex.Message);
                _panelNok.gameObject.SetActive(true);
                isLoadedEquipment = false;
            }

            if (isLoadedEmployees && isLoadedEquipment)
            {
                _panelNok.gameObject.SetActive(false);
                _panelOk.gameObject.SetActive(true);
                Loaded?.Invoke(_employees, _boxes);
            }
        }

        public void Init(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            AddListeners();
            _logViewer.Init(_saveLoadService);
        }

        public void Reset()
        {
            isLoadedEmployees = false;
            isLoadedEquipment = false;
            _panelOk.gameObject.SetActive(false);
            _panelNok.gameObject.SetActive(false);
            _logViewer.Reset();
        }

        private void AddListeners()
        {
            _apply.onClick.AddListener(() => LoadData(_inputLoggPass.text, _inputBox.text));
        }

        private void RemuveListeners()
        {
            _apply.onClick.RemoveListener(() => LoadData(_inputLoggPass.text, _inputBox.text));
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