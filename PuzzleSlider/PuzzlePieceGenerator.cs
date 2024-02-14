using System;
using System.Drawing;
using System.Windows.Forms;

public class PuzzlePieceGenerator
{
    private const int PuzzleSize = 3; // Adjust the puzzle size as needed
    private const int PieceSize = 100; // Adjust the piece size as needed

    public void GeneratePuzzlePiece()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        PuzzleForm puzzleForm = new PuzzleForm();

        PuzzlePiece[,] puzzlePieces = GeneratePuzzlePieces();
        // Display or use the generated puzzle pieces as needed

        Application.Run(puzzleForm);
    }

    private PuzzlePiece[,] GeneratePuzzlePieces()
    {
        PuzzlePiece[,] puzzlePieces = new PuzzlePiece[PuzzleSize, PuzzleSize];
        int counter = 1;

        for (int i = 0; i < PuzzleSize; i++)
        {
            for (int j = 0; j < PuzzleSize; j++)
            {
                puzzlePieces[i, j] = GetDefaultImagePiece(counter++);
            }
        }

        return puzzlePieces;
    }

    // Generate a placeholder image for each piece with a number
    private PuzzlePiece GetDefaultImagePiece(int pieceNumber)
    {
        using (Bitmap piece = new Bitmap(PieceSize, PieceSize))
        {
            using (Graphics g = Graphics.FromImage(piece))
            {
                g.DrawString(pieceNumber.ToString(), new Font("Arial", 12), Brushes.Black, new PointF(20, 20));
            }

            // Create a copy of the image to avoid animation-related issues
            Bitmap copy = new Bitmap(piece);
            return new PuzzlePiece(pieceNumber, copy);
        }
    }
}

