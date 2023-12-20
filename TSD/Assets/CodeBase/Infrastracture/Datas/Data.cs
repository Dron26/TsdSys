using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace CodeBase.Infrastracture.Datas
{
    [Serializable]
    public class Data
    {
        [Serialize] public List<Employee> _employees = new();
        [Serialize] public List<Equipment> _equipments = new();
        [Serialize] public List<Box> _boxes = new();
        [Serialize] public Employee Employee => _currentEmployee;
        [Serialize] public Employee _currentEmployee;


        public Data(List<Employee> employees, List<Box> boxes)
        {
            foreach (var employee in employees)
            {
                Employee newEmployee = new Employee(employee.Login, employee.Pass);
                _employees.Add(newEmployee);
            }

            foreach (var box in boxes)
            {
                Box newBox = new Box(box.Key, box.Equipment);
                _boxes.Add(newBox);
            }

            _currentEmployee = _employees[0];
        }

        public void SetEmployee(Employee employee)
        {
            Employee newEmployee =new Employee("test", "test");

            foreach (var thisEmployee in _employees)
            {
                if (thisEmployee.Login == employee.Login)
                {
                    newEmployee = thisEmployee;
                    break;
                }
            }

            _employees[_employees.IndexOf(newEmployee)] = employee;
            _currentEmployee = employee;
        }

        public void SetBox(Box box)
        {
            Box newBox = new Box("00", new Equipment("0001"));

            foreach (var thisBox in _boxes)
            {
                if (thisBox.Key == box.Key)
                {
                    newBox = thisBox;
                    break;
                }
            }

            _boxes[_boxes.IndexOf(newBox)] = box;
        }

        public List<Employee> GetEmployees()
        {
            return _employees;
        }

        public List<Box> GetBoxes()
        {
            return _boxes;
        }
    }
}