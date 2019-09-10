using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Schedule1
{
    public class Program
    {
        static string connStr = "Server = localhost; User = root; Database = db_tasks; port = 3308; password = '' ";
        static MySqlConnection conn = new MySqlConnection(connStr);
        static List<Task> tasks = new List<Task>();
        static int lastTask;

        public static void Main(string[] args)
        {
            string id, taskName;
            GetTasks();
            while (true)
            {
                int Act = ShowMenu();
                switch (Act)
                {
                    case 1:
                        Console.WriteLine("Введите задание:");
                        taskName = Console.ReadLine();
                        Add(taskName);
                        lastTask++;
                        break;
                    case 2:
                        Console.WriteLine("Введите номер задачи:");
                        id = Console.ReadLine();
                        Console.WriteLine("Введите содержание задачи:");
                        taskName = Console.ReadLine();
                        Update(id, taskName);
                        break;
                    case 3:
                        Console.WriteLine("Введите номер задачи:");
                        id = Console.ReadLine();
                        Delete(id);
                        break;
                    case 4:
                        ShowTasks();
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                }
            }

        }

        static int ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("----------------Меню--------------");
            Console.WriteLine("Выберете действие:");
            Console.WriteLine("1. Добавить задачу.");
            Console.WriteLine("2. Изменить задачу.");
            Console.WriteLine("3. Удалить задачу.");
            Console.WriteLine("4. Показать задачи.");
            Console.WriteLine("5. Выход.");
            Console.WriteLine("----------------------------------");
            return Convert.ToInt32(Console.ReadLine());
        }

        static void GetTasks()
        {
            try
            {
                conn.Open();
                string sql = "select * from tasks_table";
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tasks.Add(new Task() { Id = Convert.ToInt32(reader[0]), Name =  reader[1].ToString() });
                    lastTask = Convert.ToInt32(reader[0]);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        static void ShowTasks()
        {
            Console.WriteLine();
            Console.WriteLine("--------------Задачи--------------");
            foreach (Task task in tasks)
            {
                Console.WriteLine("{0} - {1}", task.Id, task.Name);
            }
            Console.WriteLine("----------------------------------");
        }

        static void Add(string taskName)
        {
            
            try
            {
                conn.Open();
                string sql = "insert into tasks_table (task_name) VALUES ('" + taskName +"')";
                MySqlCommand command = new MySqlCommand(sql, conn);
                int add = command.ExecuteNonQuery();
                if (add != 0)
                {
                    tasks.Add(new Task() {Id = lastTask+1, Name = taskName });
                    Console.WriteLine("Задача добавлена.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        static void Update(string id, string taskName)
        {
            try
            {
                conn.Open();
                string sql = "update tasks_table set task_name = '" + taskName + "' where id = " + id;
                MySqlCommand command = new MySqlCommand(sql, conn);
                int update = command.ExecuteNonQuery();
                if (update != 0)
                {
                    Task taskToRemove = tasks.Find(task => task.Id == Convert.ToInt32(id));
                    tasks.Remove(taskToRemove);
                    tasks.Add(new Task() { Id = Convert.ToInt32(id), Name = taskName });
                    Console.WriteLine("Задача изменена.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();

        }

        static void Delete(string id)
        {
            try
            {
                conn.Open();
                string sql = "delete from tasks_table where id = " + id;
                MySqlCommand command = new MySqlCommand(sql, conn);
                int remove = command.ExecuteNonQuery();
                if (remove != 0)
                {
                    Task taskToRemove = tasks.Find(task => task.Id == Convert.ToInt32(id));
                    tasks.Remove(taskToRemove);
                    Console.WriteLine("Задача удалена.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();

        }
    }
}
