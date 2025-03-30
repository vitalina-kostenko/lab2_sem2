using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace WPFCalculator
{
    public partial class MainWindow : Window
    {
        private string input = "";
        private double num1 = 0;
        private double num2 = 0;
        private char operation;
        private Stack<string> history = new Stack<string>();
        private bool isExpanded = false;
        private bool isPowerMode = false; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            input += button.Content.ToString();
            Display.Text = input;
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (!double.TryParse(input, out num1)) return;
            operation = button.Content.ToString()[0];
            history.Push(input);
            input = "";
            isPowerMode = false; 
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(input, out num2))
            {
                input = input.Replace("π", Math.PI.ToString())
                             .Replace("e", Math.E.ToString())
                             .Replace("^", "**");  

                if (!double.TryParse(input, out num2)) return;
            }

            double result = 0;

            switch (operation)
            {
                case '+': result = num1 + num2; break;
                case '-': result = num1 - num2; break;
                case '*': result = num1 * num2; break;
                case '/': result = num2 != 0 ? num1 / num2 : double.NaN; break;
                case '^': result = Math.Pow(num1, num2); break; 
            }

            Display.Text = result.ToString();
            history.Push(input);
            input = result.ToString();
            isPowerMode = false;
        }

        private void Power_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(input, out num1)) return;
            operation = '^';
            history.Push(input);
            input = "";
            isPowerMode = true;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            input = "";
            num1 = num2 = 0;
            history.Clear();
            Display.Text = "0";
            isPowerMode = false;
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (history.Count > 0)
            {
                input = history.Pop();
                Display.Text = input;
            }
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (input.Length > 0)
            {
                input = input.Substring(0, input.Length - 1);
                Display.Text = input.Length > 0 ? input : "0";
            }
        }

        private void BurgerMenu_Click(object sender, RoutedEventArgs e)
        {
            isExpanded = !isExpanded;
            ExtraColumn.Width = isExpanded ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
            BurgerButton.Content = isExpanded ? "↩" : "☰";

            foreach (UIElement element in ButtonsGrid.Children)
            {
                if (Grid.GetColumn(element) == 4)
                {
                    element.Visibility = isExpanded ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void Pi_Click(object sender, RoutedEventArgs e)
        {
            input += "π"; 
            Display.Text = input;
        }

        private void E_Click(object sender, RoutedEventArgs e)
        {
            input += "e"; 
            Display.Text = input;
        }

        private void Sqrt_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = Math.Sqrt(double.Parse(Display.Text)).ToString();
        }

        private void Log_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = Math.Log10(double.Parse(Display.Text)).ToString();
        }
    }
}
