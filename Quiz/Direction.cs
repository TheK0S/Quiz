using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class Direction
    {
        public string? Name { get; set; }
        List<QuestionBlock> questions;

        Random random = new Random();
        public Direction(string name)
        {
            Name = name;
            questions = new List<QuestionBlock>();
        }
        public QuestionBlock GetRandomQuestion()
        {
            if (questions == null || questions.Count == 0)
                return new QuestionBlock("no question", new List<string>(), new List<int>());

            else if(questions.Count == 1)
                return questions[0];

            else
                return questions[random.Next(0, questions.Count - 1)];
        }

        public void PrintQuestion(int index)
        {
            if(questions != null && index >= 0 && index < questions?.Count)
            {
                Console.WriteLine(questions[index].Question);

                for (int i = 0;i < questions[index].answerOptions.Count; i++)
                {
                    Console.WriteLine($"{i} - {questions[index].answerOptions[i]}");
                }
            }
            else
                Console.WriteLine("Ошибка! Индекс вне предела списка");
        }

        public bool CheckAnswer(int index, List<int> answers)
        {
            if (questions != null && index >= 0 && index < questions.Count && questions[index].rightAnsswers.Count == answers.Count)
            {
                answers.Sort();
                questions[index].rightAnsswers.Sort();

                for (int i = 0; i < answers.Count; i++)
                {
                    if (questions[index].rightAnsswers[i] != answers[i])
                        return false;
                }
            }
            else
                return false;

            return true;
        }

        public void Add(string question, List<string> answerOptionss, List<int> rightAnswers)
        {
            questions.Add(new QuestionBlock(question, answerOptionss, rightAnswers));
        }
        public class QuestionBlock
        {
            public string Question;
            public List<string> answerOptions;
            public List<int> rightAnsswers;
            
            public QuestionBlock(string _question, List<string> _answerOptionss, List<int> _rightAnswers)
            {
                Question = _question;
                answerOptions = _answerOptionss;
                rightAnsswers = _rightAnswers;
            }
        }
    }
}
