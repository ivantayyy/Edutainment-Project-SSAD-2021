using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
namespace Assets
{
    public static class PhotonNetworkMngr
    {

        public static string getPhotonPlayerNickName(PhotonView photonView)
        {
            if (checkPhotonView(photonView))
            {
                return PhotonNetwork.player.NickName;
            }
            return photonView.owner.NickName;
        }
        public static bool checkPhotonView(PhotonView photonView)
        {
            if (photonView.isMine)
                return true;
            return false;
        }
        public static void joinRoom(string roomName, RoomOptions roomOptions, string sceneName)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
            loadLevel(sceneName);
        }
        public static void joinLobby(TypedLobby lobby)
        {
            PhotonNetwork.JoinLobby(lobby);
        }
        public static void createRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
        {
            PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby);
        }
        public static void leaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        public static void loadLevel(string sceneName)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
        public static void setUserId(string userId)
        {
            PhotonNetwork.player.UserId = userId;
        }
        public static void setNickName(string nickName)
        {
            PhotonNetwork.player.NickName = nickName;
        }
        public static int checkPlayerListLength()
        {
            return PhotonNetwork.playerList.Length;
        }
        public static void setPlayerPropertiesForCurrentPlayer(Hashtable playerProperties)
        {
            PhotonNetwork.player.SetCustomProperties(playerProperties);
        }
        public static void setAutomaticallySyncScene(bool Bool)
        {
            PhotonNetwork.automaticallySyncScene = Bool;
        }
        public static bool checkIsMasterClient()
        {
            return PhotonNetwork.isMasterClient;
        }
        public static bool checkIfPlayerIsMasterClient(PhotonPlayer player)
        {
            return player.isMasterClient;
        }
        public static PhotonPlayer getPlayerFromPlayerlist(int i)
        {
            return PhotonNetwork.playerList[i];
        }
        public static object getPlayerPropertyForSpecificPlayer(PhotonPlayer player, string propertyName)
        {
            return player.CustomProperties[propertyName];
        }
        public static void connectUsingSettings(string setting)
        {
            PhotonNetwork.ConnectUsingSettings(setting);
        }
        public static void instantiatePlayer(string name, Vector2 position, Quaternion rotation, byte group)
        {
            PhotonNetwork.Instantiate(name, position, rotation, group);
        }
        public static string getRoomName()
        {
            return PhotonNetwork.room.Name;
        }
        public static int getPing()
        {
            return PhotonNetwork.GetPing();
        }
        public static void setMasterClient(PhotonPlayer player)
        {
            PhotonNetwork.SetMasterClient(player);
        }
        public static bool isInRoom()
        {
            return PhotonNetwork.inRoom;
        }
    }

}
