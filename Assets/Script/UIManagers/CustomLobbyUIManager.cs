using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{

    public class CustomLobbyUIManager : MonoBehaviour
    {
        public static CustomLobbyUIManager instance;

        public GameObject createQuestionUI;
        public GameObject teacherShareUI;

        /**
         * Start() is called before the first frame update.
         * Displays create question scene.
         */
        void Start()
        {

            if (instance == null)
            {
                instance = this;
                clearscreen();
                createQuestionUI.SetActive(true);
            }
        }

        /**
         * Displays sharing assignment scene.
         */
        public void teacherShare()
        {
            clearscreen();
            teacherShareUI.SetActive(true);
        }

        /**
         * Returns to create question scene.
         */
        public void back()
        {
            clearscreen();
            createQuestionUI.SetActive(true);
        }

        /**
         * Clears screen
         */
        public void clearscreen()
        {
            createQuestionUI.SetActive(false);
            teacherShareUI.SetActive(false);
        }

        /**
         * Returns to teacher menu
         */
        public void doneButton()
        {
            Destroy(GameObject.Find("TeacherMenuUIManager"));
            SceneManager.LoadScene("Teacher Menu");

        }
    }
}