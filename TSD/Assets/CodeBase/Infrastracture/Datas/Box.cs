using System;
using Unity.VisualScripting;

namespace CodeBase.Infrastracture.Datas
{
    [Serializable]
    public class Box
    {
        [Serialize] public bool Busy => _busy;
        [Serialize] public string Key => _key;

        private Equipment _equipment;
        [Serialize] public Equipment Equipment => _equipment;
        private string _key;
        private bool _busy;

        public Box(string key, Equipment equipment)
        {
            _key = key;
            _equipment = new(equipment.SerialNumber);
            _busy = false;
        }

        public Equipment GetEquipment()
        {
            Equipment oldEquipment = new(_equipment.SerialNumber);
            _equipment = new("0");
            _busy = true;
            return oldEquipment;
        }

        public void SetEquipment(Equipment equipment)
        {
        }
    }
}