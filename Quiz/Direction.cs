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
        List<UserTop20> top20 = new List<UserTop20>();
        
        Random random = new Random();
        public Direction(string name)
        {
            Name = name;
            questions = new List<QuestionBlock>();
        }
        public Direction(ref Direction other)
        {
            this.Name = other.Name;
            this.questions = new List<QuestionBlock>();
            for (int i = 0; i < other.GetQuestionsCount(); i++)
            {
                this.questions.Add(other.questions[i]);
            }
        }
        public bool AddTop20(string name, int value)
        {
            if (name == null || value < 0)
                return false;
            
            if(top20.Count < 20)
            {
                top20.Add(new UserTop20(name, value));
                SortTop20();
                return true;
            }
            else
            {
                if (value > top20[19].value)
                {
                    top20.RemoveAt(19);
                    top20.Add(new UserTop20(name, value));
                    SortTop20();
                    return true;
                }
                else
                    return false;
            }
        }
        public void SortTop20()
        {
            UserTop20 temp;
            for (int i = 0; i < top20.Count; i++)
            {
                for (int j = 0; j < top20.Count; j++)
                {
                    if (top20[i].value < top20[j].value)
                    {
                        temp = top20[i];
                        top20[i] = top20[j];
                        top20[j] = temp;
                    }
                }
            }
        }
        public void PrintTop20()
        {
            for (int i = 0; i < top20.Count; i++)
            {
                Console.WriteLine($"{i + 1} {top20[i].name} {top20[i].value} правильных ответов");
            }
        }
        public int GetAnswerCount(int questionIndex)
        {
            return questions[questionIndex].answerOptions.Count;
        }
        public int GetQuestionsCount()
        {
            return questions.Count;
        }
        public void RemoveQuestion(int index)
        {
            questions.Remove(questions[index]);
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

        public int GetRandomIndexQuestion()
        {
            return random.Next(0, questions.Count - 1);
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
