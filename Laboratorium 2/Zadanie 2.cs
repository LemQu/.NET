using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Kalkulator
{
    public partial class Calculator : Form
    {
        private double currentValue = 0;
        private string currentOperation = "";
        private bool isNewValue = true;

        public Calculator()
        {
            InitializeComponent();
        }

        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;

            if (isNewValue)
            {
                textBoxDisplay.Text = buttonText;
                isNewValue = false;
            }
            else
            {
                textBoxDisplay.Text += buttonText;
            }
        }

        private void OperationButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string operation = button.Text;

            if (!isNewValue)
            {
                Calculate();
                currentOperation = operation;
                isNewValue = true;
            }
            else
            {
                currentOperation = operation;
            }
            currentValue = double.Parse(textBoxDisplay.Text);
        }

        private void EqualsButton_Click(object sender, EventArgs e)
        {
            Calculate();
            isNewValue = true;
        }

        private void Calculate()
        {
            double newValue = double.Parse(textBoxDisplay.Text);
            switch (currentOperation)
            {
                case "+":
                    currentValue += newValue;
                    break;
                case "-":
                    currentValue -= newValue;
                    break;
                case "*":
                    currentValue *= newValue;
                    break;
                case "/":
                    if (newValue == 0)
                    {
                        MessageBox.Show("Nie mo¿na dzieliæ przez zero", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    currentValue /= newValue;
                    break;
            }
            textBoxDisplay.Text = currentValue.ToString();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            currentValue = 0;
            currentOperation = "";
            isNewValue = true;
            textBoxDisplay.Text = "0";
        }

        private void CalculatorForm_Load(object sender, EventArgs e)
        {
            // pomiar czasu
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // symulacja d³ugiego procesu inicjalizacji
            System.Threading.Thread.Sleep(2000);

            stopwatch.Stop();

            // je¿eli czas inicjalizacji jest zbyt d³ugi to zapisz do rejestru zdarzeñ
            if (stopwatch.ElapsedMilliseconds > 1000)
            {
                EventLog.WriteEntry("Application", $"Czas inicjalizacji przekracza 1000 ms: {stopwatch.ElapsedMilliseconds} ms", EventLogEntryType.Warning);
            }
        }
    }
}