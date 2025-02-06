using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        private List<User> users = new List<User>();
        public Form2()
        {
            InitializeComponent();

            users = GenerateRandomUsers(100);
        }


        private List<User> GenerateRandomUsers(int count)
        {
            var random = new Random();
            var usersList = new List<User>();
            string[] firstNames = { "Juan", "Carlos", "Marta", "José", "Ana", "Daniela", "Lucía", "Pedro", "Raúl", "Sofía" };
            string[] lastNames = { "García", "Martínez", "Rodríguez", "López", "González", "Pérez", "Hernández", "Díaz", "Álvarez", "Jiménez" };

            for (int i = 1; i <= count; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                var user = new User
                {
                    Id = i,
                    UserName = $"{firstName} {lastName}"
                };
                usersList.Add(user);
            }

            return usersList;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            jmComboBox1.DisplayMember = "UserName";
            jmComboBox1.ValueMember = "Id";
            //jmComboBox1.SelectdValue = 1;
            jmComboBox1.DataSource = users;

            jmComboBox1.SelectedValue = 6;


        }



        private void jmComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var dsfr = jmComboBox1.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dfg = jmComboBox1.SelectedValue;
            var asfr = jmComboBox1.SelectedIndex;
            var objASFDS = jmComboBox1.SelectedItem;

        }
    }
}
