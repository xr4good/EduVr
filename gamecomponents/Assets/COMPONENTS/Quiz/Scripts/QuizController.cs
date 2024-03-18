using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace QuizComponent
{
    [SelectionBase]
    public class QuizController : MonoBehaviour
    {
        [Serializable]
        public class QuizQuestion
        {
            [Serializable]
            public struct QuestionAnswer
            {
                [TextArea]
                public string answer;
                public bool isCorrect;

                public QuestionAnswer(string answer, bool isCorrect)
                {
                    this.answer = answer;
                    this.isCorrect = isCorrect;
                }
            }

            [TextArea]
            public string question;
            public List<QuestionAnswer> answers;
            public bool listAnswersWithLetters;

            public int maxAnswerCount { get; private set; }

            public QuizQuestion()
            {
                question = "sample question";
                answers = new List<QuestionAnswer>(4);
                this.maxAnswerCount = 4;
            }

            public QuizQuestion(string title)
            {
                question = title;
                answers = new List<QuestionAnswer>(4);
                this.maxAnswerCount = 4;
            }

            public QuizQuestion(int maxAnswerCount)
            {
                question = "sample question";
                answers = new List<QuestionAnswer>(maxAnswerCount);
                this.maxAnswerCount = maxAnswerCount;
            }

            public QuizQuestion(string title, int maxAnswerCount)
            {
                question = title;
                answers = new List<QuestionAnswer>(maxAnswerCount);
                this.maxAnswerCount = maxAnswerCount;
            }

            public void AddAnswer(string answer, bool isCorrect, bool force = false)
            {
                if (answers.Count >= maxAnswerCount && !force)
                {
                    Debug.LogWarning("It was not possible to add the new answer. The question already have the maximum number of answers permited");
                    return;
                }

                answers.Add(new QuestionAnswer(answer, isCorrect));
            }

            public void ChangeAnswer(int index, string newAnswer, bool isCorrect)
            {
                if (answers.Count - 1 < index)
                {
                    Debug.LogWarning("Referenced answer index does not exist (" + index + ")");
                    return;
                }

                answers[index] = new QuestionAnswer(newAnswer, isCorrect);
            }

            public void ChangeOrAddAnswer(int index, string newAnswer, bool isCorrect)
            {
                if (answers.Count - 1 < index)
                {
                    AddAnswer(newAnswer, isCorrect);
                    return;
                }

                answers[index] = new QuestionAnswer(newAnswer, isCorrect);
            }

            public void ShuffleAnswers()
            {
                System.Random rng = new System.Random();
                answers = answers.OrderBy(a => rng.Next()).ToList();
            }
        }


        [Tooltip("Max number of answers that a question can have in this specific Quiz")]
        public int maxAnswerCount;
        [Space] [Space]

        

        public Text quizTitleObject;
        public string quizTitle;
        [Space]
        [Space]
        public Text quizDescriptionObject;
        public string quizDescription;
        [Space]
        [Space]
        public Text quizStatusMessageObject;
        [TextArea]
        public string correctAnswerMessage;
        [TextArea]
        public string wrongAnswerMessage;

        [Space]
        [Space]
        [Tooltip("Field that references the object with QuizField component, used to reference where the text for the question will be shown")]
        public QuizField questionField;

        [Tooltip("List that references objects with QuizField component, used to reference where the texts for the answers will be shown")]
        public List<QuizField> answerFields;

        [Tooltip("List that stores the questions that will be asked by the Quiz")]
        public List<QuizQuestion> questions;

        public int currentQuestionIndex { get; private set; } = 0;

        void Start()
        {
            ShowQuestion(currentQuestionIndex);
        }

        // Update is called once per frame
        void Update()
        {
            if(quizTitleObject != null)
                quizTitleObject.text = quizTitle;

            if (quizDescriptionObject != null)
                quizDescriptionObject.text = quizDescription;
        }

        public void AnswerQuestion(int answerIndex)
        {
            if (answerIndex >= questions[currentQuestionIndex].answers.Count)
                throw new IndexOutOfRangeException("Invalid answer index. Such answer does not exist!");

            if (questions[currentQuestionIndex].answers[answerIndex].isCorrect)
            {
                ShowCorrectAnswerMessage();
                NextQuestion();
                return;
            }
            else
            {
                ShowWrongAnswerMessage();
                return;
            }
        }

        public void ShowWrongAnswerMessage()
        {
            if (quizStatusMessageObject == null)
                return;

            quizStatusMessageObject.text = wrongAnswerMessage;
        }

        public void ShowCorrectAnswerMessage()
        {
            if (quizStatusMessageObject == null)
                return;

            quizStatusMessageObject.text = correctAnswerMessage;
        }

        public void NextQuestion()
        {
            if (currentQuestionIndex + 1 < questions.Count)
                currentQuestionIndex++;

            ShowQuestion(currentQuestionIndex);
        }

        public void PreviousQuestion()
        {
            if (currentQuestionIndex > 0)
                currentQuestionIndex--;

            ShowQuestion(currentQuestionIndex);
        }

        public void ShowQuestion(int index)
        {
            questionField.SetText(questions[index].question);
            questionField.SetLabelText((index+1).ToString());

            int i = 0;
            foreach(QuizField qf in answerFields)
            {
                if (i < questions[index].answers.Count)
                {
                    qf.SetText(questions[index].answers[i].answer);
                    if (questions[index].listAnswersWithLetters)
                    {
                        int ascii_A = 65;
                        int ascii_Z = 65 + 25;
                        qf.SetLabelText(ascii_A + i > ascii_Z ? (i - 25).ToString() : ((char)(ascii_A+i)).ToString()); //Turn number 1 into A, 2 into B, ..., 25 into Z, 26 into 1, 27 into 2, etc
                    }
                    else
                        qf.SetLabelText((i + 1).ToString());
                }
                else break;

                i++;
            }
        }

        public void ShowQuestionShuffled(int index)
        {
            questions[index].ShuffleAnswers();
            ShowQuestion(index);
        } 
    }
}
