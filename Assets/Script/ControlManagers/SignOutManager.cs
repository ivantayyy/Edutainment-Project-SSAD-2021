using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignOutManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void SignOutButton()
    {
        FirebaseManager.SignOut();
    }
}
