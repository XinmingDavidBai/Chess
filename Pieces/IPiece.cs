namespace Chess.Pieces;

public interface IPiece {
    public bool Alive { get; set; }
    public bool Select { get; set; }
    public playerColor color { get; }
    public ChessPosition Position { get; set; }
    public void draw();
    public (int,int)[] allMoves();
    
}