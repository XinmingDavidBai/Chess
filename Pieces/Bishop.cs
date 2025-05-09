namespace Chess.Pieces;
using Raylib_cs;
public class Bishop : IPiece  {

    public Image Image;
    public Texture2D Texture;

    public Bishop (playerColor newColor, (char,int) position) {
        Color = newColor;
        (char x, int y) = position;
        Position = new ChessPosition(x,y);

        Label = this.Color switch
        {
            playerColor.White => "bishop_white",
            playerColor.Black => "bishop_black",
            _ => Label
        } ?? throw new InvalidOperationException();
        Image = Raylib.LoadImage($"assets/{Label}.png");
        Raylib.ImageResize(ref Image, Consts.TILE_SIZE, Consts.TILE_SIZE);
        Texture = Raylib.LoadTextureFromImage(Image);  
        Raylib.UnloadImage(Image);
    }
    
    public bool Alive { get; set; } = true;
    public bool Select { get; set; } = false;

    public PieceType Type { get; } = PieceType.Bishop;
    public int Value { get; } = 3;
    public ChessPosition Position { get; set; }

    public playerColor Color { get; }
    public string Label { get; set; }

    public void draw() {
        if (Alive) {
            (int posX, int posY) = Position.numPos;
            if (Select) {
                Raylib.DrawRectangle(posX*Consts.TILE_SIZE, posY*Consts.TILE_SIZE, Consts.TILE_SIZE, Consts.TILE_SIZE, Raylib_cs.Color.DarkGreen);
            }
            Raylib.DrawTexture(Texture, posX *Consts.TILE_SIZE, posY* Consts.TILE_SIZE, Raylib_cs.Color.White);

        } 
    }

    public (int, int)[] allMoves() {
        if (!Alive) return [];
        (int, int)[] moves = new (int,int)[64];
        (int x, int y) = Position.numPos;
        (int, int)[] directions = {
                    (-1, -1), (0, -1), (1, -1),
                    (-1,  0),         (1,  0),
                    (-1,  1), (0,  1), (1,  1),
        };
        int c = 0;
        foreach (var (dx, dy) in directions) {
            int nx = x + dx, ny = y + dy;
            if (nx >= 1 && nx <= 8 && ny >= 1 && ny <= 8) {
                moves[c] = (nx,ny);
                c++;
            }
            
        }
        return moves;
    }
}