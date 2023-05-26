using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Exam_Task_2
{
    internal struct Quiz
    {
        public string Name { get; set; } = null;
        public List<Question> Questions { get;set; } = new List<Question>();

        public Quiz(string name) { Name = name; }            
    }

    record class Question(string question, List<string> Answer, int RightAnswer);

    class QuizManager : InputMethods
    {
        List<Quiz> quizzes = LoadJSON(defaultPATH);
        private static string defaultPATH = "quiz.json";

        public QuizManager() { }

        public void AddQuiz()
        {
            string? name = InputCycle("Введите название викторины -> ");
            quizzes.Add(new Quiz(name));
            int iter = 1; int countQuestions;

            while(true)
            {
                string? count = Input("Введите количество вопросов (от 2 до 20) -> ");
                if (int.TryParse(count, out countQuestions) && countQuestions >= 2 && countQuestions <= 20) break;
            }

            while(quizzes.Last().Questions.Count != countQuestions)
            {
                string? question = InputCycle($"Введите {iter} вопрос -> ");
                List<string> answers = new List<string>();
                int index;

                while (true)
                {
                    string? answer = InputCycle($"Вводите вариант ответа на {iter} вопрос (Вариантов ответа: {answers.Count})\nЧтобы завершить, введите 0 -> ");
                    if(answer == "0")
                    {
                        if (answers.Count < 2) Console.WriteLine("Недостаточно ответов! Нужно как минимум 2.");
                        else break;
                    }
                    else answers.Add(answer);
                }                

                while (true)
                {
                    int answersIter = 1;
                    for (int i = 0; i < answers.Count; i++)
                        Console.WriteLine($"{answersIter++}. {answers[i]}");

                    string? index_ = Input("Какой ответ является правильным? -> ");
                    if (int.TryParse(index_, out index) && index - 1 >= 0 && index - 1 <= answers.Count - 1) { break; }
                }

                quizzes.Last().Questions.Add(new Question(question, answers, index - 1)); iter++;
            }
            SaveJSON(defaultPATH);
            Console.WriteLine("Викторина создана!");
        }

        private async void SaveJSON(string path)
        {
            File.Delete(path);
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                await JsonSerializer.SerializeAsync(fileStream, quizzes);
        }

        private static List<Quiz> LoadJSON(string path)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                    return JsonSerializer.Deserialize<List<Quiz>>(fileStream);
            }
            catch (Exception) { return new List<Quiz>(); }
        }
    }
}
