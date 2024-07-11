using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using WinFormsApp1.Model;

namespace WinFormsApp1.ViewModel
{
    public class MainViewModel
    {
        public List<WorkTasks> TasksList = new List<WorkTasks>();
        public void CreateTask()
        {
            TasksList.Add(new WorkTasks 
            { taskName = "", taskDescription = "", subtasks = new List<Tuple<bool, string>>(),completed = false });
        }

        public void DeleteTask(int Id)
        {
            TasksList.RemoveAt(Id);
        }
        
        string fileName = "modelSave.json";
        public void SaveTasksList()
        {

            string jsonString = JsonSerializer.Serialize(TasksList);
            File.WriteAllText(fileName, jsonString);
        }

        public void loadTasksList() 
        {
            string jsonString = File.ReadAllText(fileName);
            TasksList = JsonSerializer.Deserialize<List<WorkTasks>>(jsonString);
        }
    }
}
