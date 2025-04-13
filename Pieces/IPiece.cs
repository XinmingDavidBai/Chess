namespace Chess.Pieces;

public interface IPiece {
    void move(int x, int y);
    void draw();
    void kill();
    void setSelect();
    bool getSelect();
    playerColor pieceColor();
    (int,int)[] allMoves();
    (int, int) getPosition();
    
}