using Chess.ChessPosition;
using Raylib_cs;

namespace Chess.Pieces;

public class Queen : IPiece {
    private readonly Texture2D _texture;

    public Queen(PlayerColor newColor, (char, int) position) {
        Color = newColor;
        var (x, y) = position;
        Position = new ChessPosition.ChessPosition(x, y);

        Label = Color switch {
            PlayerColor.White => "queen_white",
            PlayerColor.Black => "queen_black",
            _ => Label
        } ?? throw new InvalidOperationException();
        var image = Raylib.LoadImage($"assets/{Label}.png");
        Raylib.ImageResize(ref image, Consts.TILE_SIZE, Consts.TILE_SIZE);
        _texture = Raylib.LoadTextureFromImage(image);
        Raylib.UnloadImage(image);
    }

    public bool Alive { get; set; } = true;
    public bool Select { get; set; } = false;

    public PieceType Type { get; } = PieceType.Queen;
    public int Value { get; } = 9;
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
        var moves = new (int, int)[8, 7];
        var (x, y) = Position.NumPos;

        var directions = new (int, int)[8, 7] {
            { (-1, -1), (-2, -2), (-3, -3), (-4, -4), (-5, -5), (-6, -6), (-7, -7) },
            { (1, 1), (2, 2), (3, 3), (4, 4), (5, 5), (6, 6), (7, 7) },
            { (-1, 1), (-2, 2), (-3, 3), (-4, 4), (-5, 5), (-6, 6), (-7, 7) },
            { (1, -1), (2, -2), (3, -3), (4, -4), (5, -5), (6, -6), (7, -7) },
            { (1, 0), (2, 0), (3, 0), (4, 0), (5, 0), (6, 0), (7, 0) },
            { (-1, 0), (-2, 0), (-3, 0), (-4, 0), (-5, 0), (-6, 0), (-7, 0) },
            { (0, 1), (0, 2), (0, 3), (0, 4), (0, 5), (0, 6), (0, 7) },
            { (0, -1), (0, -2), (0, -3), (0, -4), (0, -5), (0, -6), (0, -7) }
        };
        for (var i = 0; i < 8; i++)
        for (var j = 0; j < 7; j++) {
            var (dx, dy) = directions[i, j];
            int nx = x + dx, ny = y + dy;
            if (nx is >= 1 and <= 8 && ny is >= 1 and <= 8) moves[i, j] = (nx, ny);
        }

        return moves;
    }
}