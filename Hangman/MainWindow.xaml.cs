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

namespace Hangman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int attempts;
        private const int MAX_ATTEMPTS = 16;

        public MainWindow()
        {
            InitializeComponent();

            attempts = 0;
        }

        private void ResetGame()
        {
            attempts = 0;

            foreach (Button btn in wp.Children.OfType<Button>())
            {
                btn.IsEnabled = true;
                
            }
        }

        // Add a button for each letter of the alphabet once the WrapPanel has been initialized
        private void Wp_Initialized(object sender, EventArgs e)
        {
            const int ASCII_LETTER_INDEX = 65; // Start of alphabet in ascii

            for (int i = 0; i < 26; i++)
            {
                System.Windows.Controls.Button addBtn = new Button
                {
                    Content = (char)(ASCII_LETTER_INDEX + i),
                    Name = "button" + (char)(ASCII_LETTER_INDEX + i)
                };

                wp.Children.Add(addBtn);
            }
        }
    }
}
