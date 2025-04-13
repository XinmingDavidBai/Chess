namespace Chess.Pieces;

public class Empty : IPiece  {
    public ChessPosition position;

    public Empty ( (char,int) position) {
        (char x, int y) = position;
        this.position = new ChessPosition(x,y);
    }
    public void kill () {
    }
    public void draw() {

    }
    public void move(int x, int y) {
        position.setNewPos(x,y);
    }

    public playerColor pieceColor() {
        return playerColor.Empty;
    }

    public void setSelect() {
        
    }

    public bool getSelect() {
        return false;
    }

    public (int, int)[] allMoves() {
        return [];
    }
    public (int, int) getPosition() {
        throw new ArgumentNullException("");
    }
}