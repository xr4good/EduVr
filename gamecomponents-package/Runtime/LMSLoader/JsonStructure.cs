using System.Collections.Generic;
using System;

namespace SeriousGameComponents.LmsComponent
{
    /// <summary>
    /// This is the structure that the json returned by the LMS has. If any changes are made to the json, this file must be edited to acomodate the new json format
    /// </summary>
    namespace Json
    {
        [Serializable]
        public class Lessons
        {
            public List<Lesson> lessons;
        }

        [Serializable]
        public class Metadata
        {
            public string aoc_image_image;
            public string aoc_vid_video;
            public string _lp_associated_quiz;
            public string _lp_custom_lesson_is_for_game;
        }

        [Serializable]
        public class Post
        {
            public string id;
            public string post_title;
            public string post_content;
            public Metadata metadata;
        }

        [Serializable]
        public class Answer
        {
            public string question_id;
            public string question_answer_id;
            public string title;
            public string is_true;
        }

        [Serializable]
        public class Question
        {
            public string id;
            public string title;
            public string description;
            public List<Answer> answers;
        }

        [Serializable]
        public class Quiz
        {
            public string id;
            public string title;
            public string description;
            public List<Question> questions;
        }

        [Serializable]
        public class Lesson
        {
            public Post post;
            public Quiz quiz;
        }
    }
}


