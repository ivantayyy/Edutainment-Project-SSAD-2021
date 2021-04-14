using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets
{
    /**
     * SignOutManager handles user sign out action.
     */
    public class SignOutManager : MonoBehaviour
    {
        /**
         * Start() is called before the first frame update. Signs user out.
         */
        public void SignOutButton()
        {
            FirebaseManager.SignOut();
            Debug.Log("Successfully signed out");
            SceneManager.LoadScene("Login");
        }
    }

}
