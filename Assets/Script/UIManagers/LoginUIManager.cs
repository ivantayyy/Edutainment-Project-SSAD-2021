using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class LoginUIManager : MonoBehaviour
    {
        public static LoginUIManager instance;

        //Screen object variables
        public GameObject loginUI;
        public GameObject registerUI;

        private void Awake()
        {
            //DEBUG
            if (instance == null)
            {
                instance = this;
                LoginScreen();
            }
            else if (instance != null)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
                LoginScreen();

            }
        }

        /**
         * Displays login screen UI
         */
        public void LoginScreen() //Back button
        {
            clearscreen();
            loginUI.SetActive(true);
        }

        /**
         * Displays register UI.
         */
        public void RegisterScreen() // Register button
        {
            clearscreen();
            registerUI.SetActive(true);
        }

        /**
         * Clears all screens.
         */
        public void clearscreen() //turn off all screens
        {
            loginUI.SetActive(false);
            registerUI.SetActive(false);
        }

        /**
         * Returns to login UI
         */
        public void backButton()
        {
            clearscreen();
            loginUI.SetActive(true);
        }
    }
}