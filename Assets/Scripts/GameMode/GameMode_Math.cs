using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Controller;
using Timers;
using ProgressBar;
using System.Xml;
using System.IO;


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
    public int scorePerQuestionLevel;    // 문제레벨당 점수
    public int countRangeOfPerLevel;  // 문제레벨의 문제개수 범위
    public GameObject mathModeItem;   // math 모드 아이템

    // 문제의 레벨
    int QuestionLevel
    {
        get
        {
            if(GameController.Instance == null)
                return 1;

            if (scorePerQuestionLevel == 0)
                return 1;

            return GameController.Instance.Player.Score / scorePerQuestionLevel + 1;
        }
    }

    int questionIndex = -1;
    List<QuestionSet> questionList;
    float elapsedTimeFromStartingTimer;
    
    void Awake()
    {
        MakeQuestionList();
    }

    // 데미지를 준다.
    void ApplyDamage()
    {
        // 데미지를 주면 한칸 뒤로 밀려나기 때문에 우선 한칸 진행시킨다.
        GameController.Instance.Player.MoveForwardToValidWay(1);
        GameController.Instance.Player.ApplyDamage();
    }

    // 이 함수가 호출된다면 time out이다.
    void OnTimeout()
    {
        if (GameController.Instance.Player.Life <= 0)
            return;

        // 데미지를 준다.
        ApplyDamage();

        // 문제를 다시 낸다
        SelectQuestion();
        DisplayQuestion();
    }

    // 사칙연산을 계산한다.
    // form : 0 - 더하기, 1 - 빼기, 2 - 곱하기, 3 - 나누기
    // 
    float CalcArithmeticOperation(float x, float y, int form)
    {
        if (form == 0)
            return x + y;
        else if (form == 1)
            return x - y;
        else if (form == 2)
            return x * y;

        return x / y;
    }

    // 문제 리스트를 만든다.
    void MakeQuestionList()
    {
        if (questionList == null)
            questionList = new List<QuestionSet>();
        else
            questionList.Clear();

        // 하위 레벨 문제는 프로그램이 직접 만든다.
        {
            string question, answer1, answer2, answer3;
            int correctanswer;
            float x, y;
            float result;

            // 사칙연산
            // + - * /
            // 나누기는 제외한다. 소수점이 나올 수 있다.
            for (int form = 0; form < 4; ++form)
            {
                // 1자리 ~ 2자리덧셈
                int gap = 1;
                for (int ix = 1; ix < 100; ix += gap)
                {
                    x = ix;
                    for (int jx = 1; jx < 100; jx += gap)
                    {
                        y = jx;

                        // x가 10보다 작을때는 y도 10보다 작은 수로 제한한다.(유아용)
                        if (x < 10 && y > 10)
                            break;

                        // x와 y가 10보다 커지면 gap을 3으로 올린다.
                        if (x > 10 && y > 10)
                            gap = 3;

                        // 뺄샘은 더 큰값을 y로 한다.
                        if(form == 1)
                        {
                            if(y > x)
                            {
                                x = jx;
                                y = ix;
                            }
                        }

                        // 정답을 랜덤하게 정한다.
                        correctanswer = Random.Range(1, 4); // max 는 exclusive

                        // 정답 계산
                        result = CalcArithmeticOperation(x, y, form);

                        // 보기를 정한다.
                        if (correctanswer == 1)
                            answer1 = result.ToString();
                        else
                            answer1 = (result - Random.Range(1, 10)).ToString();

                        if (correctanswer == 2)
                            answer2 = (result).ToString();
                        else
                            answer2 = (result + Random.Range(1, 10)).ToString();

                        if (correctanswer == 3)
                            answer3 = (result).ToString();
                        else
                            answer3 = (result + Random.Range(1, 10) * 2).ToString();

                        // 문제를 만든다.
                        question = x.ToString() + "+" + y.ToString();

                        questionList.Add(new QuestionSet(question, answer1, answer2, answer3, correctanswer));
                    }
                }
            }
        }


        // 그다음 xml에서 불러온다.
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(Resources.Load("MathQuestions").ToString());
            if (xmlDocument == null)
            {
                System.Console.WriteLine("Couldnt Load Xml");
                return;
            }

            string question, answer1, answer2, answer3, correctAnswer;

            XmlNode xNode = xmlDocument.ChildNodes.Item(1);
            foreach (XmlNode node in xNode.ChildNodes)
            {
                if (node.LocalName == "Question")
                {
                    question = node.Attributes.GetNamedItem("question").Value;
                    answer1 = node.Attributes.GetNamedItem("answer1").Value;
                    answer2 = node.Attributes.GetNamedItem("answer2").Value;
                    answer3 = node.Attributes.GetNamedItem("answer3").Value;
                    correctAnswer = node.Attributes.GetNamedItem("correctanswer").Value;

                    questionList.Add(new QuestionSet(question, answer1, answer2, answer3, System.Convert.ToInt32(correctAnswer)));
                }
            }
        }
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
        // 문제레벨
        int minIndex = QuestionLevel * countRangeOfPerLevel;
        int maxIndex = minIndex + countRangeOfPerLevel;

        // 문제 수보다 많아지면 전체 랜덤으로 제출한다.
        if(minIndex >= questionList.Count || maxIndex >= questionList.Count)
        {
            minIndex = 0;
            maxIndex = questionList.Count-1;
        }

        questionIndex = Random.Range(minIndex, maxIndex);
    }

    void FixedUpdate()
    {
        if (GameController.Instance.IsWaitingToRevive())
            return;

        // life가 없다면 업데이트 하지 않는다.
        if (GameController.Instance.Player.Life == 0)
            return;

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
        TimersManager.ClearTimer(OnTimeout);
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
        if(GameController.Instance)
        {
            GameController.Instance.Player.EnableUserInput = false;
        }

        // 여기서 보여야 하는 item을 showItemByGameMode로 이동한다
        if (mathModeItem && GameController.Instance && GameController.Instance.gameModeController && GameController.Instance.gameModeController.showItemByGameMode)
            mathModeItem.transform.SetParent(GameController.Instance.gameModeController.showItemByGameMode.transform);

        // 맵을 구성한다.
        if (GameController.Instance)
        {
            GameController.Instance.mapController.MakeMap();
        }

        timeOutProgressbar.SetFillerSize(100);
    }

    //  답을 선택한다.
    void SelectAnswer(int answer)
    {
        Player player = GameController.Instance.Player;
        MapController mapController = GameController.Instance.MapController;

        // 그로기 상태에서는 답선택이 불가하다
        if (player.IsGroggy)
            return;
        
        QuestionSet q = CurrentQuestion;
        if(q != null)
        {
            
            

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
                ApplyDamage();
                
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
