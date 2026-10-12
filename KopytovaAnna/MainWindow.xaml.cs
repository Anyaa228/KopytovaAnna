using System;
using System.Windows;
using System.Windows.Controls;

namespace KopytovaAnna
{
    public partial class MainWindow : Window
    {
        private string _expression = "";   
        private bool _resultShown = false; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;

            if (_resultShown)
            {
                _expression = "";
                _resultShown = false;
            }

            _expression += btn.Content.ToString();
            DisplayBox.Text = _expression;
        }

        private void Op_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            string op = btn.Tag.ToString();

            // если пусто — ничего не делаем
            if (string.IsNullOrEmpty(_expression)) return;

            // если после результата нажали операцию — продолжаем с результатом
            _resultShown = false;

            // если последний символ — уже операция, заменяем её
            char last = _expression[_expression.Length - 1];
            if (last == '+' || last == '-' || last == '*' || last == '/')
            {
                _expression = _expression.Substring(0, _expression.Length - 1) + op;
            }
            else
            {
                _expression += op;
            }

            DisplayBox.Text = _expression;
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_expression)) return;

            try
            {
                double result = Calculate(_expression);
                DisplayBox.Text = _expression + " = " + result;
                _expression = result.ToString();
                _resultShown = true;
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("На ноль делить нельзя!", "Ошибка");
                Clear_Click(null, null);
            }
            catch (Exception)
            {
                MessageBox.Show("Некорректное выражение", "Ошибка");
                Clear_Click(null, null);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _expression = "";
            _resultShown = false;
            DisplayBox.Text = "0";
        }

        // Простой разбор выражения вида "8+3" или "12*5" (два числа и один оператор)
        private double Calculate(string expr)
        {
            int opIndex = -1;
            char op = ' ';

            for (int i = 1; i < expr.Length; i++)
            {
                if (expr[i] == '+' || expr[i] == '-' || expr[i] == '*' || expr[i] == '/')
                {
                    opIndex = i;
                    op = expr[i];
                    break;
                }
            }

            if (opIndex == -1)
                return double.Parse(expr); 

            double a = double.Parse(expr.Substring(0, opIndex));
            double b = double.Parse(expr.Substring(opIndex + 1));

            switch (op)
            {
                case '+': return a + b;
                case '-': return a - b;
                case '*': return a * b;
                case '/':
                    if (b == 0) throw new DivideByZeroException();
                    return a / b;
            }

            return 0;
        }
    }
}