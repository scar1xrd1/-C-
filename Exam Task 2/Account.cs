using System.Security.Principal;
using System.Text.Json;
namespace Exam_Task_2
{
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

    record class Account(string Login, string Password);

    class AccountManager : InputMethods
    {
        public List<Account> accounts { get; set; } = LoadJSON(defaultPATH);
        private static string defaultPATH = "account.json";

        public AccountManager() { }

        public void LoginAccount()
        {
            if(accounts.Count > 0)
            {
                string? login = InputCycle("Введите логин -> ");
                string? password = InputCycle("Введите пароль -> ");

                bool hasLogin = false, hasPassword = false;

                for (int i = 0; i < accounts.Count; i++)
                {
                    if (accounts[i].Login == login)
                    {
                        hasLogin = true;
                        if (accounts[i].Password == password) { hasPassword = true; break; }
                        break;
                    }
                }

                if(hasLogin)
                {
                    if(hasPassword)
                    {
                        // ВХОД ВЫПОЛНЕН
                    }
                    else Console.WriteLine();
                }
            }
            else Console.WriteLine("Ещё не создано ни одного аккаунта!");
        }

        public void RegisterAccount()
        {
            string? login;
            while(true)
            {
                login = InputCycle("Введите логин -> ");
                if (accounts.Count == 0) break;
                else
                {
                    bool hasLogin = false;
                    foreach (var account in accounts)
                        if(account.Login == login) { hasLogin = true; break; }
                    if (hasLogin) Console.WriteLine("Логин занят!");
                    else break;
                }
            }
            string? password = InputCycle("Введите пароль -> ");
            accounts.Add(new Account(login, password));
            SaveJSON(defaultPATH);
            Console.WriteLine("Аккаунт создан!");            
        }

        private async void SaveJSON(string path)
        {
            File.Delete(path);
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                await JsonSerializer.SerializeAsync(fileStream, accounts);
        }

        private static List<Account> LoadJSON(string path)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                    return JsonSerializer.Deserialize<List<Account>>(fileStream);
            }
            catch (Exception) { return new List<Account>(); }
        }
    }
}
