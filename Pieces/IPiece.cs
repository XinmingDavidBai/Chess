namespace Chess.Pieces;

public interface IPiece {
    public bool Alive { get; set; }
    public bool Select { get; set; }
    public playerColor Color { get; }
    public string Label { get; internal set; }
    public PieceType Type { get; }
    public int Value { get; }
    public ChessPosition Position { get; set; }
    public void draw();
    public (int,int)[] allMoves();
    
}