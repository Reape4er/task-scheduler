using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Model;

public class WorkTasks
{
    public string taskName { get; set; }
    public string taskDescription { get; set; }
    public List<Tuple<bool, string>> subtasks { get; set; }
    public bool completed { get; set; }
}