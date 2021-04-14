using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets
{
    public class isTeacherObject : MonoBehaviour
    {
        public bool isTeacher = false;
        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }

}
