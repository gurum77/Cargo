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

        string playerID = inputIDField.textComponent.text;

        // 아무것도 입력을 안했으면 경고메세지 보여주고 리턴
        if (playerID.Length == 0)
        {
            messageText.text = "Enter your ID...";
            return;
        }

        // 이미 존재하는 ID인지 검사
        if(leaderBoard.IsExistPlayerID(playerID))
        {
            // 있다면 다시 입력
            inputIDField.textComponent.text = "";
            inputIDField.textComponent.enabled = true;
            inputIDField.placeholder.enabled = true;

            messageText.text = "It already exists...";
        }
        // 없는 ID라면 저장하고 canvas를 삭제
        else
        {
            messageText.text = "Hi! " + playerID;
            LeaderBoard.SavePlayerID(playerID);
            GameObject.Destroy(gameObject, 1);
            return;
        }
    }
}
