using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Controller;
using Timers;
using ProgressBar;

// 문제 셋트
// 문제와, 보기와, 답을 가진다.
public class QuestionSet
{
    public QuestionSet(string question, string answer1, string answer2, string answer3, int correctAnswer)
    {
        Question = question;
        Answer1 = answer1;
        Answer2 = answer2;
        Answer3 = answer3;
        CorrectAnswer = correctAnswer;
    }

    public string Question
    {get;set;}

    public string Answer1
    { get; set; }

    public string Answer2
    { get; set; }

    public string Answer3
    { get; set; }

    public int CorrectAnswer
    { get; set; }
}

public class GameMode_Math : MonoBehaviour {

    public GameObject leftButton;
    public GameObject jumpButton;
    public GameObject rightButton;
    public Text questionText;
    public Text answer1Text;
    public Text answer2Text;
    public Text answer3Text;
    public float timeOut;
    public ProgressBarBehaviour timeOutProgressbar;

    int questionIndex = -1;
    List<QuestionSet> questionList;
    float elapsedTimeFromStartingTimer;
    
    void Awake()
    {
        MakeQuestionList();

        
    }

    // 이 함수가 호출된다면 time out이다.
    void OnTimeout()
    {
        GameController.Me.Player.ApplyDamage();

        // 문제를 다시 낸다
        SelectQuestion();
        DisplayQuestion();
    }
    // 문제 리스트를 만든다.
    void MakeQuestionList()
    {
        if (questionList == null)
            questionList = new List<QuestionSet>();

        questionList.Add(new QuestionSet("1+1", "1", "2", "3", 2));
        questionList.Add(new QuestionSet("1+2", "1", "2", "3", 3));
        questionList.Add(new QuestionSet("2+2", "3", "4", "5", 2));
    }
    // Use this for initialization
	void Start () {
	    	
	}
	
    
	// Update is called once per frame
	void Update () {
		
	}

    // 제출되어 있는 문제가 있는지?
    bool IsExistQuestion()
    {
        return questionIndex == -1 ? false : true;
    }

    // 문제를 고른다.
    void SelectQuestion()
    {
        questionIndex = Random.Range(0, questionList.Count);
    }

    void FixedUpdate()
    {
        // 풀리지 않은 문제가 없다면 문제를 제출한다.
        if(!IsExistQuestion())
        {
            SelectQuestion();
            DisplayQuestion();
        }

        // 입력
        if (Input.GetKeyDown(Define.Key.Left))
        {
            SelectAnswer(1);
        }
        else if(Input.GetKeyDown(Define.Key.Right))
        {
            SelectAnswer(3);
        }
        else if(Input.GetKeyDown(Define.Key.Space))
        {
            SelectAnswer(2);
        }

        elapsedTimeFromStartingTimer += Time.deltaTime;
        // progress bar 갱신
        DisplayProgressbar();
    }

    void DisplayProgressbar()
    {
        if (timeOutProgressbar == null)
            return;


        timeOutProgressbar.Value = ((timeOut - elapsedTimeFromStartingTimer) / timeOut * 100.0f);
    }

    // 현제 문제를 리턴
    QuestionSet CurrentQuestion
    {

        get 
        {
            if (questionIndex < 0)
                return null;
            if (questionIndex >= questionList.Count)
                return null;

            return questionList[questionIndex]; 
        }
    }

    // 문제를 표시한다.
    void DisplayQuestion()
    { 
        QuestionSet q   = CurrentQuestion;
        if(q == null)
            return;

        if (questionText)
            questionText.text = q.Question;
        if (answer1Text)
            answer1Text.text = q.Answer1;
        if (answer2Text)
            answer2Text.text = q.Answer2;
        if (answer3Text)
            answer3Text.text = q.Answer3;


        // timer를 시작한다.
        TimersManager.SetLoopableTimer(this, timeOut, OnTimeout);
        elapsedTimeFromStartingTimer = 0.0f;
    }

    // 시작할때 버튼들을 숨긴다.
    void OnEnable()
    {
        if (leftButton)
            leftButton.SetActive(false);
        if (jumpButton)
            jumpButton.SetActive(false);
        if (rightButton)
            rightButton.SetActive(false);

        // 사용자 입력을 막는다.
        if(GameController.Me)
        {
            GameController.Me.Player.EnableUserInput = false;
        }

        // 맵을 구성한다.
        if (GameController.Me)
        {
            // 시계 아이템 활성화
            GameController.Me.mapController.EnableItem(Assets.Scripts.Controller.MapBlockProperty.ItemType.eClock, true);

            GameController.Me.mapController.MakeMap();

            // 시계 아이템 비활성화
            GameController.Me.mapController.EnableItem(Assets.Scripts.Controller.MapBlockProperty.ItemType.eClock, false);
        }

        timeOutProgressbar.SetFillerSize(100);
    }

    //  답을 선택한다.
    void SelectAnswer(int answer)
    {
        QuestionSet q = CurrentQuestion;
        if(q != null)
        {
            Player player = GameController.Me.Player;
            MapController mapController = GameController.Me.MapController;

            // 정답이면 방향전환 전까지 자동 진행(최대 100칸)
            if (q.CorrectAnswer == answer)
            {
                MapBlockProperty prop;
                MapBlockProperty propLast;
                

                for (int i = 0; i < 100; ++i)
                {
                    propLast    = mapController.GetMapBlockProperty(player.PlayerPosition);
                    prop = mapController.GetMapBlockProperty(player.PlayerPosition+1);
                    if (propLast == null || prop == null)
                        break;

                    // 한칸 진행
                    player.MoveForwardToValidWay(1);
                    
                    // 이전과 방향이 달라 졌다면 그만한다.
                    if (propLast != null && propLast.Left != prop.Left)
                        break;
                }
                
            }
            // 틀리면 데미지를 준다.
            else
            {
                // 데미지를 주면 한칸 뒤로 밀려나기 때문에 우선 한칸 진행시킨다.
                player.MoveForwardToValidWay(1);
                player.ApplyDamage();
            }
        }
        
        


        SelectQuestion();
        DisplayQuestion();

        
    }

    public void OnAnswer1ButtonClicked()
    {
        SelectAnswer(1);
    }

    public void OnAnswer2ButtonClicked()
    {
        SelectAnswer(2);
    
    }

    public void OnAnswer3ButtonClicked()
    {
        SelectAnswer(3);
    }
}
