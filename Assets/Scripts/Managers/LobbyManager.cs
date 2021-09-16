using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject m_UserNameScreen, m_ConnectScreen, m_RoomListScreen, m_RoomNameScreen;

    [SerializeField]
    private GameObject m_CreateUserNameButton;

    [SerializeField]
    private InputField m_UserNameField, m_RoomNameInput;

    [SerializeField]
    private GameObject m_MessagePanel;

    [SerializeField]
    private Text m_MessageText;

    [SerializeField]
    private GameObject m_RoomButtonPrefab;

    private Color m_DefaultTextFieldColor;
    private int m_MinNameLength;
    public List<RoomInfo> m_RoomList;
    public Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "0.0.1";

        m_UserNameScreen.SetActive(false);
        m_ConnectScreen.SetActive(false);
        m_RoomListScreen.SetActive(false);

        m_MessagePanel.SetActive(false);
        m_MessageText.text = string.Empty;

        m_DefaultTextFieldColor = new Color(0.2f, 0.2f, 0.2f, 1f);
        m_MinNameLength = 3;

        m_RoomList = new List<RoomInfo>();
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        base.OnConnectedToMaster();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to Lobby");
        m_UserNameScreen.SetActive(true);

        base.OnJoinedLobby();
    }


    private void ClearRoomList()
    {
      Transform content = m_RoomListScreen.transform.Find("Scroll View/Viewport/Content");
      foreach (Transform obj in content)
          Destroy(obj.gameObject);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);   
    }
    

    #region UIMethods

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        Debug.Log("Loaded Room" + Time.time);
         Transform content = m_RoomListScreen.transform.Find("Scroll View/Viewport/Content");
        
         foreach (RoomInfo roomInfo in roomList)
         {
          
             GameObject newRoomButton = Instantiate(m_RoomButtonPrefab, content) as GameObject;
            
             newRoomButton.transform.Find("Name").GetComponent<Text>().text = roomInfo.Name;
             newRoomButton.transform.Find("Players").GetComponent<Text>().text = roomInfo.PlayerCount + " / " + roomInfo.MaxPlayers;
        
             newRoomButton.GetComponent<Button>().onClick.AddListener(delegate { OnJoinRoomButtonClick(newRoomButton.transform); });
            
         }
        base.OnRoomListUpdate(roomList);
    }
    public void UpdadeClick()
    {
        ClearRoomList();
        UpdateRoomList(m_RoomList);
    }

    public void OnCreateNameButtonClick()
    {
        m_UserNameField.text.Trim();
        if (m_UserNameField.text.Length < m_MinNameLength)
        {
            ShowMessage("The name must be at least " + m_MinNameLength + " letters long");
            m_UserNameField.textComponent.color = Color.red;
            return;
        }

        PhotonNetwork.NickName = m_UserNameField.text;
        m_UserNameScreen.SetActive(false);
        m_ConnectScreen.SetActive(true);
    }

    public void OnUserNameFieldValueChanged()
    {
        m_UserNameField.textComponent.color = m_DefaultTextFieldColor;
    }

    private void ShowMessage(string messageText)
    {
        m_MessagePanel.SetActive(true);
        m_MessageText.text = messageText;
    }

    public void HideMessage()
    {
        m_MessagePanel.SetActive(false);
        m_MessageText.text = string.Empty;
    }

    public void OnCreateRoomClick()
    {
        m_ConnectScreen.SetActive(false);
        m_RoomNameScreen.SetActive(true);
    }
    
    public void CreateRoomName()
    {
        PhotonNetwork.CreateRoom(m_RoomNameInput.text, new RoomOptions { MaxPlayers = 2 }, null);
    }
    public void OnJoinRoomClick()
    {
        m_ConnectScreen.SetActive(false);
        m_RoomListScreen.SetActive(true);
    }

    private void OnJoinRoomButtonClick(Transform button)
    {
        Debug.Log("JOINING ROOM @ " + Time.time);
        string roomName = button.transform.Find("Name").GetComponent<Text>().text;
        PhotonNetwork.JoinRoom(roomName);
    }

    #endregion


    public override void OnJoinedRoom()
    {
        Debug.Log("Joined the room");

        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
