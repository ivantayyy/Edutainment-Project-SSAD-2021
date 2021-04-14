using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using System.Threading.Tasks;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


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

    /*!\brief check firebase dependencies
     *          
     */
    void Start()
    {
        Debug.Log("LoginManager instantiated");

        FirebaseManager.CheckFirebaseDependencies();
    }

    /*!\brief Function for the login button
     *          
     *   Check input fields for email and password before authenticating w firebase
     *   display error message if invalid input/ not a registered user/ wrong password
     *   login using firebase
     *   load student menu/ teacher menu depending on account type
     */
    //Function for the login button
    public async void LoginButton()
    {
        if (emailLoginField.text == "")
        {
            //clear previous text
            warningLoginText.text = "";
            warningLoginText.text = "No Email Provided";
            return;
        }else if(passwordLoginField.text == "")
        {
            warningLoginText.text = "";
            warningLoginText.text = "No Password Provided";
            return;
        }
        //Call the login coroutine passing the email and password
        var LoginTask = FirebaseManager.LoginAsync(emailLoginField.text, passwordLoginField.text);
        string message = await LoginTask;
        Debug.Log("message is" + message);
        warningLoginText.text = "";
        warningLoginText.text = message;
        Debug.Log("Login Task on Login Button() completed sucessfully proceeding to instantiate photon user");
        //instantiatePhotonUser();
        // wait for 2 seconds
        //redirects to next screen
        var isTeacherTask = FirebaseManager.isTeacher();
        bool isTeacher = await isTeacherTask;
        if (isTeacher)
        {
            //go to teacher menu
            SceneManager.LoadScene("Teacher Menu");
            Debug.Log("The user is teacher");
        }
        else
        {
            new WaitForSeconds(2);
            SceneManager.LoadScene("Main Menu");
        }
            //UnityEngine.Debug.Log("Login reached 4");

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
        LoginUIManager.instance.LoginScreen();
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

    /*!\brief register new user function
     *          
     *    Check inputs for valid email and password to register new account
     *    else display error message
     *    store new user in firebase database
     */
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
            //Call the register coroutine passing the email, password, and username
            Task<string> messageTask = FirebaseManager.RegisterAsync(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text, acctype, classSubscribed);
            string message = await messageTask;
            warningRegisterText.text = message;
        }
    }


}
