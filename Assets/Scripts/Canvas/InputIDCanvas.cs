using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputIDCanvas : MonoBehaviour {

    LeaderBoard leaderBoard;
    Text messageText;
    InputField inputIDField;

	// Use this for initialization
	void Start () {
        leaderBoard = GameObject.Find("Leaderboards").GetComponent<LeaderBoard>();
        messageText = transform.Find("InputField/MessageText").GetComponent<Text>();
        inputIDField = transform.Find("InputField").GetComponent<InputField>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // OK를 누르면 호출
    public void OnOKButtonClicked()
    {
        if (inputIDField == null)
            return;
        if (messageText == null)
            return;
        if (leaderBoard == null || leaderBoard.leaderBoards.Length == 0)
            return;


        messageText.text = "Checking...";

        string playerID = inputIDField.textComponent.text;

        // 아무것도 입력을 안했으면 경고메세지 보여주고 리턴
        if (playerID.Length == 0)
        {
            messageText.text = "Enter your ID...";
            return;
        }

        // 이미 존재하는 ID인지 검사
        StartCoroutine(CheckNewPlayerID(playerID));
    }

    IEnumerator CheckNewPlayerID(string playerID)
    {
        bool isExist = false;
        for (int i = 0; i < leaderBoard.leaderBoards.Length; ++i)
        {
            WWW www = leaderBoard.leaderBoards[i].GetWWW_GetSingleScore(playerID);
            do
            {
                yield return true;
            }
            while (!www.isDone);
            

            // 웹 접속 실패나 text가 있다면 id가 있는것으로 판단
            if (www.error != null || www.text.Length > 0)
                isExist = true;
        }


        if (isExist)
        {
            // 있다면 다시 입력
            if(inputIDField != null && messageText != null)
            {
                inputIDField.textComponent.text = "";
                inputIDField.textComponent.enabled = true;
                inputIDField.placeholder.enabled = false;

                messageText.text = "It already exists...";
            }
            
        }
        // 없는 ID라면 저장하고 canvas를 삭제
        else
        {
            if(messageText != null)
                messageText.text = "Hi! " + playerID;
            LeaderBoard.SavePlayerID(playerID);
            GameObject.Destroy(gameObject, 1);
        }
        
    }
}
