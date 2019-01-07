using System;
using System.Windows.Forms;

namespace BlackOut
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainWindow juego = new MainWindow(this);
            juego.ShowDialog();
        }
    }
}
