using System;
using System.Drawing;
using System.Windows.Forms;

public class PuzzleForm : Form
{
    private SliderPuzzle sliderPuzzle;
    private Button[,] puzzleButtons;
    private Label instructionLabel;

    public PuzzleForm()
    {
        try
        {
            InitializeForm();

            string imageUrl = GetImageUrlFromUser();
            Console.WriteLine($"Image URL: {imageUrl}");

            sliderPuzzle = new SliderPuzzle(3);
            sliderPuzzle.LoadImage(imageUrl);

            Console.WriteLine("Image Loaded");

            InitializePuzzleButtons();
            InitializeInstructionLabel();
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }

    private void InitializeForm()
    {
        this.Text = "9-Piece Slider Puzzle";
        this.Size = new Size(300, 400);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.FormClosing += PuzzleForm_FormClosing;
    }

    private string GetImageUrlFromUser()
    {
        string imageUrl = "";

        try
        {
            using (var form = new InputForm())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                    imageUrl = form.ImageUrl;
                else
                    ExitWithMessage("User canceled image input.");
            }
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }

        return imageUrl;
    }

    private void InitializePuzzleButtons()
    {
        puzzleButtons = new Button[3, 3];

        int buttonSize = 80;
        int margin = 10;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Button button = new Button
                {
                    Size = new Size(buttonSize, buttonSize),
                    Location = new Point(j * (buttonSize + margin), i * (buttonSize + margin)),
                };

                button.Click += PuzzleButton_Click;
                puzzleButtons[i, j] = button;
                this.Controls.Add(button);
            }
        }

        UpdatePuzzleButtons();
    }

    private void UpdatePuzzleButtons()
    {
        try
        {
            PuzzlePiece[,] puzzle = sliderPuzzle.GetPuzzle();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (puzzle[i, j] != null && puzzle[i, j].Image != null)
                    {
                        // Ensure the Image is a valid Bitmap before setting it
                        if (puzzle[i, j].Image is Bitmap bitmap)
                        {
                            puzzleButtons[i, j].Image = bitmap;
                        }
                        else
                        {
                            // Log a message or show a MessageBox for invalid image types
                            Console.WriteLine($"Error: Puzzle piece at ({i}, {j}) has an invalid image type.");
                        }
                    }
                    else
                    {
                        // Log a message or show a MessageBox for null or missing images
                        Console.WriteLine($"Error: Puzzle piece at ({i}, {j}) is null or has a null image.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating puzzle buttons: {ex.Message}");
        }
    }


    private void InitializeInstructionLabel()
    {
        instructionLabel = new Label
        {
            Text = "Instructions:\n1. Click a tile next to the empty space.\n2. Repeat until the puzzle is solved.",
            AutoSize = true,
            Location = new Point(10, 300),
        };

        this.Controls.Add(instructionLabel);
    }

    private void PuzzleButton_Click(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (puzzleButtons[i, j] == clickedButton)
                {
                    int direction = DetermineDirection(i, j);
                    sliderPuzzle.MoveTile(i, j, direction);
                    UpdatePuzzleButtons();

                    if (sliderPuzzle.IsSolved())
                        MessageBox.Show("Congratulations! Puzzle solved!");
                }
            }
        }
    }

    private int DetermineDirection(int row, int col)
    {
        if (row == sliderPuzzle.EmptyRow && col == sliderPuzzle.EmptyCol - 1)
            return 3; // Right
        else if (row == sliderPuzzle.EmptyRow && col == sliderPuzzle.EmptyCol + 1)
            return 2; // Left
        else if (row == sliderPuzzle.EmptyRow - 1 && col == sliderPuzzle.EmptyCol)
            return 1; // Down
        else if (row == sliderPuzzle.EmptyRow + 1 && col == sliderPuzzle.EmptyCol)
            return 0; // Up

        return -1; // Default value (should not happen if the buttons are set up correctly)
    }

    private void PuzzleForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.No)
            e.Cancel = true;
    }

    private void ExitWithMessage(string message)
    {
        Console.WriteLine(message);
        ShowMessage(message);
        Environment.Exit(0);
    }

    private void ShowMessage(string message)
    {
        MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void HandleError(Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        ShowMessage($"An error occurred: {ex.Message}");
        Environment.Exit(-1);
    }
}
