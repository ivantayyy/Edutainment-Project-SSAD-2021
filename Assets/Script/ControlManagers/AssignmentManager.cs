using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets
{
    /**
    *  AssignmentManager manages the List of Assignments for students on the on the Assignment scene.
    */

    public class AssignmentManager : MonoBehaviour
    {

        public GameObject classElement;
        public Text assignmentText;
        public Transform AssignmentBoardContent;

        // Start is called before the first frame update

        public async void Start()
        {
            string uid = FirebaseManager.auth.CurrentUser.UserId;
            /*assignmentList = await FirebaseManager.getAssignmentName(uid);
            foreach (string i in assignmentList)
            {
                //Debug.Log("main");
                Debug.Log("get assignment " + i);
                assignments += i + "\n";
                Debug.Log("assigns =" + assignments);
            }
            assignmentText.text = assignments;*/
            LoadAssignmentList(uid);
        }

        public void chooseAssignmentBut(string roomName)

        {
            PhotonNetworkMngr.joinRoom(roomName, new RoomOptions() { MaxPlayers = 2 }, "ChooseCharacters");

        }

        public async void LoadAssignmentList(string uid)
        { 
            List<string> AssignmentList = await FirebaseManager.getAssignmentName(uid);
            ClassElement element = new ClassElement();
            foreach (Transform child in this.AssignmentBoardContent.transform)
            {
                UnityEngine.Debug.Log("reached before transform loop");
                Destroy(child.gameObject);
                UnityEngine.Debug.Log("reached after transform loop");
            }
            foreach (string assignmentName in AssignmentList)
            {
                
                //get instantiated gameobject
                GameObject classBoardElement = Instantiate(classElement, this.AssignmentBoardContent);
                //add and get script to gameobject
                element = classBoardElement.AddComponent<ClassElement>();
                //assign script's text to gameobject's child's text
                element.TextName = classBoardElement.transform.GetChild(0).GetComponent<Text>();
                //edit text
                element.NewElement(assignmentName);
                //add onclick function to gameobject which will load student names
                classBoardElement.GetComponent<Button>().onClick.AddListener(async delegate
                {
                    string roomName = classBoardElement.transform.GetChild(0).GetComponent<Text>().text;
                    chooseAssignmentBut(roomName);

                });
            }
        }

        public void backButton()
        {
            Destroy(GameObject.Find("modeObject"));
            PhotonNetworkMngr.loadLevel("Main Menu");
        }

    }

}
