using Core.StartUp;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GUI
{
    public class LoginButton : MonoBehaviour
    {
        public ClientStartUp clientStartUp; 
        public Button loginButton;
        
        private void Start()
        {
            loginButton.onClick.AddListener(clientStartUp.OnLoginUserButtonClick);
        }
    }
}