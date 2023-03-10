using Newtonsoft.Json;
using System.IO;


using Quiz;
using static Quiz.Direction;
using System.Collections.Generic;
using System;



//Чтение списка викторин из файла
var Quizzes = new List<Direction>();
ReadQuizzesList(ref Quizzes);

//Чтение списка зарегестрированных пользователей из файла
var userBase = new UserBase(new List<User>());
ReadUsersList(ref userBase);

var userAnswer = new List<int>();
var ansOpt = new List<string>();




int currentUserIndex;
int userInput;
string login, password, nickName, birthDate;
bool menu = true;



while (true)
{
    while (true)
    {
        Console.WriteLine("Введите действие");
        Console.WriteLine("0 - Редактор Викторины\n1 - Вход\n2 - Регистрация");
        if (int.TryParse(Console.ReadLine(), out userInput) && userInput >= 0 && userInput <= 2)
            break;
        else
            Console.WriteLine("Некоректно введены данные. Повторите попытку");
    }
    //0 - Редактор викторин
    if (userInput == 0)
    {
        while(true)
        {
            Console.WriteLine("Введите выбор");
            Console.WriteLine("1 - Добавить новую викторину");
            Console.WriteLine("2 - Редактор викторин");

            if (int.TryParse(Console.ReadLine(), out userInput) && userInput >= 1 && userInput < 2)
                break;
            else
                Console.WriteLine("Некоректно введены данные. Повторите попытку");
        }
        //Добавить викторину
        if (userInput == 1)
        {
            var questions = new List<QuestionBlock>();
            var top20 = new List<UserTop20>();
            var answerOptions = new List<string>();
            var rightAnswer = new List<int>();

            Console.WriteLine("Введите название викторины");
            string nameQuiz = Console.ReadLine() ?? "no_name";
            string question;
            string answer;

            for(int i = 0; i < 3; i++)
            {
                Console.Clear();
                Console.WriteLine($"Введите вопрос {i+1}");
                question = Console.ReadLine() ?? "noquest";

                answerOptions.Clear();

                while (true)
                {
                    Console.WriteLine("Введите вариант ответа и Enter. Введите 0, если больше нет вариантов ответа");
                    answer = Console.ReadLine() ?? "0";
                    if (int.TryParse(answer, out userInput) && userInput == 0)
                        break;
                    else
                        answerOptions.Add(answer);
                }

                rightAnswer.Clear();

                Console.WriteLine("\tВарианты ответа добавлены.\n\nВведите номер правильного ответа начиная с 0, если их несколько вводите по одному.");
                while (true)
                {
                    Console.WriteLine("Введите номер правильного ответа или -1, чтобы перейти к следующему вопросу");
                    answer = Console.ReadLine() ?? "-1";
                    if (int.TryParse(answer, out userInput))
                    {
                        if (userInput == -1)
                            break;
                        else
                            rightAnswer.Add(userInput);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода. Повторите попытку");
                        Console.ReadKey();
                    }                        

                }

                questions.Add(new QuestionBlock(question, answerOptions, rightAnswer));                
            }

            Quizzes.Add(new Direction(nameQuiz, questions, top20));
        }
        //Редактор конкретной викторины
        if (userInput == 2)
        {
            Console.WriteLine("Функционал будет добавлен в следующих версиях программы");
        }
    }
    // 1 - Вход (Авторизация)
    else if (userInput == 1)
    {
        while (true)
        {
            Console.WriteLine("Введите логин");
            login = Console.ReadLine() ?? "";
            Console.WriteLine("Введите пароль");
            password = Console.ReadLine() ?? "";
            Console.WriteLine("Введите ваше имя");

            if (userBase.Authorization(login, password))
            {
                Console.WriteLine("Авторизация прошла успешно");
                currentUserIndex = userBase.GetUserIndex(login);
                break;
            }
            else
                Console.WriteLine("Ошибка авторизации");
        }
        break;
    }
    //Регистрация
    else if (userInput == 2)
    {
        while (true)
        {
            Console.WriteLine("Введите логин длинной не менее 3 символов");
            login = Console.ReadLine() ?? "";
            Console.WriteLine("Введите пароль длинной не менее 3 символов");
            password = Console.ReadLine() ?? "";
            Console.WriteLine("Введите ваше имя длинной не менее 3 символов");
            nickName = Console.ReadLine() ?? "";
            Console.WriteLine("Введите дату вашего рождения");
            birthDate = Console.ReadLine() ?? "";

            if (userBase.Registration(login, password, nickName, birthDate))
            {
                Console.WriteLine("Регистрация прошла успешно");
                break;
            }
            else
                Console.WriteLine("Ошибка регистрации");
        }
    }
}

Console.Clear();

while (menu)
{
    while (true)
    {
        Console.WriteLine("Введите выбор");
        Console.WriteLine("1 - Начать новую викторину");
        Console.WriteLine("2 - Просмотр своих результатов");
        Console.WriteLine("3 - Просмотр 20 лучших результатов викторины");
        Console.WriteLine("4 - Изменить свои данные");
        Console.WriteLine("0 - Выход");

        if (int.TryParse(Console.ReadLine(), out userInput) && userInput >= 0 && userInput <= 4)
            break;
        else
            Console.WriteLine("Некоректно введены данные. Повторите попытку");
    }
    switch ((UserMenu)userInput)
    {
        case UserMenu.exit:
            foreach (var item in Quizzes)
            {
                item.Serealize();
            }

            userBase.Serealize();

            Console.WriteLine("Вы вышли из программы");
            menu = false;
            break;

        case UserMenu.startQuiz:
            Console.Clear();
            if(Quizzes == null || Quizzes.Count == 0)
            {
                Console.WriteLine("В приложении нет добавленных викторин");
                break;
            }    
            while (true)
            {
                Console.WriteLine("Выберите викторину");

                for (int i = 0; i < Quizzes.Count; i++)
                    Console.WriteLine($"{i} - {Quizzes[i].Name}");

                if (int.TryParse(Console.ReadLine(), out userInput) && userInput >= 0 && userInput < Quizzes.Count)
                    break;
                else
                    Console.WriteLine("Некоректно введены данные. Повторите попытку");
            }

            Console.Clear();

            PlayQuiz(Quizzes[userInput], userBase.GetUser(currentUserIndex));

            break;

        case UserMenu.checkUserResults:
            userBase.GetUser(currentUserIndex).PrintArchiveResults();
            break;

        case UserMenu.checkTop20:
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Выберите викторину");

                for (int i = 0; i < Quizzes.Count; i++)
                    Console.WriteLine($"{i} - {Quizzes[i].Name}");

                if (int.TryParse(Console.ReadLine(), out userInput) && userInput >= 0 && userInput < Quizzes.Count)
                    break;
                else
                    Console.WriteLine("Некоректно введены данные. Повторите попытку");
            }

            Quizzes[userInput].PrintTop20();
            break;

        case UserMenu.changeUserData:
            Console.Clear();
            userBase.ChangePassword(currentUserIndex);

            break;

        default:
            break;
    }
}


static void ReadQuizzesList(ref List<Direction> list)
{
    try
    {
        string[] quizzesPathArray = Directory.GetFiles(Directory.GetCurrentDirectory(), "*quiz.json");
        string[] top20PathArray = Directory.GetFiles(Directory.GetCurrentDirectory(), "*top20.json");


        for (int i = 0; i < quizzesPathArray.Length; i++)
        {
            for (int j = quizzesPathArray[i].Length - 9; j > 0; j--)
            {
                if (quizzesPathArray[i][j] == '\\')
                {
                    string quizName = quizzesPathArray[i].Remove(0, j + 1);
                    quizName = quizName.Substring(0, quizName.Length - 9);


                    list.Add(new Direction(quizName,
                        JsonConvert.DeserializeObject<List<QuestionBlock>>(File.ReadAllText(quizzesPathArray[i])),
                        JsonConvert.DeserializeObject<List<UserTop20>>(File.ReadAllText(top20PathArray[i]))));
                    break;
                }
            }
        }
    }
    catch (Exception)
    {
        Console.WriteLine("Ошибка при чтении списка викторин");
    }
}

static void ReadUsersList(ref UserBase userBase)
{
    try
    {
        userBase = new UserBase(JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("userBase.json")));
    }
    catch (Exception)
    {
        Console.WriteLine("Ошибка чтения данных пользователей");
    }
}

static void PlayQuiz(Direction quiz, User user)
{
    var tempQuiz = new Direction(ref quiz);
    int index = 0;
    int userPoints = 0;
    int answer;
    List<int> answers = new List<int>();

    for (int i = 0; i < 20; i++)
    {
        index = tempQuiz.GetRandomIndexQuestion();
        if (index == -1)
            break;
        tempQuiz.PrintQuestion(i);

        Console.WriteLine("Введите ваш ответ и нажмите Enter. Правильных ответов может быть один или несколько." +
            "\nЕсли по вашему вы ввели все правильные ответы, введите -1(минус один)");
              
        while (true)
        {
            Console.WriteLine("Введите номер правильного ответа или -1 для перехода к следующему вопросу");

            answers.Clear();

            if (int.TryParse(Console.ReadLine(), out answer) && answer >= -1 && answer <= quiz.GetAnswerCount(index))
            {
                if (answer == -1)
                    break;
                else
                    answers.Add(answer);
            }
            else
                Console.WriteLine("Ошибка ввода. Повторите попытку");
        }

        if (tempQuiz.CheckAnswer(index, answers))
            userPoints++;

        tempQuiz.RemoveQuestion(index);
    }

    if(quiz.AddTop20(user.Name, userPoints))
        Console.WriteLine("Ваш результат добавлен в топ 20 лучших результатов викторины");
    else
        Console.WriteLine("Вашего результата недостаточно для добавления в топ 20 лучших результатов");

    user.SaveResults(quiz.Name, userPoints);
}

public enum UserMenu
{
    exit, startQuiz, checkUserResults, checkTop20, changeUserData
}

/*
Задание 2
Создать приложение «Викторина».
Основная задача проекта: предоставить пользователю возможность проверить свои
знания в разных областях.
Интерфейс приложения должен предоставлять такие возможности:
■ При старте приложения пользователь вводит логин и пароль для входа. Если пользователь не зарегистрирован, он должен пройти процесс регистрации.
■ При регистрации нужно указать:
• логин(нельзя зарегистрировать уже существующий логин);
• пароль;
• дату рождения.
■ После входа в систему пользователь может:
• стартовать новую викторину;
• посмотреть результаты своих прошлых викторин;
• посмотреть Топ-20 по конкретной викторине;
• изменить настройки: можно менять пароль и дату рождения;
• выход.
■ Для старта новой викторины пользователь должен выбрать раздел знаний викторины. Например: «История», «География», «Биология» и т.д.Также нужно
предусмотреть смешанную викторину, когда вопросы будут выбираться из разных
викторин по случайному принципу.
■ Конкретная викторина состоит из двадцати вопросов. У каждого вопроса может
быть один или несколько правильных вариантов ответа.Если вопрос предполагает
несколько правильных ответов, а пользователь указал не все, вопрос не засчитывается.
■ По завершении викторины пользователь получает количество правильно отвеченных вопросов, а также свое место в таблице результатов игроков викторины.
Необходимо также разработать утилиту для создания и редактирования викторин
и их вопросов. Это приложение должно предусматривать вход по логину и паролю. 
*/
