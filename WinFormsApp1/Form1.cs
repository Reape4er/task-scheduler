using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Model;
using WinFormsApp1.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private MainViewModel model = new MainViewModel();
        public Form1()
        {
            InitializeComponent();
            try
            {
                model.loadTasksList();
                for (int i = 0; i < model.TasksList.Count; i++)
                {
                    taskButtonMaker(model.TasksList[i].taskName, model.TasksList[i].completed);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
            }
            catch
            {
                MessageBox.Show("Загрузка списка не удалась");
            }
        }
        private List<Button> buttons = new List<Button>();
        private int currentTaskNumber = 0;
        private void taskButtonMaker(string buttonText, bool Completed)
        {
            Button newButton = new Button();
            newButton.Size = new Size(273, 38);
            newButton.Text = buttonText;
            newButton.BackColor = Color.WhiteSmoke;
            newButton.Click += (sender, e) =>
            {
                currentTaskNumber = buttons.IndexOf(newButton);
                loadtask(currentTaskNumber);
            };
            buttons.Add(newButton);
            if (Completed)
            {
                flowLayoutPanel3.Controls.Add(newButton);
            }
            else
            {
                flowLayoutPanel1.Controls.Add(newButton);
            }
        }

        List<CheckBox> checkBoxes = new List<CheckBox>();
        List<TextBox> textBoxes = new List<TextBox>();
        List<Tuple<bool, string>> currentSubtaskList = new List<Tuple<bool, string>>();
        private void subtaskButtonMaker(bool completed, string text)
        {
            Panel newPanel = new Panel();
            newPanel.Size = new Size(635, 23);
            CheckBox newCheckBox = new CheckBox();
            if (completed)
            {
                newCheckBox.Checked = true;
            }
            newCheckBox.Size = new Size(22, 23);


            TextBox newTextBox = new TextBox();
            newTextBox.Size = new Size(604, 23);
            newTextBox.Location = new Point(23, 0);
            newTextBox.Text = text;
            
            if (newCheckBox.Checked)
            {
                Font currentFont = newTextBox.Font;
                newTextBox.Font = new Font(currentFont, currentFont.Style | FontStyle.Strikeout);
            }
            else
            {
                Font currentFont = newTextBox.Font;
                newTextBox.Font = new Font(currentFont, currentFont.Style & ~FontStyle.Strikeout);
            }
            
            newTextBox.Leave += (sender, e) =>
            {
                var t = new System.Tuple<bool, string>(newCheckBox.Checked, newTextBox.Text);
                currentSubtaskList[textBoxes.IndexOf(newTextBox)] = t;
                model.TasksList[currentTaskNumber].subtasks[textBoxes.IndexOf(newTextBox)] = t;
                model.SaveTasksList();
            };
            newCheckBox.CheckedChanged += (sender, e) =>
            {
                var t = new System.Tuple<bool, string>(newCheckBox.Checked, newTextBox.Text);
                currentSubtaskList[textBoxes.IndexOf(newTextBox)] = t;
                model.TasksList[currentTaskNumber].subtasks[textBoxes.IndexOf(newTextBox)] = t;
                if (newCheckBox.Checked)
                {
                    Font currentFont = newTextBox.Font;
                    newTextBox.Font = new Font(currentFont, currentFont.Style | FontStyle.Strikeout);
                }
                else
                {
                    Font currentFont = newTextBox.Font;
                    newTextBox.Font = new Font(currentFont, currentFont.Style & ~FontStyle.Strikeout);
                }
                model.SaveTasksList();
            };

            flowLayoutPanel2.Controls.Add(newPanel);
            newPanel.Controls.Add(newCheckBox);
            checkBoxes.Add(newCheckBox);
            newPanel.Controls.Add(newTextBox);
            textBoxes.Add(newTextBox);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            model.CreateTask();
            currentTaskNumber = buttons.Count - 1;
            taskButtonMaker("Новая задача", false);
            loadtask(buttons.Count - 1);
        }

        private void loadtask(int id)
        {
            //currentSubtaskList.Clear();
            checkBoxes.Clear();
            textBoxes.Clear();
            flowLayoutPanel2.Controls.Clear();
            currentTaskNumber = id;
            currentSubtaskList = model.TasksList[id].subtasks;
            textBox1.Text = model.TasksList[id].taskName;
            textBox2.Text = model.TasksList[id].taskDescription;
            checkBox1.Checked = model.TasksList[id].completed;

            if (currentSubtaskList.Count != 0)
                for (int i = 0; i < currentSubtaskList.Count - 1; i++)
                {
                    subtaskButtonMaker(currentSubtaskList[i].Item1, currentSubtaskList[i].Item2);
                }
            groupBox1.Enabled = true;
            groupBox1.Visible = true;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            model.TasksList[currentTaskNumber].taskName = textBox1.Text;
            buttons[currentTaskNumber].Text = textBox1.Text;
            model.SaveTasksList();
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            model.TasksList[currentTaskNumber].taskDescription = textBox2.Text;
            model.SaveTasksList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            model.DeleteTask(currentTaskNumber);
            buttons[currentTaskNumber].Dispose();
            buttons.RemoveAt(currentTaskNumber);
            flowLayoutPanel2.Controls.Clear();
            groupBox1.Enabled = false;
            groupBox1.Visible = false;
            model.SaveTasksList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var t = new System.Tuple<bool, string>(false, "");
            currentSubtaskList.Add(t);
            model.TasksList[currentTaskNumber].subtasks.Add(t);
            subtaskButtonMaker(false, "");

        }

        private void checkBox1_MouseClick(object sender, MouseEventArgs e)
        {
            model.TasksList[currentTaskNumber].completed = checkBox1.Checked;
            model.SaveTasksList();
            if (checkBox1.Checked)
            {
                flowLayoutPanel1.Controls.Remove(buttons[currentTaskNumber]);
                flowLayoutPanel3.Controls.Add(buttons[currentTaskNumber]);
                Font currentFont = textBox1.Font;
                textBox1.Font = new Font(currentFont, currentFont.Style | FontStyle.Strikeout);
            }
            else
            {
                flowLayoutPanel3.Controls.Remove(buttons[currentTaskNumber]);
                flowLayoutPanel1.Controls.Add(buttons[currentTaskNumber]);
                Font currentFont = textBox1.Font;
                textBox1.Font = new Font(currentFont, currentFont.Style & ~FontStyle.Strikeout);
            }
        }
    }
}