using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; 

namespace Com.MyCompany.MyGame

{

    public class NetworkController : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields 

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        #endregion



        #region Private Fields  

        bool isConnecting;

        string gameVersion = "1";

        #endregion 

        #region MonoBehaviour CallBacks 

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true; 
        }


        // Start is called before the first frame update
        void Start()
        {
            Connect();
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

        }


        #endregion

        #region Public Methods 

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        public void Connect()

        {
            isConnecting = true;
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom(); 
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings(); 
            }
        }

        #region MonoBehaviorPunCallbacks Callbacks 

        public override void OnConnectedToMaster()
        {
            Debug.Log("Pun Basics Tutorial/Launcher: OnconnectedToMaster() was called by Pun");

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();

            }
            
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("Pun Basics Tutorial/Launcher: OnDisconnected was called by Pun with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Pun Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");


                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("Room for 1");
            }
        }


        #endregion



        // Update is called once per frame
        void Update()
        {

        }
    }
}
        #endregion