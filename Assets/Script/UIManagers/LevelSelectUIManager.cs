using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class LevelSelectUIManager : Photon.PunBehaviour
    {
        // Start is called before the first frame update
        public GameObject Master, Client;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (PhotonNetworkMngr.isInRoom())
            {
                if (PhotonNetwork.isMasterClient)
                {
                    Master.SetActive(true);
                    Client.SetActive(false);
                }
                else
                {
                    Master.SetActive(false);
                    Client.SetActive(true);
                }
            }
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("room joined");
            base.OnJoinedRoom();
            if (PhotonNetworkMngr.checkIsMasterClient())
            {
                Master.SetActive(true);
                Client.SetActive(false);
            }
            else
            {
                Master.SetActive(false);
                Client.SetActive(true);
            }
        }


    }
}