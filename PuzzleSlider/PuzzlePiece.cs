using System;
using System.Drawing;

public class PuzzlePiece
{
    public int Number { get; }
    public Image Image { get; }

    public PuzzlePiece(int number, Image image)
    {
        Number = number;
        Image = image;
    }
}


