using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets
{
    public class AssignmentManager : MonoBehaviour
    {
        private List<string> assignmentList;
        private string assignments;
        public Text assignmentText;
        [SerializeField] private InputField joinGameInput;
        // Start is called before the first frame update
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
        public void chooseAssignmentBut()
        {
            PhotonNetworkMngr.joinRoom(joinGameInput.text, new RoomOptions() { MaxPlayers = 2 }, "ChooseCharacters");

        }
        public void backButton()
        {
            Destroy(GameObject.Find("modeObject"));
            PhotonNetworkMngr.loadLevel("Main Menu");
        }

    }

}
