using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonNetworkManager:Photon.MonoBehaviour
{
    public string getPhotonPlayerNickName()
    {
        if(checkPhotonView())
        {
            return PhotonNetwork.player.NickName;
        }

        return photonView.owner.NickName;
    }
    public bool checkPhotonView()
    {
        if (photonView.isMine)
            return true;
        return false;
    }


}
