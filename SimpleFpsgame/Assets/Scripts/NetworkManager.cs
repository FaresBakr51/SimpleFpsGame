using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using agora_gaming_rtc;
using UnityEngine.Android;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    private IRtcEngine mRtcEngine = null;
    private string AppID = "ab4ec26b1b25490b8a34855f45e0f961";
    public Button playbutt;
    private void Start()
    {
        Connect();
        checkpremission();
    }

    void checkpremission()
    {
#if (UNITY_2018_3_OR_NEWER)
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {

        }
        else
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
        mRtcEngine = IRtcEngine.GetEngine(AppID);
    }
    void Joinchannel()
    {
        mRtcEngine.JoinChannel(null, "extra", 0);
    }
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public void Play()
    {
        PhotonNetwork.JoinRandomRoom();       
    }
    void OnApplicationQuit()
    {
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();
        }
    }
    public override void OnConnectedToMaster()
    {
        playbutt.interactable = true;
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {       
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
            Joinchannel();           
        }
        
    }
}
