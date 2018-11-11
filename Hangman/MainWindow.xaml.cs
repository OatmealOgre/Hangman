using System;
using System.Collections.Generic;
using System.IO;
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
        private int attempts, difficulty;
        private const int MAX_ATTEMPTS = 16; // 16 maxium guesses because we have 16 images
        private String Word;
        private enum DifficultyLevel
        {
            EASY = 1,   // One guess is one guess
            MEDIUM = 2, // One guess costs two attempts
            HARD = 3    // Costs three attempts
        }

        public MainWindow()
        {
            InitializeComponent();

            attempts = MAX_ATTEMPTS;
            difficulty = (int)DifficultyLevel.MEDIUM; 
            Word = "";
            DisableButtons();
        }

        // Most of the logic happens here when a guess is made
        private void MakeGuess(char c)
        {
            if (attempts < MAX_ATTEMPTS)
            {
                
                int cIndex = Word.IndexOf(c);

                if (cIndex < 0) // No index of the letter in the word -> wrong guess
                {
                    attempts += difficulty;
                    SetHangmanImage(attempts);

                    if (attempts >= MAX_ATTEMPTS)
                    {
                        DisableButtons();
                        WordBox.Text = "You Lost! Try again!";
                    }
                }
                else
                {
                    StringBuilder BoxText = new StringBuilder(WordBox.Text);
                    while (cIndex >= 0)
                    {
                        BoxText[cIndex * 2] = c;
                        cIndex = Word.IndexOf(c, cIndex + 1);
                    }

                    // Check if there are still underlines in the word - otherwise the player won
                    if (!BoxText.ToString().Contains('_'))
                    {
                        BoxText.Append("- You Win!");
                        DisableButtons();
                    }

                    WordBox.Text = BoxText.ToString();
                }
                
            }
        }

        // Called to reset elements for the game
        private void ResetGame()
        {
            attempts = 0;

            SetNewWord();

            // Enable all buttons that may have been disabled
            foreach (Button btn in Wp.Children.OfType<Button>())
            {
                btn.IsEnabled = true;
            }

            // Reset the Hangman image to the first
            SetHangmanImage(0);

            // Set the Difficulty
            if (EasyBtn.IsChecked ?? false)
            {
                difficulty = (int)DifficultyLevel.EASY;
            }
            else if (MediumBtn.IsChecked ?? false)
            {
                difficulty = (int)DifficultyLevel.MEDIUM;
            } 
            else
            {
                difficulty = (int)DifficultyLevel.HARD;
            }
        }

        // Disable buttons on win / loss / start
        private void DisableButtons()
        {
            foreach (Button btn in Wp.Children.OfType<Button>())
            {
                btn.IsEnabled = false;
            }
        }

        private void SetHangmanImage(int index)
        {
            if (index < 0) index = 0;
            else if (index > 16) index = 16;

            String path = "/Images/hangman_"+ index +".png"; // Grab the path to the corresponding image

            HangmanImage.Source = new BitmapImage(new Uri(path, UriKind.Relative));
            if (HangmanImage.Source == null)
            {
                throw new System.Exception("Hangman images are missing!");
            }
        }

        // Used to get a new random word from file
        private void SetNewWord()
        {
            String[] Lines = File.ReadAllLines("words.txt"); // This will throw an exception if the file is missing
            Word = Lines[new Random().Next(Lines.Length)].ToUpper(); // Get the word of a random line and make sure it's upper case

            String shownText = "";
            foreach (char c in Word)
            {
                if (c == ' ')
                {
                    shownText += "  ";
                }
                else
                {
                    shownText += "_ "; // Display blanks for each letter in the word
                }
            }

            WordBox.Text = shownText;
        }

        // Add a button for each letter of the alphabet once the WrapPanel has been initialized
        private void Wp_Initialized(object sender, EventArgs e)
        {
            const int ASCII_LETTER_INDEX = 65; // Start of alphabet in ascii

            for (int i = 0; i < 26; i++)
            {
                // Create a button for each letter of the alphabet
                System.Windows.Controls.Button addBtn = new Button
                {
                    Content = (char)(ASCII_LETTER_INDEX + i) //,
                    //Name = "button" + (char)(ASCII_LETTER_INDEX + i)
                };

                // Event when each button is pressed
                addBtn.Click += (s, ev) =>
                {
                    addBtn.IsEnabled = false; // We can't use the same letter more than once, so let's disable it
                    MakeGuess(addBtn.Content.ToString()[0]); // Make a guess based on the letter on the button
                };

                Wp.Children.Add(addBtn); // Add the buttons to the WrapPanel
            }
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }
    }
}
