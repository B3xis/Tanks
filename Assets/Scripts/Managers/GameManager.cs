using Photon.Pun;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region PUBLIC
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f;           
    public Text m_MessageText;
    public Text m_PingText;
    public Transform m_SpawnPoint_1;
    public Transform m_SpawnPoint_2;
    public GameObject m_TankPrefab;         
    public TankManager m_Tank;
    #endregion

    #region PRIVATE
    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;       
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;
    #endregion


    private void Awake()
    {
        PhotonNetwork.SendRate = 60;            
        PhotonNetwork.SerializationRate = 60;
    }

    [System.Obsolete]
    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllTanks()
    {
        Transform currentSpawnPoint = (PhotonNetwork.IsMasterClient) ? m_SpawnPoint_1 : m_SpawnPoint_2;

        m_Tank.m_Instance = 
            PhotonNetwork.Instantiate(m_TankPrefab.name, currentSpawnPoint.position, currentSpawnPoint.rotation) as GameObject;
        m_Tank.m_PlayerColor = (PhotonNetwork.IsMasterClient) ? Color.red : Color.blue;
        m_Tank.m_PlayerName = PhotonNetwork.LocalPlayer.NickName;
        m_Tank.Setup();
    }


    [System.Obsolete]
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        m_MessageText.text = string.Empty;

        while (true/*!OneTankLeft()*/)
        {
            m_PingText.text = "Ping: " + PhotonNetwork.GetPing();
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        m_RoundWinner = GetRoundWinner();

        if (m_RoundWinner != null)
        {
            m_RoundWinner.m_Wins++;
        }

        m_GameWinner = GetGameWinner();

        m_MessageText.text = EndMessage();

        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;
        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        return null;
    }


    private TankManager GetGameWinner()
    {
        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";
        return message;
    }


    private void ResetAllTanks()
    {
    }


    private void EnableTankControl()
    {
    }


    private void DisableTankControl()
    {
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        // When current player left the gameRoom
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player {0} entered room", newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player {0} left room", otherPlayer.NickName);
    }
}