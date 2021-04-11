using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

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

    //Functions to change the login screen UI
    public void LoginScreen() //Back button
    {
        clearscreen();
        loginUI.SetActive(true);
    }
    public void RegisterScreen() // Regester button
    {
        clearscreen();
        registerUI.SetActive(true);
    }

    public void clearscreen() //turn off all screens
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
    }
    
}