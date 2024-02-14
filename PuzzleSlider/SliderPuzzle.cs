using System.Drawing;
using System;

public class SliderPuzzle
{
    private PuzzlePiece[,] puzzle; // Represents the puzzle pieces
    private int emptyRow, emptyCol; // Represents the position of the empty space

    public int EmptyRow => emptyRow;
    public int EmptyCol => emptyCol;

    // Constructor 
    public SliderPuzzle(int size)
    {
        InitializePuzzle(size);
    }

    // Initialise the puzzle with default image pieces
    private void InitializePuzzle(int size)
    {
        puzzle = new PuzzlePiece[size, size];
        int counter = 1;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                puzzle[i, j] = GetDefaultImagePiece(counter++);
            }
        }

        puzzle[size - 1, size - 1] = null; // Set the last element as empty
        emptyRow = size - 1;
        emptyCol = size - 1;

        ShufflePuzzle();
    }

    // Shuffle the puzzle by making random moves
    private void ShufflePuzzle()
    {
        Random rand = new Random();
        for (int i = 0; i < 1000; i++)
        {
            int direction = rand.Next(4);
            MoveTile(emptyRow, emptyCol, direction);
        }
    }

    // Generate a placeholder image for each piece with a number
    private PuzzlePiece GetDefaultImagePiece(int pieceNumber)
    {
        using (Bitmap piece = new Bitmap(100, 100))
        {
            using (Graphics g = Graphics.FromImage(piece))
            {
                g.DrawString(pieceNumber.ToString(), new Font("Arial", 12), Brushes.Black, new PointF(20, 20));
            }
            return new PuzzlePiece(pieceNumber, piece);
        }
    }

    // Load an image from a URL, resize it, and divide it into pieces for the puzzle
    public void LoadImage(string imageUrl)
    {
        try
        {
            using (var webClient = new System.Net.WebClient())
            {
                byte[] data = webClient.DownloadData(imageUrl);
                using (var stream = new System.IO.MemoryStream(data))
                {
                    // Attempt to load the image
                    Image originalImage = Image.FromStream(stream);

                    // Check if the image is valid
                    if (originalImage.Width <= 0 || originalImage.Height <= 0)
                    {
                        Console.WriteLine("Invalid image dimensions.");
                        return;
                    }

                    // Resize the image to fit the puzzle size
                    Image resizedImage = ResizeImage(originalImage, 300, 300);

                    // Divide the resized image into pieces for the puzzle
                    puzzle = DivideImage(resizedImage, puzzle.GetLength(0));
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exception that occurred during image processing
            Console.WriteLine($"Error processing image: {ex.Message}");
            Console.WriteLine($"Exception details: {ex.StackTrace}");
            throw; 
        }

        // Shuffle the puzzle after loading the image
        ShufflePuzzle();
    }

    // Resize an image to a specified width and height
    private Image ResizeImage(Image image, int width, int height)
    {
        return new Bitmap(image, new Size(width, height));
    }

    // Divide an image into pieces based on the specified size
    private PuzzlePiece[,] DivideImage(Image image, int size)
    {
        PuzzlePiece[,] result = new PuzzlePiece[size, size];
        int pieceWidth = image.Width / size;
        int pieceHeight = image.Height / size;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                using (Bitmap piece = new Bitmap(pieceWidth, pieceHeight))
                {
                    using (Graphics g = Graphics.FromImage(piece))
                    {
                        g.DrawImage(image, new Rectangle(0, 0, pieceWidth, pieceHeight),
                            new Rectangle(j * pieceWidth, i * pieceHeight, pieceWidth, pieceHeight), GraphicsUnit.Pixel);
                    }

                    result[i, j] = new PuzzlePiece(i * size + j + 1, piece);
                }
            }
        }

        return result;
    }

    // Get the current state of the puzzle
    public PuzzlePiece[,] GetPuzzle()
    {
        return puzzle;
    }

    // Move a tile in the puzzle based on user input
    public void MoveTile(int row, int col, int direction)
    {
        // Check if the selected tile is adjacent to the empty space
        if (IsAdjacent(row, col, direction))
        {
            // Swap the selected tile with the empty space
            PuzzlePiece temp = puzzle[row, col];
            puzzle[row, col] = puzzle[emptyRow, emptyCol];
            puzzle[emptyRow, emptyCol] = temp;

            // Update the empty space position
            emptyRow = row;
            emptyCol = col;
        }
    }

    // Check if the puzzle is in the solved state
    public bool IsSolved()
    {
        int counter = 1;

        // Check if the puzzle is solved
        for (int i = 0; i < puzzle.GetLength(0); i++)
        {
            for (int j = 0; j < puzzle.GetLength(1); j++)
            {
                int expectedPieceNumber = counter % (puzzle.GetLength(0) * puzzle.GetLength(1));
                int actualPieceNumber = puzzle[i, j]?.Number ?? -1;

                if (actualPieceNumber != expectedPieceNumber)
                {
                    return false;
                }

                counter++;
            }
        }

        return true;
    }

    // Check if the selected tile is adjacent to the empty space
    private bool IsAdjacent(int row, int col, int direction)
    {
        switch (direction)
        {
            case 0: // Up
                return row == emptyRow + 1 && col == emptyCol;
            case 1: // Down
                return row == emptyRow - 1 && col == emptyCol;
            case 2: // Left
                return row == emptyRow && col == emptyCol + 1;
            case 3: // Right
                return row == emptyRow && col == emptyCol - 1;
            default:
                return false;
        }
    }
}