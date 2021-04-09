using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using System.Threading.Tasks;
using System;
using UnityEngine.Events;

public class LoginManager : MonoBehaviour
{
    //Login variables
    [Header("Login")]
    public InputField emailLoginField;
    public InputField passwordLoginField;
    public Text warningLoginText;
    public Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public Dropdown ddClass;
    public Dropdown ddReg;
    public InputField usernameRegisterField;
    public InputField emailRegisterField;
    public InputField passwordRegisterField;
    public InputField passwordRegisterVerifyField;
    public Text warningRegisterText;

    
    void Start()
    {
        FirebaseManager.CheckFirebaseDependencies();
    }

    //helper function for instantiating Photon user
    public void instantiatePhotonUser()
    {
        string userid = FirebaseManager.auth.CurrentUser.UserId;
        PhotonNetwork.player.UserId = userid;

        string username = FirebaseManager.auth.CurrentUser.DisplayName;
        PhotonNetwork.player.NickName = username;
    }

    //Function for the login button
    public async void LoginButton()
    {
        //Call the login coroutine passing the email and password
        string message = await FirebaseManager.LoginAsync(emailLoginField.text, passwordLoginField.text);
        instantiatePhotonUser();
        warningRegisterText.text = message;
    }


    //Dropdown button for Register page
    public void DropDownButtonRegister()
    {
        DropDownCheck(ddReg);
    }
    public void DropDownButtonClass()
    {
        DropDownCheck(ddClass);
    }

    //Visual debug of dropdown button
    private void DropDownCheck(Dropdown dd)
    {
        if (dd.value == 0)
        {
            UnityEngine.Debug.Log("No Dropdown value chosen");
        }
        if (dd.value == 1)
        {
            UnityEngine.Debug.Log("Student chosen");
        }
        if (dd.value == 2)
        {
            UnityEngine.Debug.Log("Teacher chosen");
        }
    }


    //gets acctype from dropdown button
    //3 choices available
    private string getAccType(Dropdown dd)
    {
        string res;
        if (dd.value == 0)
        {
            res = "none";
        }
        else if (dd.value == 1)
        {
            res = "Student";
        }
        else
        {
            res = "Teacher";
        }
        return res;
    }

    private string GetClassSubscribed(Dropdown dd)
    {
        string res;
        if (dd.value == 0)
        {
            res = "none";
        }
        else if (dd.value == 1)
        {
            res = "FS6";
        }
        else if (dd.value == 2)
        {
            res = "FS7";
        }
        else if (dd.value == 3)
        {
            res = "FS8";
        }
        else
        {
            res = "FS9";
        }
        return res;
    }


    //button ui to go to summary report page
    

    //Signout method
    public void SignOutButton()
    {
        FirebaseManager.SignOut();
        UIManager.instance.LoginScreen();
        ClearRegisterFields();
        ClearLoginFields();
    }
    // clears the login an register fields
    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
        //clear dropdown Account info
    }
    public void ClearRegisterFields()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
        //clear dropdown Account info
        ddReg.value = 0;
    }

    public async void RegisterButton()
    {
        string acctype = getAccType(ddReg);
        string classSubscribed = GetClassSubscribed(ddClass);

        if (acctype == "none")
        {
            UnityEngine.Debug.Log("No Account inputted in dropdown");
            warningLoginText.text = "Please select a valid account type in the Dropdown menu";
        }
        else if (classSubscribed == "none")
        {
            UnityEngine.Debug.Log("No class inputted in dropdown");
            warningLoginText.text = "Please select a valid account type in the Dropdown menu";
        }
        else if (usernameRegisterField.text == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }else if ( await FirebaseManager.CheckUsernameExistsInDatabaseAsync(usernameRegisterField.text))
        {
            warningRegisterText.text = "username already exists";
        }
        else
        {
            UnityEngine.Debug.Log("reached1");
            //Call the register coroutine passing the email, password, and username
            Task<string> messageTask = FirebaseManager.RegisterAsync(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text, acctype, classSubscribed);
            string message = await messageTask;
            UnityEngine.Debug.Log("reached5");
            warningRegisterText.text = message;
        }
    }




    


    // Update is called once per frame

    void Update()
    {
        
    }
}
