using System;
using System.Drawing;
using System.Windows.Forms;

public class InputForm : Form
{
    public string ImageUrl { get; private set; }

    public InputForm()
    {
        InitializeForm();
        InitializeControls();
    }

    private void InitializeForm()
    {
        this.Text = "Image Input";
        this.Size = new Size(300, 150);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
    }

    private void InitializeControls()
    {
        Label label = new Label
        {
            Text = "Enter Image URL:",
            Location = new Point(10, 20),
            AutoSize = true,
        };

        TextBox textBox = new TextBox
        {
            Location = new Point(10, 50),
            Size = new Size(250, 20),
        };

        Button okButton = new Button
        {
            Text = "OK",
            Location = new Point(10, 80),
            DialogResult = DialogResult.OK,
        };

        this.Controls.AddRange(new Control[] { label, textBox, okButton });

        okButton.Click += (sender, e) => ImageUrl = textBox.Text;
    }
}
