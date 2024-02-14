using System;
using System.Windows.Forms;

partial class Program
{
    [STAThread]
    static void Main()
    {
        MainApplication();
    }

    static void MainApplication()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        PuzzleForm puzzleForm = new PuzzleForm();

        Application.Run(puzzleForm);
        PuzzlePieceGeneratorMethod();
    }



    static void PuzzlePieceGeneratorMethod()
    {
    
     
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        PuzzleForm puzzleForm = new PuzzleForm();

        Application.Run(puzzleForm);
    }
}
