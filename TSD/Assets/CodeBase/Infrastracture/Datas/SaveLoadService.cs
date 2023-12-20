using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

// Предполагаю, что это ваши собственные библиотеки

namespace CodeBase.Infrastracture.Datas
{
    public class SaveLoadService : MonoBehaviour
    {
        [SerializeField] private SenterInfo _senter;
        SaveLoadFile saveLoadFile = new SaveLoadFile();
        public bool IsDatabaseLoaded => _isDatabaseLoaded;
        private readonly string _employeesInfo = "employee.json";
        private readonly string _boxInfo = "box.json";
        private readonly string _dataInfo = "data.json";

        private string _filePathEmployee;
        private string _filePathBox;
        private string _filePathData;

        private bool _isDatabaseLoaded;
        public Data Database => _database;
        private Data _database;
        public Employee Employee => _employee;
        private Employee _employee;
        private List<Box> _boxes;
        private List<Employee> _employees;

        public void Init()
        {
            _filePathEmployee = Path.Combine(Application.persistentDataPath, _employeesInfo);
            _filePathBox = Path.Combine(Application.persistentDataPath, _boxInfo);
            _filePathData = Path.Combine(Application.persistentDataPath, _dataInfo);
            CreateTestBase();
            _database = LoadDatabase();
        }

        public void SaveDatabase()
        {
            try
            {
                // saveLoadFile.SaveToJson(_databaseFile, _database);
                string json = JsonConvert.SerializeObject(_boxes);
                File.WriteAllText(_filePathEmployee, json);
                json = JsonConvert.SerializeObject(_employees);
                File.WriteAllText(_filePathBox, json);
                //json = JsonConvert.SerializeObject(Employee);
                // File.WriteAllText(_filePathData, json);
                
                SentLogInfo("База данных успешно сохранена.");
                
            }
            catch (Exception ex)
            {
                SentLogInfo("Ошибка сохранения базы данных: " + ex.Message);
            }
        }

        public Data LoadDatabase()
        {
            if (!File.Exists(_filePathEmployee))
            {
                SaveDatabase();
            }
            else
            {
                try
                {
                    // _database= saveLoadFile.LoadFromJson<Data>(_databaseFile);
                    string json = File.ReadAllText(_filePathEmployee);
                    _boxes = JsonConvert.DeserializeObject<List<Box>>(json);
                    json = File.ReadAllText(_filePathBox);
                    _employees = JsonConvert.DeserializeObject<List<Employee>>(json);
                    // json = File.ReadAllText(_filePathData);
                    // _employee=JsonConvert.DeserializeObject<Employee>(json);
                    _database = new Data(_employees, _boxes);
                    _database.SetEmployee(_employees[0]);
                    _isDatabaseLoaded = true;
                    
                    SentLogInfo("База данных успешно загружена.");
                }
                catch (Exception ex)
                {
                    SentLogInfo("Ошибка загрузки базы данных: " + ex.Message);
                }
            }

            return _database;
        }

        private void CreateTestBase()
        {
            Box box = new Box("00", new Equipment("0001"));
            _boxes = new List<Box>();
            _boxes.Add(box);
            _employee = new Employee("Admin", "Admin");
            _employee.SetPermision("2");
            _employees = new List<Employee>();
            _employees.Add(_employee);
            _database = new Data(_employees, _boxes);
            _database.SetEmployee(_employees[0]);
            SentLogInfo("База данных успешно создана.");
        }

        private string GetDatabaseFilePath()
        {
            return Path.Combine(Application.persistentDataPath, _employeesInfo);
        }

        public void SetDatabase(List<Employee> employees, List<Box> boxes)
        {
            _employees = employees;
            _boxes = boxes;
            SaveDatabase();
            SetCurrentEmployee(_employees[0]);
        }

        public void SetCurrentEmployee(Employee employee)
        {
            _database.SetEmployee(employee);
            _employee = employee;
            SaveDatabase();
        }

        public List<Employee> GetEmployees()
        {
            return _database.GetEmployees();
        }

        public List<Box> GetBoxes()
        {
            return _database.GetBoxes();
        }

        public void SentDataInfo(string action)
        {
             DateTime currentTime = DateTime.Now;
             SentData sentData = new SentData(action, _database.Employee.Login,_database.Employee.Box.Key, _database.Employee.Box.Equipment.ShortSerial, currentTime.ToString());
             _senter.Send(sentData);
        }

        public void SentLogInfo(string action)
        {
            _senter.SendLog(action);
        }

        public void SetReturnBox(Box box)
        {
            _database.SetBox(box);
        }
    }
}