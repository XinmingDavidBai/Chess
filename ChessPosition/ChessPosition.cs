namespace Chess;
public class ChessPosition {
    public (char x, int y) chessPos;
    public (int x, int y) numPos;
    public ChessPosition(char x, int y) {
        chessPos = (x,y);
        numPos = (ChessPositionTransformer.charToInt(x), y);
    }
    public void setNewPos(int x, int y) {
        numPos = (x, y);
        chessPos = (ChessPositionTransformer.intToChar(x-1), y);
    }
}