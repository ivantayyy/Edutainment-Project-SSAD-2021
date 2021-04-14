using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets
{
    public class SignOutManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public void SignOutButton()
        {
            FirebaseManager.SignOut();
            Debug.Log("Successfully signed out");
            SceneManager.LoadScene("Login");
        }
    }

}
