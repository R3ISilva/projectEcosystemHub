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
using AI;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //center the screen
            WindowStartupLocation = WindowStartupLocation.Manual;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
            textBox.Focus();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string userInput = textBox.Text;
                TextHandler(userInput);
                textBox.Text = string.Empty;
            }
        }

        private void TextHandler(string userInputRaw)
        {
            string userInputCleaned;
            userInputCleaned = userInputRaw.Trim();

            ExecuteCommands(SeperateCommands(userInputCleaned));
        }

        public async void ExecuteCommands(KeyValuePair<string, string> inputText)
        {
            if (inputText.Key == Commands.exit)
            {
                if (inputText.Value != null)
                {
                    // handle quando ha "exit texto"
                }

                Application.Current.Shutdown();
                return;
            }

            #region AI COMMANDS

            OpenAIAPI OpenAI;

            if (inputText.Key == Commands.complete)
            {
                OpenAI = new OpenAIAPI();

                string result = await OpenAI.Completions.GetCompletion(inputText.Value);

                textBox.Text = result;
            }
            #endregion
        }


        private KeyValuePair<string, string> SeperateCommands(string userInputCleaned)
        {
            string command = string.Empty;
            string inputString = string.Empty;

            if (userInputCleaned.Contains(" "))
            {
                string[] parts = userInputCleaned.Split(new char[] { ' ' }, 2);
                command = parts[0].ToLower(); //commands to lower
                inputString = parts[1];

                foreach (var field in typeof(Commands).GetFields())
                {
                    if (field.GetValue(null).ToString() == inputString)
                    {
                        command = field.GetValue(null).ToString();
                    }
                }

            }
            else
            {
                command = null;
                inputString = userInputCleaned;
            }

            return new KeyValuePair<string, string>(command, inputString);
        }
    }
}
