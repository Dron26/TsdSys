using System;
using Unity.VisualScripting;

namespace CodeBase.Infrastracture.Datas
{
    [Serializable]
    public class Employee
    {
        [Serialize] public string Login { get; set; }
        [Serialize] public string Pass { get; set; }

        [Serialize] public bool HaveEquipment => _haveEquipment;

        [Serialize] public bool HaveBox => _haveBox;

        private bool _haveBox;
        private bool _haveEquipment;
        public string Permission => _permission;
        private string _permission;

        public Equipment Equipment => _equipment;
        public Box Box => _box;
        private Equipment _equipment;
        private Box _box;

        public Employee(string login, string pass)
        {
            Login = login;
            Pass = pass;
        }

        public Equipment GetEquipment()
        {
            Equipment oldEquipment = new(_equipment.SerialNumber);
            _equipment = new("0");
            _haveEquipment = false;
            return oldEquipment;
        }

        public Box GetBox()
        {
            Box oldBox = new(_box.Key, GetEquipment());
            _box = new("0", new("0"));
            _haveBox = false;
            return oldBox;
        }

        public void SetBox(Box box)
        {
            _box = new(box.Key, box.Equipment);
            _haveBox = true;
            _equipment = _box.GetEquipment();
            _haveEquipment = true;
        }
        
        public void SetPermision(string permission)
        {
            _permission = permission;
        }
    }
}