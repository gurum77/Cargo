using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LeaderboardCanvas : MonoBehaviour {

    public LeaderBoard leaderBoard;
    public RectTransform nameCardPrefab;
    public Dropdown dropdown;
    public GameObject contents;
    bool isWaitingDownload;
    int myRanking;
    int lastRanking;
    public Text myRankingText;
    public Sprite mobileSprite;
    public Sprite pcSprite;

    dreamloLeaderBoard GetDreamloLeaderBoard()
    {
        if (contents == null)
            return null;

        if (dropdown == null)
            return null;

        if (leaderBoard == null)
            return null;

        return leaderBoard.leaderBoards[dropdown.value];
    }
      

    // Use this for initialization
    void Start () {
        isWaitingDownload = false;
        myRanking = -1;

        dropdown.options.Clear();

        for (int i = 0; i < (int)GameModeController.GameMode.eUnknown; ++i)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = GameModeController.GetGameModeDisplayName((GameModeController.GameMode)i);
            dropdown.options.Add(data);
        }

        dropdown.value = 0;
        OnDropdownChanged();
    }
	
	// Update is called once per frame
	void Update () {
		
        // download 대기중인 경우에는 download가 완료 되었는지 확인해서
        // 완료되면 리스트를 뿌리고 대기를 푼다.
        if(isWaitingDownload)
        {
            dreamloLeaderBoard lb = GetDreamloLeaderBoard();
            if(lb && lb.IsExistHighScores())
            {
                DisplayRanking();
                isWaitingDownload = false;
            }
        }
        else
        {
            // esc 키 누르면 초기화면으로
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(Define.Scene.Playground);
            }
        }
	}


    // drop down 이 변경되면 그린다.
    public void OnDropdownChanged()
    {
        dreamloLeaderBoard lb = GetDreamloLeaderBoard();
        if (lb == null)
            return;

        isWaitingDownload = true;

        // 점수 로딩
        lb.LoadScores();
    }

    // ranking 을 표시한다.
    void DisplayRanking()
    {
        // 전체 ranking 리스트
        MakeListItemsWithScores();

        // my ranking을 표시한다.
        DisplayMyRanking();
    }

    void DisplayMyRanking()
    {
        if (myRankingText == null)
            return;


        StringBuilder sb = new StringBuilder();

        sb.Append("ID : ");
        sb.Append(LeaderBoard.GetPlayerID());
        sb.Append("   Rank : ");
        if (myRanking < 0)
            sb.Append("No ranking");
        else
            sb.Append(myRanking.ToString());
        sb.Append("/");
        sb.Append(lastRanking.ToString());


        myRankingText.text = sb.ToString();
    }

    // 전체 랭킹 리스트를 만들면서 내 랭킹도 확인
    void MakeListItemsWithScores()
    {
        myRanking = -1;

        if (contents == null)
            return;

        if (dropdown == null)
            return;

        if (leaderBoard == null)
            return;

        int count = contents.transform.GetChildCount();
        for(int i = 0; i < count; ++i)
        {
            GameObject.Destroy(contents.transform.GetChild(i).gameObject);
        }

        dreamloLeaderBoard lb = leaderBoard.leaderBoards[dropdown.value];
        List<dreamloLeaderBoard.Score> scores = dropdown.value == 1 ? lb.ToListLowToHigh() : lb.ToListHighToLow();
        if (scores == null)
            return;

        int num = 1;
        string playerID = LeaderBoard.GetPlayerID();

        foreach (var score in scores)
        {
            // 내 랭킹 확인
            if(score.playerName == playerID)
            {
                myRanking = num;
            }

            RectTransform root = GameObject.Instantiate(nameCardPrefab, contents.transform);

            var mainPanel = root.GetChild(0);
            Text statusText = mainPanel.Find("AvatarPanel/StatusText").GetComponent<Text>();
            Text nameText = mainPanel.Find("NameAndLocationPanel/NameText").GetComponent<Text>();
            Text locationText = mainPanel.Find("NameAndLocationPanel/LocationText").GetComponent<Text>();
            Image avater   = mainPanel.Find("AvatarPanel").GetComponent<Image>();
            if (IsMobileScore(score))
            {
                avater.sprite = mobileSprite;
                avater.color = new Color(1, 1, 0);
            }
            else
            {
                avater.sprite = pcSprite;
                avater.color = new Color(0, 1, 0);
            }


            statusText.text = "";
            nameText.text = "[" + num.ToString() + "]" + "  " + score.playerName;
            locationText.text = score.score.ToString();

            num++;
        }

        lastRanking = num - 1;
    }

    // mobile 에서 기록한 점수인지?
    bool IsMobileScore(dreamloLeaderBoard.Score score)
    {
        return true;
    }
}
