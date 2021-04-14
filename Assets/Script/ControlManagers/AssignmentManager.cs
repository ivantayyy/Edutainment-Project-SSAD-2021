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
        private List<string> assignmentList;
        private string assignments;
        public Text assignmentText; /** To display List of Assignments */
        [SerializeField] private InputField joinGameInput;

        /**
        * Start() is called before the first frame update.
        * This function fetches the assignment list for the student using their userId.
        */
        public async void Start()
        {
            string uid = FirebaseManager.auth.CurrentUser.UserId;
            assignmentList = await FirebaseManager.getAssignmentName(uid);
            foreach (string i in assignmentList)
            {
                //Debug.Log("main");
                Debug.Log("get assignment " + i);
                assignments += i + "\n";
                Debug.Log("assigns =" + assignments);
            }
            assignmentText.text = assignments;
        }

        /**
        * A public function that called when user clicks on 'Choose Assignment'.
        * It redirects user to join room that is specified.
        */
        public void chooseAssignmentBut()
        {
            PhotonNetworkMngr.joinRoom(joinGameInput.text, new RoomOptions() { MaxPlayers = 2 }, "ChooseCharacters");

        }

        /**
        * A public function that brings user back to Main Menu.
        */
        public void backButton()
        {
            Destroy(GameObject.Find("modeObject"));
            PhotonNetworkMngr.loadLevel("Main Menu");
        }

    }

}
