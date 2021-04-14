using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomLobbyUIManager : MonoBehaviour
{
    public static CustomLobbyUIManager instance;

    public GameObject createQuestionUI;
    public GameObject teacherShareUI;
    

    void Start()
    {

        if (instance == null)
        {
            instance = this;
            clearscreen();
            createQuestionUI.SetActive(true);
        }
    }

    // Update is called once per frame
    public void teacherShare()
    {
        clearscreen();
        teacherShareUI.SetActive(true);
    }
    public void back()
    {
        clearscreen();
        createQuestionUI.SetActive(true);
    }
    public void clearscreen()
    {
        createQuestionUI.SetActive(false);
        teacherShareUI.SetActive(false);
    }
    public void doneButton()
    {
        Destroy(GameObject.Find("TeacherMenuUIManager"));
        SceneManager.LoadScene("Teacher Menu");

    }
}
