using Exam_Task_2;

QuizManager quizManager = new QuizManager();
AccountManager accountManager = new AccountManager();
InputMethods inputMethods = new InputMethods();

while (true)
{
    string? user = inputMethods.InputCycle("1. Войти в аккаунт\n2. Зарегистрироваться\n3. Создать викторину\n-> ");

    if (user == "1")
    {
        int indexAccount = accountManager.LoginAccount();

        if (indexAccount != -1)
        {
            while(true)
            {
                int iter = 1;

                if(quizManager.quizzes.Count > 0) foreach (var quiz in quizManager.quizzes) { Console.WriteLine($"{iter++}. {quiz.Name}"); }
                Console.WriteLine($"{iter}. Вернуться назад");

                string? index = inputMethods.Input("Выберите викторину -> ");

                if (index == $"{iter}") break;
                else if(quizManager.quizzes.Count > 0 && int.TryParse(index, out iter) && iter-1 >= 0 && iter-1 <= quizManager.quizzes.Count-1)
                {
                    iter--;                    

                    while(true)
                    {
                        string? user_ = inputMethods.InputCycle($"1. Запустить викторину {quizManager.quizzes[iter].Name}\n2. Посмотреть топ-20 викторины\n3. Вернуться\n-> ");

                        if (user_ == "1")
                        {
                            int ball = 0;
                            for (int i = 0; i < quizManager.quizzes[iter].Questions.Count; i++)
                            {
                                while(true)
                                {
                                    int iterAnswer = 1;
                                    Console.WriteLine($"Вопрос {i+1}. {quizManager.quizzes[iter].Questions[i].question}");
                                    foreach(var answer in quizManager.quizzes[iter].Questions[i].Answer) Console.WriteLine($"{iterAnswer++}. {answer}");

                                    string? userAnswer = inputMethods.Input("Выберите правильный ответ -> ");

                                    if (int.TryParse(userAnswer, out iterAnswer) && iterAnswer - 1 >= 0 && iterAnswer - 1 <= quizManager.quizzes[iter].Questions[i].Answer.Count - 1)
                                        if (iterAnswer - 1 == quizManager.quizzes[iter].Questions[i].RightAnswer) { ball++; break; }
                                        else break;
                                }
                            }

                            int ii = -1;
                            for (int i = 0; i < quizManager.quizzes[iter].Standings.Count; i++)
                                if (accountManager.accounts[indexAccount].Login == quizManager.quizzes[iter].Standings[i].Login) { ii = i; break; }


                            if (ii == -1) quizManager.quizzes[iter].Standings.Add(new Standing(accountManager.accounts[indexAccount].Login, ball));
                            else
                                if (ball > quizManager.quizzes[iter].Standings[ii].Ball) quizManager.quizzes[iter].Standings[ii] = new Standing(accountManager.accounts[indexAccount].Login, ball); 
                                else quizManager.quizzes[iter].Standings.Add(new Standing(accountManager.accounts[indexAccount].Login, ball)); 

                            quizManager.SaveJSON(QuizManager.defaultPATH);
                            Console.WriteLine($"Вы прошли викторину!\nКоличество баллов: {ball}/{quizManager.quizzes[iter].Questions.Count}");
                        }
                        else if (user_ == "2")
                        {
                            if (quizManager.quizzes[iter].Standings.Count > 0)
                            {
                                var topTwenty = from standing in quizManager.quizzes[iter].Standings
                                                orderby standing.Ball descending
                                                select standing;

                                int iter_ = 1;
                                foreach (var standing in topTwenty) if(iter < 21) Console.WriteLine($"{iter_++}. {standing.Login} - {standing.Ball}");
                                Console.WriteLine("--------------------");
                            }
                            else Console.WriteLine("Турнирная таблица пустая!");
                        }
                        else if (user_ == "3") break;
                    }
                }
            }
        }
    }
    else if (user == "2") accountManager.RegisterAccount();
    else if (user == "3") quizManager.AddQuiz();
}

class InputMethods
{
    public string? InputCycle(string msg)
    {
        string? text;
        while (true)
        {
            text = Input(msg);
            if (!string.IsNullOrEmpty(text)) { return text; }
        }
    }

    public string? Input(string msg)
    {
        Console.Write(msg); string? text = Console.ReadLine(); Console.Clear();
        return text;
    }
}