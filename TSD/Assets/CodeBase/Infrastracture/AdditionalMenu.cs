using CodeBase.Infrastracture;
using CodeBase.Infrastracture.Datas;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AdditionalMenu:MonoBehaviour
{
    
    [SerializeField] private Button _adminButton;
    [SerializeField] private Button _managerButton;
    [SerializeField] private Button _historyButton;
    
    [SerializeField] private AdminPanel _adminPanel;
    [SerializeField] private ManagersPanel _managerPanel;
    [SerializeField] private InfoViwer _historyPanel;
    
    private SaveLoadService _saveLoadService;
    private Programm _programm;
    
    private bool _isAdminEnter = false;
    private bool _isManagerEnter = false;


    public void Init(SaveLoadService saveLoadService,Programm programm,WarningPanel warningPanel)
    {
        _saveLoadService = saveLoadService;
        _programm=programm;
        
        _managerPanel.Init(_saveLoadService,warningPanel);
        _historyPanel.Init(_saveLoadService);
        _adminPanel.Init(_saveLoadService);
        
        AddListeners();
        SentLogMessage("Программа инизиализированна");
    }
    
    public void Work()
    {
        _adminPanel.SwithState(false);
        _managerPanel.SwithState(false);
        _historyPanel.SwithState(false);
        
        _adminButton.interactable=true;
        _managerButton.interactable = true;
        _historyButton.interactable = true;
        
        
    }
    private void Reset()
    {
        _managerPanel.Reset();
        _adminPanel.Reset();
        _historyPanel.Reset();
        OnEnter();
        Work();
    }
    
    private void EnterAdmin(bool isAdmin)
    {
        if (isAdmin)
        {
            _isAdminEnter = true;
            _isManagerEnter = false;
        }
        else
        {
            _isAdminEnter = false;
            _isManagerEnter = true;
        }
        
        OnEnter();
       
    }
    
    private void OnEnter()
    {
        if (_isManagerEnter)
        {
            _managerButton.gameObject.SetActive(true);
            _historyButton.gameObject.SetActive(true);
            _adminButton.gameObject.SetActive(false);
        }
        else 
        {
            _managerButton.gameObject.SetActive(true);
            _historyButton.gameObject.SetActive(true);
            _adminButton.gameObject.SetActive(true);
        }
    }

    
    private void OpenManagerPanel()
    {
        SwitchButtonState(false,false);
        _managerPanel.SwithState(true);
    }
    
    private void OpenAdminPanel()
    {
        SwitchButtonState(false,false);
        _adminPanel.SwithState(true);
    }
    private void OpenHistoryPanel()
    {
        SwitchButtonState(false,false);
        _historyPanel.SwithState(true);
    }
    
    private void SwitchButtonState(bool isAdminActive,bool isManagerActive)
    {
        _adminButton.gameObject.SetActive(isAdminActive);
        _managerButton.gameObject.SetActive(isManagerActive);
        _historyButton.gameObject.SetActive(isManagerActive);
    }
    
    private void SentLogMessage(string message)
    {
        _saveLoadService.SentLogInfo(message);
    }
    
    private void AddListeners()
    {
        _managerButton.onClick.AddListener(OpenManagerPanel);
        _adminButton.onClick.AddListener(OpenAdminPanel);
        _historyButton.onClick.AddListener(OpenHistoryPanel);
        _programm.OnEnterAdmin+=EnterAdmin;
        _adminPanel.OnExit+=Reset;
        _managerPanel.OnExit+=Reset;
        _historyPanel.OnExit+=Reset;
    }

    private void RemuveListeners()
    {
        _managerButton.onClick.RemoveListener(OpenManagerPanel);
        _adminButton.onClick.RemoveListener(OpenAdminPanel);
        _historyButton.onClick.RemoveListener(OpenHistoryPanel);
        _programm.OnEnterAdmin-=EnterAdmin;
        _adminPanel.OnExit-=Reset;
        _managerPanel.OnExit-=Reset;
        _historyPanel.OnExit-=Reset;
    }

    

    private void OnDisable()
    {
        RemuveListeners();
    }
}