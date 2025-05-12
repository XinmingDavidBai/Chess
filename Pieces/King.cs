using Chess.ChessPosition;
using Raylib_cs;

namespace Chess.Pieces;

public class King : IPiece {
    private readonly Texture2D _texture;
    public King(PlayerColor newColor, (char, int) position) {
        Color = newColor;
        var (x, y) = position;
        Position = new ChessPosition.ChessPosition(x, y);

        Label = Color switch {
            PlayerColor.White => "king_white",
            PlayerColor.Black => "king_black",
            _ => Label
        } ?? throw new InvalidOperationException();
        var image = Raylib.LoadImage($"assets/{Label}.png");
        Raylib.ImageResize(ref image, Consts.TILE_SIZE, Consts.TILE_SIZE);
        _texture = Raylib.LoadTextureFromImage(image);
        Raylib.UnloadImage(image);
    }

    public bool Alive { get; set; } = true;
    public bool Select { get; set; } = false;
    
    public PieceType Type { get; } = PieceType.King;
    public int Value { get; } = 0;
    public ChessPosition.ChessPosition Position { get; private set; }
    
    public void setPosition(int x, int y) {
        Position = new ChessPosition.ChessPosition(ChessPositionTransformer.intToChar(x - 1), y);
    }

    public PlayerColor Color { get; }
    public string Label { get; set; }
    
    public void draw() {
        if (Alive) {
            var (posX, posY) = Position.NumPos;
            if (Select)
                Raylib.DrawRectangle(posX * Consts.TILE_SIZE, posY * Consts.TILE_SIZE, Consts.TILE_SIZE,
                    Consts.TILE_SIZE, Raylib_cs.Color.DarkGreen);
            Raylib.DrawTexture(_texture, posX * Consts.TILE_SIZE, posY * Consts.TILE_SIZE, Raylib_cs.Color.White);
        }
    }

    public (int, int)[,] allMoves() {
        var moves = new (int, int)[8, 1];
        var (x, y) = Position.NumPos;
        (int, int)[] directions = {
            (-1, -1), (0, -1), (1, -1),
            (-1, 0),          (1, 0),
            (-1, 1), (0, 1), (1, 1)
        };
        var c = 0;
        foreach (var (dx, dy) in directions) {
            int nx = x + dx, ny = y + dy;
            if (nx >= 1 && nx <= 8 && ny >= 1 && ny <= 8) {
                moves[c, 0] = (nx, ny);
                c++;
            }
        }

        return moves;
    }
}