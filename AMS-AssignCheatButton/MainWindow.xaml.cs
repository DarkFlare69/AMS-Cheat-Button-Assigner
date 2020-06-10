using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace AMS_AssignCheatButton
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int lineCount = 0;
        int currentActivator = 0;
        static List<int> buttonValues = new List<int>();
        static List<int> linesWithButtons = new List<int>();
        static List<string> codeNames = new List<string>();
        static List<int> linesWithNames = new List<int>();

        private void Button_Click(object sender, RoutedEventArgs e) // Next Button
        {
            if (textBox1.Text.Length < 8 || buttonValues.Count < 1)
            {
                ThrowError();
                return;
            }
            currentActivator++;
            if (currentActivator >= buttonValues.Count)
            {
                currentActivator = 0;
                textBoxName.Text = "Code name not found";
            }
            textBox2.Text = "Value: " + buttonValues[currentActivator].ToString("X");
            SetAllCheckBoxes(false);
            SetButtonValues(currentActivator);
            textBox3.Text = "Instance: " + (currentActivator + 1).ToString();
            TextBoxLine.Text = "Line: " + linesWithButtons[currentActivator].ToString();
            for (int i = 0; i < linesWithNames.Count; i++)
                if (linesWithButtons[currentActivator] > linesWithNames[i])
                    textBoxName.Text = codeNames[i];
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) // Previous Button
        {
            if (textBox1.Text.Length < 8 || buttonValues.Count < 1)
            {
                ThrowError();
                return;
            }
            currentActivator = (--currentActivator >= 1 ? currentActivator : 0);
            textBoxName.Text = "Code name not found";
            textBox2.Text = "Value: " + buttonValues[currentActivator].ToString("X");
            SetAllCheckBoxes(false);
            SetButtonValues(currentActivator);
            textBox3.Text = "Instance: " + (currentActivator + 1).ToString();
            TextBoxLine.Text = "Line: " + linesWithButtons[currentActivator].ToString();
            for (int i = 0; i < linesWithNames.Count; i++)
                if (linesWithButtons[currentActivator] > linesWithNames[i])
                    textBoxName.Text = codeNames[i];
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) // Save Current Activator
        {
            if (textBox1.Text.Length < 8 || buttonValues.Count < 1)
            {
                ThrowError();
                return;
            }
            string button = (SR.IsChecked == true ? "1" : "0") + (SL.IsChecked == true ? "1" : "0") + (RightStickDown.IsChecked == true ? "1" : "0") + (RightStickRight.IsChecked == true ? "1" : "0") + (RightStickUp.IsChecked == true ? "1" : "0") + (RightStickLeft.IsChecked == true ? "1" : "0") + (LeftStickDown.IsChecked == true ? "1" : "0") + (LeftStickRight.IsChecked == true ? "1" : "0") + (LeftStickUp.IsChecked == true ? "1" : "0") + (LeftStickleft.IsChecked == true ? "1" : "0") + (Down.IsChecked == true ? "1" : "0") + (Right1.IsChecked == true ? "1" : "0") + (Up.IsChecked == true ? "1" : "0") + (Left1.IsChecked == true ? "1" : "0") + (Minus.IsChecked == true ? "1" : "0") + (Plus.IsChecked == true ? "1" : "0") + (ZR.IsChecked == true ? "1" : "0") + (ZL.IsChecked == true ? "1" : "0") + (R.IsChecked == true ? "1" : "0") + (L.IsChecked == true ? "1" : "0") + (RightStickpressed.IsChecked == true ? "1" : "0") + (LeftStickPressed.IsChecked == true ? "1" : "0") + (Y.IsChecked == true ? "1" : "0") + (X.IsChecked == true ? "1" : "0") + (B.IsChecked == true ? "1" : "0") + (A.IsChecked == true ? "1" : "0");
            int but = Convert.ToInt32(button, 2);
            buttonValues[currentActivator] = but;
            List<string> allText = new List<string>();
            allText.Clear();
            StringReader lineString = new StringReader(textBox1.Text);
            while (textBox1.Text != null)
            {
                string line = lineString.ReadLine();
                if (line == null)
                    break;
                allText.Add(line);
            }
            textBox2.Text = "Value: " + but.ToString("X");
            allText[linesWithButtons[currentActivator] - 1] = "8" + but.ToString("X").PadLeft(7, '0');
            textBox1.Text = "";
            foreach (string i in allText)
                textBox1.Text += i + "\n";
        }

        public void Button_Click_1(object sender, RoutedEventArgs e) // Load Code: get all button activators into ArrayList 'buttonValues' and load names
        {
            buttonValues.Clear();
            linesWithButtons.Clear();
            codeNames.Clear();
            linesWithNames.Clear();
            lineCount = 0;
            currentActivator = 0;
            if (textBox1.Text == "")
            {
                ThrowError();
                return;
            }
            StringReader lineString = new StringReader(textBox1.Text);
            while (textBox1.Text != null)
            {
                string line = lineString.ReadLine();
                if (line == null)
                    break;
                lineCount++;
                if (line.StartsWith("8") && line.Length >= 8)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(line.Substring(1, 7), @"\A\b[0-9a-fA-F]+\b\Z")) // would rather not use regex but it works
                        buttonValues.Add(Convert.ToInt32(line.Substring(1, 7), 16));
                    else
                        buttonValues.Add(0);
                    linesWithButtons.Add(lineCount);
                    textBox2.Text = "Value: " + buttonValues[0].ToString("X");
                    SetAllCheckBoxes(false);
                    SetButtonValues(0);
                    textBox3.Text = "Instance: 1";
                    TextBoxLine.Text = "Line: " + linesWithButtons[currentActivator].ToString();
                }
                if (line.StartsWith("[") && line.Contains("]"))
                {
                    codeNames.Add(line.Substring(1, line.Length - 2));
                    linesWithNames.Add(lineCount);
                }
                if (linesWithButtons.Count > 0 && linesWithNames.Count > 0 && linesWithButtons[0] > linesWithNames[0])
                    textBoxName.Text = codeNames[0];
                else
                    textBoxName.Text = "Code name not found";
                for (int i = 0; i < linesWithNames.Count; i++)
                    if (linesWithButtons.Count > 0 && linesWithButtons[0] > linesWithNames[i])
                        textBoxName.Text = codeNames[i];
            }
            if (buttonValues.Count == 0)
                MessageBox.Show("No button activators found in your codes.", "Info");
        }

        private void LoadTextFile_Click(object sender, EventArgs e) // Text file loading
        {
            {
                OpenFileDialog dialogue = new OpenFileDialog();
                dialogue.Title = "Open Text File";
                dialogue.Filter = "TXT files|*.txt";
                if (dialogue.ShowDialog() != DialogResult.HasValue)
                {
                    string filename = dialogue.FileName;
                    string[] filelinesWithButtons = File.ReadAllLines(filename);
                    textBox1.Text = "";
                    foreach (string line in filelinesWithButtons)
                        textBox1.Text += line + "\n";
                }
            }
        }

        private void SetButtonValues(int activator) // Set the checked status of each box
        {
            for (int i = 0; i < 26; i++)
                if ((Convert.ToInt32(buttonValues[activator]) & (1 << i)) != 0) // wish i didnt have to do this, dont mind how gross the code is
                    switch (i)
                    {
                        case 0:
                            {
                                A.IsChecked = true;
                                continue;
                            }
                        case 1:
                            {
                                B.IsChecked = true;
                                continue;
                            }
                        case 2:
                            {
                                X.IsChecked = true;
                                continue;
                            }
                        case 3:
                            {
                                Y.IsChecked = true;
                                continue;
                            }
                        case 4:
                            {
                                LeftStickPressed.IsChecked = true;
                                continue;
                            }
                        case 5:
                            {
                                RightStickpressed.IsChecked = true;
                                continue;
                            }
                        case 6:
                            {
                                L.IsChecked = true;
                                continue;
                            }
                        case 7:
                            {
                                R.IsChecked = true;
                                continue;
                            }
                        case 8:
                            {
                                ZL.IsChecked = true;
                                continue;
                            }
                        case 9:
                            {
                                ZR.IsChecked = true;
                                continue;
                            }
                        case 10:
                            {
                                Plus.IsChecked = true;
                                continue;
                            }
                        case 11:
                            {
                                Minus.IsChecked = true;
                                continue;
                            }
                        case 12:
                            {
                                Left1.IsChecked = true;
                                continue;
                            }
                        case 13:
                            {
                                Up.IsChecked = true;
                                continue;
                            }
                        case 14:
                            {
                                Right1.IsChecked = true;
                                continue;
                            }
                        case 15:
                            {
                                Down.IsChecked = true;
                                continue;
                            }
                        case 16:
                            {
                                LeftStickleft.IsChecked = true;
                                continue;
                            }
                        case 17:
                            {
                                LeftStickUp.IsChecked = true;
                                continue;
                            }
                        case 18:
                            {
                                LeftStickRight.IsChecked = true;
                                continue;
                            }
                        case 19:
                            {
                                LeftStickDown.IsChecked = true;
                                continue;
                            }
                        case 20:
                            {
                                RightStickLeft.IsChecked = true;
                                continue;
                            }
                        case 21:
                            {
                                RightStickUp.IsChecked = true;
                                continue;
                            }
                        case 22:
                            {
                                RightStickRight.IsChecked = true;
                                continue;
                            }
                        case 23:
                            {
                                RightStickDown.IsChecked = true;
                                continue;
                            }
                        case 24:
                            {
                                SL.IsChecked = true;
                                continue;
                            }
                        case 25:
                            {
                                SR.IsChecked = true;
                                continue;
                            }
                        default:
                            break;
                    }
        }

        private void SetAllCheckBoxes(bool value) // set all check boxes
        {
            A.IsChecked = value;
            B.IsChecked = value;
            X.IsChecked = value;
            Y.IsChecked = value;
            LeftStickPressed.IsChecked = value;
            RightStickpressed.IsChecked = value;
            L.IsChecked = value;
            R.IsChecked = value;
            ZL.IsChecked = value;
            ZR.IsChecked = value;
            Plus.IsChecked = value;
            Minus.IsChecked = value;
            Left1.IsChecked = value;
            Up.IsChecked = value;
            Right1.IsChecked = value;
            Down.IsChecked = value;
            LeftStickleft.IsChecked = value;
            LeftStickUp.IsChecked = value;
            LeftStickRight.IsChecked = value;
            LeftStickDown.IsChecked = value;
            RightStickLeft.IsChecked = value;
            RightStickUp.IsChecked = value;
            RightStickRight.IsChecked = value;
            RightStickDown.IsChecked = value;
            SL.IsChecked = value;
            SR.IsChecked = value;
        }

        private void Info_Click(object sender, RoutedEventArgs e) // Info tab
        {
            MessageBox.Show("This is a program that will accept Atmosphere or SX OS codes as an input and read/change their button activators. This can be useful since everyone has a different taste in what buttons they would like to use, often different from the code creator.\n\nCredits:\n\n-Program and conversions by DarkFlare", "About");
        }

        void    ThrowError() // generic
        {
            MessageBox.Show("Enter codes in the textbox on the right or load them from a txt file. Once completed, press the \"Load Codes\" button.", "Info");
        }
    }
}
