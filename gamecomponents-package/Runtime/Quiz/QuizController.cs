using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SeriousGameComponents.QuizComponent
{
    [SelectionBase]
    public class QuizController : MonoBehaviour
    {
        [Tooltip("String that uniquely identifies this component for the LMS Loader. Leave empty to use the default identifier (WIP)")]
        [HideInInspector]
        public string identifier;

        [Tooltip("Flag to tell if the assets that this component will use are being loaded from the XR4GOOD LMS with the LMSLoader component")]
        [HideInInspector]
        public bool useLmsLoader;

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
        [HideInInspector]
        public int maxAnswerCount;
        [Tooltip("Text object that represents where the Quiz Title will be shown in the scene")]
        [HideInInspector]
        public Text quizTitleObject;
        [Tooltip("Text object that represents where the Quiz Description will be shown in the scene")]
        [HideInInspector]
        public Text quizDescriptionObject;
        [Tooltip("Text object that represents where the Quiz Status Message will be shown in the scene")]
        [HideInInspector]
        public Text quizStatusMessageObject;
        [Tooltip("Text that will be shown in the status message if a correct answer is selected")]
        [TextArea]
        [HideInInspector]
        public string correctAnswerMessage;
        [Tooltip("Text that will be shown in the status message if a wrong answer is selected")]
        [TextArea]
        [HideInInspector]
        public string wrongAnswerMessage;

        [Tooltip("Field that references the object with QuizField component, used to reference where the text for the question will be shown")]
        [HideInInspector]
        public QuizField questionField;

        [Tooltip("List that references objects with QuizField component, used to reference where the texts for the answers will be shown")]
        [HideInInspector]
        public List<QuizField> answerFields;


        [Tooltip("The title of the quiz")]
        public string quizTitle;
        [TextArea]
        [Tooltip("The description of the quiz")]
        public string quizDescription;
        [Tooltip("List that stores the questions that will be asked by the Quiz")]
        public List<QuizQuestion> questions;

        public int currentQuestionIndex { get; private set; } = 0;
        public int selectedAnswer { get; private set; } = -1;
        public bool quizCompleted { get; private set; } = false;

        /// <summary>
        /// Is the last answer submited marked as correct?
        /// </summary>
        public bool isLastAnswerCorrect { get; private set; } = false;

        bool lmsDataLoaded = false;
        /// <summary>
        /// Load the downloaded data from the XR4GOOD LMS registered in the LMS Loader component. If the reload flag is set to false, the new slides will be appended to the slide list
        /// </summary>
        /// <param name="reload"></param>
        public void LoadDownloadedLMSData(bool reload = true)
        {
            if (!useLmsLoader)
            {
                Debug.LogWarning("This object " + gameObject.name + " has not been set to receive data from the XR4GOOD LMS. If you wish to call this method, set the useLmsData attribute value to true");
                return;
            }

            if (reload)
                questions.Clear();

            if (LmsComponent.LmsLoader.quizes == null ||
                LmsComponent.LmsLoader.quizes.Count == 0)
            {
                return;
            }

            string identifier = (this.identifier == "" || this.identifier == null)? LmsComponent.LmsLoader.DEFAULT_IDENTIFIER : this.identifier;

            //Debug.Log("ID: "identifier);
            
            if (!LmsComponent.LmsLoader.quizes.ContainsKey(identifier))
            {
                throw new KeyNotFoundException("Could not load data. \"" + identifier + "\" identifier is not registered in the LMS for this type of component");
            }

            LmsComponent.Json.Quiz q = LmsComponent.LmsLoader.quizes[identifier];
            quizTitle = q.title;
            quizDescription = q.description;

            foreach(LmsComponent.Json.Question qu in q.questions)
            {
                questions.Add(ConvertJsonQuestionToQuizQuestion(qu));
            }

            lmsDataLoaded = true;
        }

        private QuizQuestion ConvertJsonQuestionToQuizQuestion(LmsComponent.Json.Question q)
        {
            QuizQuestion qq = new QuizQuestion(q.description, q.answers.Count);

            List<QuizQuestion.QuestionAnswer> answers = new List<QuizQuestion.QuestionAnswer>();

            foreach (LmsComponent.Json.Answer a in q.answers)
            {
                QuizQuestion.QuestionAnswer qa = new QuizQuestion.QuestionAnswer(a.title, a.is_true == "yes");
                answers.Add(qa);
            }

            qq.answers = answers;

            return qq;
        }

        private void Start()
        {
            if (questions == null) questions = new List<QuizQuestion>();
        }

        bool showedQuestion = false;
        void Update()
        {
            if (useLmsLoader && LmsComponent.LmsLoader.quizFetched && !lmsDataLoaded)
            {
                LoadDownloadedLMSData();
            }

            if (!showedQuestion && questions.Count > 0)
            {
                showedQuestion = true;
                ShowQuestion(currentQuestionIndex);
            }

            if (quizTitleObject != null)
                quizTitleObject.text = quizTitle;

            if (quizDescriptionObject != null)
                quizDescriptionObject.text = quizDescription;
        }

        public void SelectAnswer(int index)
        {
            selectedAnswer = index;
        }

        public void UnselectAnswer()
        {
            selectedAnswer = -1;
        }

        public void AnswerQuestion()
        {
            if(selectedAnswer < 0)
            {
                Debug.LogError("No answer is selected. Select one before calling this function or call it with the \"index\" attribute set");
                return;
            }

            AnswerQuestion(selectedAnswer);
        }

        public void AnswerQuestion(int answerIndex)
        {
            if (answerIndex >= questions[currentQuestionIndex].answers.Count)
                throw new IndexOutOfRangeException("Invalid answer index. Such answer does not exist!");

            if (questions[currentQuestionIndex].answers[answerIndex].isCorrect)
            {
                isLastAnswerCorrect = true;
                ShowCorrectAnswerMessage();
                NextQuestion();
                return;
            }
            else
            {
                isLastAnswerCorrect = false;
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

        public void ClearStatusMessage()
        {
            if (quizStatusMessageObject == null)
                return;

            quizStatusMessageObject.text = "";
        }

        public void NextQuestion()
        {
            if (currentQuestionIndex + 1 < questions.Count)
                currentQuestionIndex++;
            else
                quizCompleted = true;

            ShowQuestion(currentQuestionIndex);
        }

        public void PreviousQuestion()
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                quizCompleted = false;
            }

            ShowQuestion(currentQuestionIndex);
        }

        public void ShowQuestion(int index)
        {
            questionField.SetText(questions[index].question);
            questionField.SetLabelText((index+1).ToString());
            quizStatusMessageObject.text = "";

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

            currentQuestionIndex = index;
        }

        public void ShowQuestionShuffled(int index)
        {
            questions[index].ShuffleAnswers();
            ShowQuestion(index);
        } 
    }
}
