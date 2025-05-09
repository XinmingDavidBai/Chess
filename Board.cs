using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Chess.Pieces;
using Raylib_cs;

namespace Chess;

public class Board {
    private IPiece[] _pieces;
    private playerColor _turn;
    public IPiece? SelectedPiece;
    private List<((int,int),IPiece?)>  _possibleMoves;

    public Board ()
    {
        _possibleMoves = new List<((int, int), IPiece?)>();
        SelectedPiece=null;
        _turn = playerColor.White;
        _pieces = new IPiece[32];
        int c = 0;
        for (int i = 0; i < Consts.INITIAL_PIECES_LOCATION.Length; i++) {
            for (int j = 0; j < Consts.INITIAL_PIECES_LOCATION[i].Length; j++) {
                playerColor col = i < 3 ? playerColor.Black : playerColor.White;
                switch (Consts.INITIAL_PIECES_LOCATION[i][j]) {
                    case 'k':
                        _pieces[c] = new King(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    case 'q':
                        _pieces[c] = new Queen(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    case 'b':
                        _pieces[c] = new Bishop(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    case 'r':
                        _pieces[c] = new Rook(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    case 'p':
                        _pieces[c] = new Pawn(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    case 'n':
                        _pieces[c] = new Knight(col, (ChessPositionTransformer.intToChar(j), i + 1));
                        c++;
                        break;
                    default:
                        break;
                }
            }
        } 
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

        foreach (((int, int), IPiece?) move in _possibleMoves)
        {
            ((int x, int y), IPiece? piece) = move;
            if (x == 0 && y == 0) break;
            if (piece == null)
            {
                Raylib.DrawCircle(x * Consts.TILE_SIZE + Consts.TILE_SIZE / 2,
                    y * Consts.TILE_SIZE + Consts.TILE_SIZE / 2,
                    Consts.TILE_SIZE / 8, Color.DarkGreen);
            }
            else
            {
                Raylib.DrawRectangle((x) * Consts.TILE_SIZE, (y) * Consts.TILE_SIZE, Consts.TILE_SIZE, Consts.TILE_SIZE,
                    Color.DarkBlue);
            }
        }
        foreach (IPiece piece in _pieces) {
            if (!piece.Alive) continue;
            piece.draw();
        }
        

    }
    private void findPossibleMoves ((int,int)[] moves)
    {
        if (SelectedPiece==null) return;
        foreach ((int, int) pos in moves)
        {
            bool moveTaken = false;
            foreach (IPiece piece in _pieces) {
                if (piece.Position.numPos == pos && piece.Alive)
                {
                    if (piece.Color == SelectedPiece.Color)
                    {
                        moveTaken = true;
                        break;
                    }
                    _possibleMoves.Add((pos, piece));
                    moveTaken = true;
                    break;
                }
            }

            if (!moveTaken)
            {
                _possibleMoves.Add((pos, null));
            }
        }
    }  
    public void select(int x, int y) {
        IPiece? selectedPiece = null;
        foreach (IPiece piece in _pieces) {
            if (piece.Position.numPos == (x,y) && piece.Alive) {
                selectedPiece = piece;
                break;
            }
        }
        if (selectedPiece == null || selectedPiece.Color != _turn) return;

        this.SelectedPiece = selectedPiece;
        
        findPossibleMoves(SelectedPiece.allMoves());

        selectedPiece.Select = !selectedPiece.Select;
    }
    private void unSelectPiece() {
        if (SelectedPiece == null) return;
        SelectedPiece.Select = !SelectedPiece.Select;
        SelectedPiece = null;
        resetPossibleMove();
    }
    private void resetAndSwitch() { switchTurn();
        unSelectPiece();
    }
    public void move(int x, int y) {
        if (SelectedPiece == null) return;
        
        if (SelectedPiece.Position.numPos==(x,y)) {
            unSelectPiece();
            return;
        }
        foreach (((int, int), IPiece?) move in _possibleMoves)
        {
            ((int, int) pos, IPiece? piece) = move;
            if ((x, y) == pos)
            {
                if (piece == null)
                {
                    Utilities.movePiece(SelectedPiece,x,y);
                    resetAndSwitch();
                    return;
                }
                else
                {
                    Utilities.movePiece(SelectedPiece,x,y);
                    piece.Alive = false;
                    resetAndSwitch();
                    return;
                }
            }
        } 
        unSelectPiece(); // if moved piece is not possible
    }

    private void switchTurn () {
        switch (_turn) {
            case playerColor.White: 
                _turn = playerColor.Black;
                break;
            case playerColor.Black:
                _turn = playerColor.White;
                break;
        }
    }

    private void resetPossibleMove()
    {
        _possibleMoves.Clear();
    }
}