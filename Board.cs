using Chess.ChessPosition;
using Chess.Pieces;
using Raylib_cs;

namespace Chess;

public class Board {
    private readonly IPiece[] _pieces;
    private PlayerColor _turn;
    private List<IPiece> _checkedPieces;
    private readonly Dictionary<IPiece, List<((int, int), IPiece?)>> _allMovesWhite;
    private readonly Dictionary<IPiece, List<((int, int), IPiece?)>> _allMovesBlack;
    private readonly Dictionary<IPiece, List<((int, int), IPiece?)>> _allPossibleMoves;
    
    public IPiece? SelectedPiece;
    public GameState GameState;
    
    public Board() {
        _checkedPieces = [];
        SelectedPiece = null;
        _turn = PlayerColor.White;
        _pieces = new IPiece[32];
        
        _allMovesWhite = new Dictionary<IPiece, List<((int, int), IPiece?)>>();
        _allMovesBlack = new Dictionary<IPiece, List<((int, int), IPiece?)>>();
        _allPossibleMoves = new Dictionary<IPiece, List<((int, int), IPiece?)>>();
        
        GameState = GameState.Ongoing;
        var c = 0;
        for (var i = 0; i < Consts.INITIAL_PIECES_LOCATION.Length; i++)
            for (var j = 0; j < Consts.INITIAL_PIECES_LOCATION[i].Length; j++) {
            var col = i < 3 ? PlayerColor.Black : PlayerColor.White;
            switch (Consts.INITIAL_PIECES_LOCATION[i][j]) {
                case 'k':
                    _pieces[c] = new King(col, (ChessPositionTransformer.intToChar(j), i + 1));
                    break;
                case 'q':
                    _pieces[c] = new Queen(col, (ChessPositionTransformer.intToChar(j), i + 1));
                    break;
                case 'b':
                    _pieces[c] = new Bishop(col, (ChessPositionTransformer.intToChar(j), i + 1));
                    break;
                case 'r':
                    _pieces[c] = new Rook(col, (ChessPositionTransformer.intToChar(j), i + 1));
                    break;
                case 'p':
                    _pieces[c] = new Pawn(col, (ChessPositionTransformer.intToChar(j), i + 1));
                    break;
                case 'n':
                    _pieces[c] = new Knight(col, (ChessPositionTransformer.intToChar(j), i + 1));
                    break;
                default:
                    continue;
            }
            c++;
        }
        findAllMoves();
        findAllPossibleMoves();
    }

    public void draw() {
        for (var i = 0; i < 8; i++)
        for (var j = 0; j < 8; j++)
            if (Consts.BOARD[i][j] == 1)
                Raylib.DrawRectangle((i + 1) * Consts.TILE_SIZE, (j + 1) * Consts.TILE_SIZE, Consts.TILE_SIZE,
                    Consts.TILE_SIZE, Color.Beige);
            else
                Raylib.DrawRectangle((i + 1) * Consts.TILE_SIZE, (j + 1) * Consts.TILE_SIZE, Consts.TILE_SIZE,
                    Consts.TILE_SIZE, Color.Brown);

        if (SelectedPiece != null)
            foreach (var move in _allPossibleMoves[SelectedPiece]) {
                var ((x, y), piece) = move;
                if (x == 0 && y == 0) break;
                if (piece == null)
                    Raylib.DrawCircle(x * Consts.TILE_SIZE + Consts.TILE_SIZE / 2,
                        y * Consts.TILE_SIZE + Consts.TILE_SIZE / 2,
                        Consts.TILE_SIZE / 8, Color.DarkGreen);
                else
                    Raylib.DrawRectangle(x * Consts.TILE_SIZE, y * Consts.TILE_SIZE, Consts.TILE_SIZE, Consts.TILE_SIZE,
                        Color.DarkBlue);
            }

        foreach (var checkedPiece in _checkedPieces) {
            var (x, y) = checkedPiece.Position.NumPos;
            Raylib.DrawRectangle( x * Consts.TILE_SIZE, y * Consts.TILE_SIZE, Consts.TILE_SIZE, Consts.TILE_SIZE,
                Color.Red);
        }

        foreach (var piece in _pieces) {
            if (!piece.Alive) continue;
            piece.draw();
        }
    }

    private void findAllPossibleMoves() {
        _allPossibleMoves.Clear();
        foreach (var piece in _pieces) {
            if (piece.Color == _turn && piece.Alive) {
                _allPossibleMoves.Add(piece,(piece.Color == PlayerColor.White ? _allMovesWhite : _allMovesBlack)[piece].FindAll(m => {
                    ((int, int) move, IPiece? target) = m;
                    return doesMoveStopOrNotResultInCheck(piece, move, target);
                }));
            }
        }
    }

    public void select(int x, int y) {
        IPiece? selectedPiece = null;
        foreach (var piece in _pieces)
            if (piece.Position.NumPos == (x, y) && piece.Alive) {
                selectedPiece = piece;
                break;
            }

        if (selectedPiece == null || selectedPiece.Color != _turn) return;

        SelectedPiece = selectedPiece;
    }
    

    private void resetAndSwitch() {
        switchTurn();
        SelectedPiece = null;
    }

    public void move(int x, int y) {
        if (SelectedPiece == null) return;

        if (SelectedPiece.Position.NumPos == (x, y)) {
            SelectedPiece = null;
            return;
        }

        foreach (var move in _allPossibleMoves[SelectedPiece]) {
            var (pos, piece) = move;
            if ((x, y) != pos) continue;
            if (piece == null) {
                SelectedPiece.setPosition(x, y);
                if (SelectedPiece.Type == PieceType.Pawn) (SelectedPiece as Pawn)!.FirstMove = false;
                resetAndSwitch();
                return;
            }

            SelectedPiece.setPosition(x, y);
            if (SelectedPiece.Type == PieceType.Pawn) (SelectedPiece as Pawn)!.FirstMove = false;

            piece.Alive = false;
            resetAndSwitch();
            return;
        }

        SelectedPiece = null; // if moved piece is not possible
    }

    private void switchTurn() {
        _turn = _turn switch {
            PlayerColor.White => PlayerColor.Black,
            PlayerColor.Black => PlayerColor.White,
            _ => _turn
        };
        findAllMoves();
        findAllPossibleMoves();
        _checkedPieces = checkChecker();
        checkGameState();
    }

    private List<((int,int), IPiece?)> findAllMovesForPiece(IPiece p) {
        List<((int,int), IPiece?)> allMoves = [];
        var moves = p.allMoves();
        for (var i = 0; i < moves.GetLength(0); i++) // each direction
        {
            var directionBlocked = false;
            for (var j = 0; j < moves.GetLength(1); j++) // each move in direction
            {
                (int x, int y) pos = moves[i, j];
                if (pos == (0, 0)) continue; // ignores the empty ones
                if (directionBlocked) break;     // goes to next direction
                foreach (var piece in _pieces)
                    if (piece.Position.NumPos == pos && piece.Alive) {
                        directionBlocked = true;
                        if (piece.Color == p.Color ||
                            (p.Type == PieceType.Pawn && i == 2)) // only pawn diag
                            break;
                        allMoves.Add((pos, piece));
                        break;
                    }

                // if there move is on empty cell
                if (p.Type == PieceType.Pawn && i is 0 or 1) continue; // not pawn diag 
                if (!directionBlocked) allMoves.Add((pos, null));
            }
        }

        return allMoves;
    }
    private void findAllMoves() {
        _allMovesBlack.Clear();
        _allMovesWhite.Clear();
        foreach (var p in _pieces) {
            var allMoves = findAllMovesForPiece(p);
            switch (p.Color) {
                case PlayerColor.White:
                    _allMovesWhite.Add(p,allMoves);
                    break;
                case PlayerColor.Black:
                    _allMovesBlack.Add(p,allMoves);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    private List<IPiece> checkChecker() {
        List<IPiece> checkPieces = [];
        foreach (var x in _allMovesWhite) {
            foreach (var v in x.Value) {
                ((int _, int _), IPiece? p) = v;
                if (p == null) continue;
                if (p.Type == PieceType.King) {
                    checkPieces.Add(p);
                }
            }
        }
        foreach (var x in _allMovesBlack) {
            foreach (var v in x.Value) {
                ((int _, int _), IPiece? p) = v;
                if (p == null) continue;
                if (p.Type == PieceType.King) {
                    checkPieces.Add(p);
                }
            }
        }
        
        return checkPieces;
    }

    private bool doesMoveStopOrNotResultInCheck(IPiece piece, (int, int) move, IPiece? targetPiece) {
        if (!piece.Alive) throw new ArgumentNullException();
        (int oldX, int oldY) = piece.Position.NumPos;
        foreach (var tempPiece in _pieces) {
            if (tempPiece.Position != piece.Position && tempPiece.Alive) continue;
            
            var (x, y) = move;
            if (targetPiece != null) targetPiece.Alive = false;
            tempPiece.setPosition(x,y);
            findAllMoves(); // terrible
            var retVal = checkChecker().Find(p => p.Color == tempPiece.Color) == null;
            if (targetPiece != null) targetPiece.Alive = true;
            tempPiece.setPosition(oldX, oldY);
            findAllMoves();
            return retVal;
        }
        return false;
    }

    private Dictionary<IPiece, List<((int, int), IPiece?)>> currentMoveSet() {
        switch (_turn)  {
            case PlayerColor.White:
                return _allMovesWhite;
            case PlayerColor.Black:
                return _allMovesBlack;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
 
    private void checkGameState() {
        bool possibleMoveCountIsZero = true;
        foreach (var x in _allPossibleMoves) {
            var v = x.Value;
            if (v.Count <= 0) continue;
            possibleMoveCountIsZero = false;
            break;

        }

        if (possibleMoveCountIsZero) {
            if (_checkedPieces.Count != 0) { //checkmate
                switch (_turn) {
                    case PlayerColor.White:
                        GameState = GameState.BlackWinner;
                        return;
                    case PlayerColor.Black:
                        GameState = GameState.WhiteWinner;
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }  //draw
            GameState = GameState.Draw;
        } else { //ongoing
            GameState = GameState.Ongoing;
        }
        
    }
}
