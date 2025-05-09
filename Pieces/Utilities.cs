namespace Chess.Pieces;

public static class Utilities {
    public static void movePiece(IPiece piece, int x, int y) {
        piece.Position = new ChessPosition(ChessPositionTransformer.intToChar(x-1),y); //this is braindead
    }
}