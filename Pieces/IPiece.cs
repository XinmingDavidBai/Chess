namespace Chess.Pieces;

public interface IPiece {
    public bool Alive { get; set; }
    public bool Select { get; set; }
    public PlayerColor Color { get; }
    public string Label { get; internal set; }
    public PieceType Type { get; }
    public int Value { get; }
    public ChessPosition.ChessPosition Position { get; }
    public void setPosition(int x, int y);
    public void draw();
    public (int, int)[,] allMoves();
}