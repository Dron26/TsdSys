using System;
using Unity.VisualScripting;

namespace CodeBase.Infrastracture.Datas
{
    [Serializable]
    public class Equipment
    {
        [Serialize] public string ShortSerial => _shortSerial;
        [Serialize] public string SerialNumber => _serialNumber;

        private string _shortSerial;
        private string _serialNumber;

        public Equipment(string serialNumber)
        {
            _serialNumber = serialNumber;
            _shortSerial = _serialNumber[^4..];
        }
    }
}