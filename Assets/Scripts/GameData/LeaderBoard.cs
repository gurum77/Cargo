using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 리더 보드를 컨트롤 한다.
public class LeaderBoard : MonoBehaviour {

    public dreamloLeaderBoard[] leaderBoards;
    public GameObject inputIDCanvasPrefab;
    string playerID;

    static public string GetPlayerID()
    {
        return PlayerPrefs.GetString("PlayerID", "");
    }

    static public void SavePlayerID(string id)
    {
        PlayerPrefs.SetString("PlayerID", id);
    }

    public void SetScore(int leaderBoardIndex, int score)
    {
        if (leaderBoards.Length <= leaderBoardIndex)
            return;

        leaderBoards[leaderBoardIndex].AddScore(playerID, score);
    }
    
    private void Awake()
    {
       
    }
    // Use this for initialization
    void Start () {
        playerID = LeaderBoard.GetPlayerID();

        // ID가 없다면 입력을 받는다.
        if (playerID.Length == 0)
        {
            if (inputIDCanvasPrefab == null)
            {
                Debug.Log("You need a inputIDCanvasPrefab");
                return;
            }

            Instantiate(inputIDCanvasPrefab);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
