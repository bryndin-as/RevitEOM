using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EditCableStream.View
{
    /// <summary>
    /// Логика взаимодействия для EditCableStreamView.xaml
    /// </summary>
    public partial class EditCableStreamView : Window
    {
        public EditCableStreamView()
        {
            InitializeComponent();
            
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)

        {
             this.Close();
        }
    }
}
