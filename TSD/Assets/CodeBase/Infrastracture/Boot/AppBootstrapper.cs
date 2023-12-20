using CodeBase.Infrastracture.Datas;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Infrastracture.Boot
{
    public class AppBootstrapper : MonoBehaviour
    {
        [SerializeField] private Programm _programm;
        [SerializeField] private SaveLoadService _saveLoad;

        private void Awake()
        {
            _saveLoad.Init();
            Init();
        }


        public void Init()
        {
            _programm.Init(_saveLoad);
            _programm.Work();
        }

        private void RunApplication()
        {
            _programm.GameObject().SetActive(true);
        }
    }
}