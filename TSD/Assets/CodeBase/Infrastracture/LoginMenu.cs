using UnityEngine;

namespace CodeBase.Infrastracture
{
    public class LoginMenu:MonoBehaviour, ILoginMenu
    {
        public bool CheckCredentials(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public void DisplayWarning(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}