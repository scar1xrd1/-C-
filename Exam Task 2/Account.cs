using System.Security.Principal;
using System.Text.Json;
namespace Exam_Task_2
{
    record class Account(string Login, string Password, DateTime DateOfBirth);

    class AccountManager : InputMethods
    {
        public List<Account> accounts { get; set; } = LoadJSON(defaultPATH);
        private static string defaultPATH = "account.json";

        public AccountManager() { }

        public int LoginAccount()
        {
            if(accounts.Count > 0)
            {
                string? login = InputCycle("Введите логин -> ");
                string? password = InputCycle("Введите пароль -> ");
                int index = 0;

                bool hasLogin = false, hasPassword = false;

                for (int i = 0; i < accounts.Count; i++)
                {
                    if (accounts[i].Login == login)
                    {
                        index = i;
                        hasLogin = true;
                        if (accounts[i].Password == password) { hasPassword = true; break; }
                        break;
                    }
                }

                if(hasLogin)
                {
                    if(hasPassword)
                    {
                        Console.WriteLine("Вы вошли в аккаунт!");
                        return index;
                        // ВХОД ВЫПОЛНЕН
                    }
                    else Console.WriteLine("Неверный пароль!");
                }
                else Console.WriteLine("Такого логина не существует!");
            }
            else Console.WriteLine("Ещё не создано ни одного аккаунта!");
            return -1;
        }

        public void RegisterAccount()
        {
            string? login;
            DateTime dateOfBirth;

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

            while(true)
            {
                string? date = Input("Введите дату рождения в формате 22/05/2006 -> ");
                if(DateTime.TryParse(date, out dateOfBirth) && dateOfBirth < DateTime.Now) { break; }
                else Console.WriteLine("Вы ввели некорректную дату, либо вы ещё не родились!");
            }

            accounts.Add(new Account(login, password, dateOfBirth));
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
