using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizControl : MonoBehaviour
{
public GameObject testButton;
    public GameObject screen;
    public GameObject title;
    public GameObject subtitle;
    public GameObject error;
    public GameObject questionNumber;
    public GameObject question;
    public GameObject optionText1;
    public GameObject optionText2;
    public GameObject optionText3;
    public GameObject optionText4;
    public GameObject optionButton1;
    public GameObject optionButton2;
    public GameObject optionButton3;
    public GameObject optionButton4;
    public GameObject endTest;
    public StatementSender statementSender;
        private string titleQuiz;
        private string content;
         private List<Question> questions;
    private int actualQuestion = 0;
    public static bool quizCompleted;
    public GameObject microscopeText;
    public void Start(){
        titleQuiz = GetDataAPI.selectedLesson.quiz.title;
        questions =  GetDataAPI.selectedLesson.quiz.questions;
        content = GetDataAPI.selectedLesson.quiz.description;
        quizCompleted = false;
    }
    
    // Start is called before the first frame update
    public void startTest(){
        testButton.SetActive(false);
        screen.SetActive(true);
        optionButton1.SetActive(true);
        optionButton2.SetActive(true);
        optionButton3.SetActive(true);
        optionButton4.SetActive(true);
        title.GetComponent<TextMeshPro>().text = titleQuiz;
        subtitle.GetComponent<TextMeshPro>().text = "";
        setupQuestion();
    }
    private string getAnswerText(string answer){
        if(answer.Equals("A")){
            return questions[actualQuestion].answers[0].title;
        }
        if(answer.Equals("B")){
            return questions[actualQuestion].answers[0].title;
        }
         if(answer.Equals("C")){
            return questions[actualQuestion].answers[0].title;
        }
         if(answer.Equals("D")){
            return questions[actualQuestion].answers[0].title;
        }

        return "";
    }

    public string correctAnswer(){
        int count = 0;
        foreach(Answer i in questions[actualQuestion].answers){
            if(i.is_true.Equals("yes")){
                switch (count)
                {
                    case 0: 
                        return "A";
                    case 1: 
                        return "B";
                    case 2: 
                        return "C";
                    case 3: 
                        return "D";
                    default:
                        break;                        
                }
            }
            count++;
        }
        return null;
    }


    public void setupQuestion() {
        error.GetComponent<TextMeshPro>().text = "";
        questionNumber.GetComponent<TextMeshPro>().text ="" + actualQuestion+1;
        question.GetComponent<TextMeshPro>().text = questions[actualQuestion].description;
        optionText1.GetComponent<TextMeshPro>().text = questions[actualQuestion].answers[0].title;
        optionText2.GetComponent<TextMeshPro>().text = questions[actualQuestion].answers[1].title;
        optionText3.GetComponent<TextMeshPro>().text = questions[actualQuestion].answers[2].title;
        optionText4.GetComponent<TextMeshPro>().text = questions[actualQuestion].answers[3].title;
    }
    public void checkAnswer(string answer){
        if(answer.Equals(correctAnswer())){
            error.GetComponent<TextMeshPro>().text = ""; 
           statementSender.logQuestionAnswers(questions[actualQuestion].title, getAnswerText(answer), true); // acerto
            if(actualQuestion==questions.Count-1){
                optionButton1.SetActive(false);
                optionButton2.SetActive(false);
                optionButton3.SetActive(false);
                optionButton4.SetActive(false);
                endTest.SetActive(true);
                error.GetComponent<TextMeshPro>().text = "Teste concluido. Clique no botão para ver sua amostra e encerrar o jogo."; 
                statementSender.logQuizFinished(); // concluido 
                microscopeText.SetActive(true);
                microscopeText.GetComponent<TextMeshPro>().text = "Olhe no microscópio para ver a amostra";
                microscopeText.GetComponent<TextMeshPro>().color =  new Color32(17, 116, 0, 255);
                quizCompleted=true;
            }else{
                actualQuestion++;
                setupQuestion();
            }
        }else{
            error.GetComponent<TextMeshPro>().text = "Resposta incorreta, tente novamente. Volte a ler o material caso tenha dificuldade.";
            statementSender.logQuestionAnswers(questions[actualQuestion].title, getAnswerText(answer), false); // falha

        }
    }

}
