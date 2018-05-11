using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LeaderboardCanvas : MonoBehaviour {

    public LeaderBoard leaderBoard;
    public RectTransform nameCardPrefab;
    public Dropdown dropdown;
    public GameObject contents;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        dropdown.options.Clear();

        for(int i = 0; i < (int)GameModeController.GameMode.eUnknown; ++i)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = GameModeController.GetGameModeDisplayName((GameModeController.GameMode)i);
            dropdown.options.Add(data);
        }

        dropdown.value = 0;
    }

    private void FixedUpdate()
    {
        
    }

    // drop down 이 변경되면 그린다.
    public void OnDropdownChanged()
    {
        if (contents == null)
            return;

        if (dropdown == null)
            return;

        if (leaderBoard == null)
            return;

        // 점수 로딩
        dreamloLeaderBoard lb = leaderBoard.leaderBoards[dropdown.value];
        lb.LoadScoresNoneSync();

        
        dreamloLeaderBoard.Score[] scores = lb.ToScoreArray();
        if (scores == null)
            return;

        foreach (var score in scores)
        {
            GameObject.Instantiate(nameCardPrefab, contents.transform);
        }
        
        
    }
}
