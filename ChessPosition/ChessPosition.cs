namespace Chess.ChessPosition;

public class ChessPosition(char x, int y) {
    public (char x, int y) ChessPos = (x, y);
    public (int x, int y) NumPos = (ChessPositionTransformer.charToInt(x), y);

    public void setNewPos(int x, int y) {
        NumPos = (x, y);
        ChessPos = (ChessPositionTransformer.intToChar(x - 1), y);
    }
}