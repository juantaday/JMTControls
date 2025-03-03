using JMTControls.NetCore.Controls;
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

        private ContextMenuStrip successMenuStrip;
        private ToolStripMenuItem successMessageItem;
        private PopupMessageContro _successMessageControl  =  new PopupMessageContro();    

        public Form2()
        {
            InitializeComponent();

            users = GenerateRandomUsers(100);

            // Crear el ContextMenuStrip
            successMenuStrip = new ContextMenuStrip();
            successMessageItem = new ToolStripMenuItem("¡Operación exitosa!");

            // Configurar el ícono
            successMessageItem.Image = global::WinFormsApp1.Properties.Resources.eyes_20; // Asegúrate de tener el ícono en tus recursos
            successMessageItem.ImageAlign = ContentAlignment.MiddleLeft;

            // Agregar el mensaje al ContextMenuStrip
            successMenuStrip.Items.Add(successMessageItem);

            // Configurar el tiempo de duración para que desaparezca
            System.Windows.Forms.Timer disappearTimer = new System.Windows.Forms.Timer();
            disappearTimer.Interval = 2000; // Duración en milisegundos
            disappearTimer.Tick += (s, e) =>
            {
                successMenuStrip.Visible = false;
                disappearTimer.Stop();
            };

            // Mostrar el menú
            successMenuStrip.Opening += (s, e) =>
            {
                disappearTimer.Start();
            };
       

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
           
        }



        private void button2_Click(object sender, EventArgs e)
        {
            _successMessageControl.ShowSuccessMessage(sender as Control,
                       "Mensaje de prueba.\nRevice por favor",
                       Color.Red,
                       Color.White, SystemIcons.Information.ToBitmap());

        }

        // Método para mostrar el mensaje de éxito en el punto de clic
        public void ShowSuccessMessage(Control control, string message, Color backgroundColor,
            Color textColor, Image image)
        {
            // Establecer los valores del mensaje
            successMessageItem.Text = message;
            successMessageItem.ForeColor = textColor;
            successMessageItem.BackColor = backgroundColor;
            successMessageItem.Image = image;
            successMessageItem.Height = 100;

            // Obtener la ubicación del clic y mostrar el menú
            Point controlLocation = control.PointToClient(Cursor.Position); // Obtener la ubicación del clic en coordenadas del control
            successMenuStrip.Show(control, controlLocation);
        }
    }
}
