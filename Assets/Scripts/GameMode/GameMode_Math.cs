using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Button leftButton;
    public Button jumpButton;
    public Button rightButton;
    public Text questionText;
    public Text answer1Text;
    public Text answer2Text;
    public Text answer3Text;

    int questionIndex = -1;
    List<QuestionSet> questionList = new List<QuestionSet>();
    
    void Awake()
    {
        MakeQuestionList();
    }

    // 문제 리스트를 만든다.
    void MakeQuestionList()
    {
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
    }

    // 시작할때 버튼들을 숨긴다.
    void OnEnable()
    {
        if (leftButton)
            leftButton.enabled = false;
        if (jumpButton)
            jumpButton.enabled = false;
        if (rightButton)
            rightButton.enabled = false;
    }

    //  답을 선택한다.
    void SelectAnswer(int answer)
    {
        QuestionSet q = CurrentQuestion;
        if(q.CorrectAnswer == answer)
        { 
        }
        else
        {

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
