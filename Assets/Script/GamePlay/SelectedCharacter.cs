using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets
{
    public class SelectedCharacter : MonoBehaviour
    {
        public string selection = null;

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }


    }

}
