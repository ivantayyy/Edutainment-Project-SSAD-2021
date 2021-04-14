using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isTeacherObject : MonoBehaviour
{
    public bool isTeacher=false;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
