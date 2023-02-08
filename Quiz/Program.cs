


using Quiz;
using static Quiz.Direction;

var Quizzes = new List<Direction>();

var userAnswer = new List<int>();
var ansOpt = new List<string>();
var userBase = new UserBase(new List<User>());
int currentUserIndex;
int userInput;
string login, password, nickName, birthDate;
bool menu = true;



while (true)
{


    while (true)
    {
        Console.WriteLine("Введите действие");
        Console.WriteLine("1 - Вход\n2 - Регистрация");
        if (int.TryParse(Console.ReadLine(), out userInput) && userInput >= 1 && userInput <= 2)
            break;
        else
            Console.WriteLine("Некоректно введены данные. Повторите попытку");
    }

    if (userInput == 1)
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

        if (int.TryParse(Console.ReadLine(), out userInput) && userInput >= 0 && userInput <= 4)
            break;
        else
            Console.WriteLine("Некоректно введены данные. Повторите попытку");
    }
    switch ((UserMenu)userInput)
    {
        case UserMenu.exit:
            Console.WriteLine("Вы вышли из программы");
            menu = false;
            break;

        case UserMenu.startQuiz:            
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

            PlayQuiz(Quizzes[userInput], userBase.GetUser(currentUserIndex));

            break;

        case UserMenu.checkUserResults:
            userBase.GetUser(currentUserIndex).PrintArchiveResults();
            break;

        case UserMenu.checkTop20:

            break;

        case UserMenu.changeUserData:

            break;

        default:
            break;
    }
}


static void PlayQuiz(Direction quiz, User user)
{
    var tempQuiz = new Direction(ref quiz);
    int index = 0;
    int userPoints = 0;
    int answer;
    
    for (int i = 0; i < 20; i++)
    {
        index = tempQuiz.GetRandomIndexQuestion();
        tempQuiz.PrintQuestion(index);

        Console.WriteLine("Введите ваш ответ и нажмите Enter. Правильных ответов может быть один или несколько." +
            "\nЕсли по вашему вы ввели все правильные ответы, введите -1(минус один)");

        List<int> answers = new List<int>();

        while (true)
        {
            Console.WriteLine("Введите номер правильного ответа или -1 для перехода к следующему вопросу");



            if (int.TryParse(Console.ReadLine(), out answer) && answer <= 0 && answer >= tempQuiz.GetAnswerCount(index))
                answers.Add(answer);

            else if (answer == -1)
                break;

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