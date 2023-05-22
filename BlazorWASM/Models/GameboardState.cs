public class GameBoardState
{
    public BoardState Left { get; set; }
    public BoardState Right { get; set; }
    public BoardState Up { get; set; }
    public BoardState Down { get; set; }

    public GameBoardState()
    {
        Left = new BoardState();
        Right = new BoardState();
        Up = new BoardState();
        Down = new BoardState();
    }
}