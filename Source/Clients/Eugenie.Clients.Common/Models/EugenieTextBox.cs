namespace Eugenie.Clients.Common.Models
{
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class EugenieTextBox : TextBox
    {
        public EugenieTextBox()
        {
            this.GotFocus += this.OnGotFocus;
            this.LostFocus += this.OnLostFocus;
            this.PreviewTextInput += this.OnPreviewTextInput;
        }

        public bool AutoSelect { get; set; }

        public bool BlockInvalidNumberCharacters { get; set; }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (this.AutoSelect
                && !Mouse.LeftButton.HasFlag(MouseButtonState.Pressed)
                && !Mouse.RightButton.HasFlag(MouseButtonState.Pressed)
                && !Mouse.MiddleButton.HasFlag(MouseButtonState.Pressed))
            {
                this.SelectAll();
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.Select(0, 0);
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.BlockInvalidNumberCharacters)
            {
                var regex = new Regex(@"[0-9-,]+$");
                e.Handled = !regex.IsMatch(e.Text);
            }
        }
    }
}