using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Chess.Pieces;
using Raylib_cs;

namespace Chess;

public class Board {
    public IPiece[] pieces;
    public playerColor turn;
    public IPiece? selectedPiece;
    public (int,int)[] movablePositions;
    public (int, int)[] takeablePositions;

    public Board () {
        selectedPiece=null;
        turn = playerColor.White;
        pieces = new IPiece[32];
        int c = 0;
        for (int i = 0; i < Consts.INITIAL_PIECES_LOCATION.Length; i++) {
            for (int j = 0; j < Consts.INITIAL_PIECES_LOCATION[i].Length; j++) {
                playerColor col = i < 3 ? playerColor.Black : playerColor.White;
                switch (Consts.INITIAL_PIECES_LOCATION[i][j]) {
                    case 'k':
                        pieces[c] = new King(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    case 'q':
                        pieces[c] = new Queen(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    case 'b':
                        pieces[c] = new Bishop(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    case 'r':
                        pieces[c] = new Rook(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    case 'p':
                        pieces[c] = new Pawn(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    case 'n':
                        pieces[c] = new Knight(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    default:
                        break;
                }
            }
        } 
        movablePositions = [];
        takeablePositions = [];
    }
    public void draw() {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                if (Consts.BOARD[i][j] == 1) {
                    Raylib.DrawRectangle((i+1)*Consts.TILE_SIZE, (j+1)*Consts.TILE_SIZE, Consts.TILE_SIZE,Consts.TILE_SIZE, Color.Beige);
                } else {
                    Raylib.DrawRectangle((i+1)*Consts.TILE_SIZE, (j+1)*Consts.TILE_SIZE, Consts.TILE_SIZE,Consts.TILE_SIZE, Color.Brown);
                }
            }
        }
        foreach ((int, int) pos in takeablePositions) {
            (int x, int y) = pos;
            if (x == 0 && y == 0) break;
            Raylib.DrawRectangle((x)*Consts.TILE_SIZE, (y)*Consts.TILE_SIZE, Consts.TILE_SIZE,Consts.TILE_SIZE, Color.DarkBlue);
        }
        foreach (IPiece piece in pieces) {
            if (piece == null) break;
            piece.draw();
        }

        foreach ((int, int) pos in movablePositions) {
            (int x, int y) = pos;

            if (x == 0 && y == 0) break;
            Raylib.DrawCircle(x*Consts.TILE_SIZE + Consts.TILE_SIZE / 2, y*Consts.TILE_SIZE + Consts.TILE_SIZE / 2, Consts.TILE_SIZE / 8, Color.DarkGreen);
        }



    }
    private ((int, int)[], (int, int)[]) findMovableAndTakeablePositions((int,int)[] moves) {
        (int, int)[] movablePositions = new (int, int)[moves.Length];
        (int, int)[] takeablePositions = new (int, int)[moves.Length];
        int m = 0;
        int t = 0;
        foreach ((int, int) pos in moves) {
            bool placed = false;
            bool sameColor = false;
            foreach (IPiece piece in pieces) {
                if (piece == null || !piece.Alive) {
                    continue;
                }
                if (piece.Position.numPos == pos) {
                    if (piece.color != turn) {
                        takeablePositions[t] = pos;
                        t++;
                        placed = true;
                        break;
                    } else {
                        sameColor = true;
                        break;
                    }
                }
            }
            if (placed || sameColor) continue;
            movablePositions[m] = pos;
            m++;
        }
        return (movablePositions, takeablePositions);
    }  
    public void select(int x, int y) {
        IPiece? selectedPiece = null;
        foreach (IPiece piece in pieces) {
            if (piece == null) continue;
            if (piece.Position.numPos == (x,y)) {
                selectedPiece = piece;
                break;
            }
        }
        if (selectedPiece == null || selectedPiece.color != turn) return;

        this.selectedPiece = selectedPiece;
        
        (movablePositions, takeablePositions) = findMovableAndTakeablePositions(selectedPiece.allMoves());    

        selectedPiece.Select = !selectedPiece.Select;
    }
    private void unSelectPiece() {
        if (selectedPiece == null) return;
        selectedPiece.Select = !selectedPiece.Select;
        selectedPiece = null;
        movablePositions = [];
        takeablePositions = []; 
    }
    private void resetAndSwitch() {
        switchTurn();
        unSelectPiece();
    }
    public void move(int x, int y) {
        if (selectedPiece == null) return;

        if (selectedPiece.Position.numPos==(x,y)) {
            unSelectPiece();
            return;
        }
        bool available = false;
        foreach ((int,int) move in movablePositions) {
            if (move == (x,y)) {
                available = true;
            }
        }
        if (available) {
            Utilities.movePiece(selectedPiece,x,y);
            resetAndSwitch();
            return;
        }
        IPiece? takeablePiece = null;
        foreach ((int,int) move in takeablePositions) {
            if (move == (x,y)) {
                takeablePiece = Array.Find(pieces, p=>p.Position.numPos==(x,y));
            }
        }
        if (takeablePiece != null) {
            Utilities.movePiece(selectedPiece,x,y);
            takeablePiece.Alive = false;
            resetAndSwitch();
            return;
        }
    }

    public void switchTurn () {
        switch (turn) {
            case playerColor.White:
                turn = playerColor.Black;
                break;
            case playerColor.Black:
                turn = playerColor.White;
                break;
        }
    }
    
}