using System;
using CodeBase.Infrastracture.Datas;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastracture
{
    public class EquipmentRegistrationMenu : MonoBehaviour
    {
        [SerializeField] private ResultPanel _resultPanel;
        [SerializeField] private Button _buttonApply;
        [SerializeField] private Button _buttonApplyResult;

        [SerializeField] private EquipmentValidator _validator;
        [SerializeField] private Image _CheckUpKey;
        [SerializeField] private Image _CheckDownKey;
        [SerializeField] private Image _CheckUpTsd;
        [SerializeField] private Image _CheckDownTsd;

        [SerializeField] private SentData _sentData;

        public Action OnApply;
        public Action OnReturnEquipment;
        public Action OnRegistrationEnd;
        public Action OnApplyRegistration;
        private SaveLoadService _saveLoadService;
        

        public void Init(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _validator.Init(_saveLoadService);
            _resultPanel.Init(_saveLoadService);

            AddListeners();
        }

        public void Work()
        {
            _validator.Work();
            _resultPanel.Work();

            _CheckDownKey.enabled = true;
            _CheckUpKey.enabled = false;
            _CheckDownTsd.enabled = true;
            _CheckUpTsd.enabled = false;
            _buttonApply.interactable = false;
        }

        public void Reset()
        {
            _validator.Reset();
            _resultPanel.Reset();
            _CheckDownKey.enabled = true;
            _CheckUpKey.enabled = false;
            _CheckDownTsd.enabled = true;
            _CheckUpTsd.enabled = false;
            _buttonApply.interactable = false;
        }

        private void OnResultButtonClick()
        {
            OnApplyRegistration.Invoke();
            _resultPanel.SwithState(false);
            Reset();
        }

        private void OnGetTsd()
        {
            _CheckDownTsd.enabled = false;
            _CheckUpTsd.enabled = true;
        }

        private void OnGetEquipment()
        {
            _buttonApply.interactable = true;
            _CheckDownKey.enabled = false;
            _CheckUpKey.enabled = true;
        }

        private void OnApplyButtonClick()
        {
            _validator.SwithState(false);
            OnApply?.Invoke();
            OnRegistrationEnd?.Invoke();
            _resultPanel.SwithState(true);
            _resultPanel.SetData();
        }

        public void SwitchValidatorState(bool state)
        {
            _validator.SwithState(state);
        }

        private void AddListeners()
        {
            _validator.OnTakeKey += OnGetEquipment;
            _validator.OnTakeTsd += OnGetTsd;
            _buttonApply.onClick.AddListener(OnApplyButtonClick);
            _buttonApplyResult.onClick.AddListener(OnResultButtonClick);
        }

        private void RemuveListeners()
        {
            _validator.OnTakeKey -= OnGetEquipment;
            _validator.OnTakeTsd -= OnGetTsd;
            _buttonApply.onClick.RemoveListener(OnApplyButtonClick);
            _buttonApplyResult.onClick.RemoveListener(OnResultButtonClick);
        }

        private void OnDisable()
        {
            RemuveListeners();
        }
    }
}