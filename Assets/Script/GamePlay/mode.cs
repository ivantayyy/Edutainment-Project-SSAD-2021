using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mode : MonoBehaviour
{
    public int modeType = 0;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame

}
