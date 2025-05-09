namespace Chess.Pieces;
using Raylib_cs;
public class Pawn : IPiece  {

    public Image image;
    public Texture2D texture;

    public Pawn (playerColor newColor, (char,int) position) {
        color = newColor;
        (char x, int y) = position;
        Position = new ChessPosition(x,y);
        
        switch (this.color) {
            case playerColor.White:
                image = Raylib.LoadImage("assets/pawn_white.png");
                break;
            case playerColor.Black:
                image = Raylib.LoadImage("assets/pawn_black.png");
                break;
        }
        Raylib.ImageResize(ref image, Consts.TILE_SIZE, Consts.TILE_SIZE);
        texture = Raylib.LoadTextureFromImage(image);  
        Raylib.UnloadImage(image);
    }
    
    public bool Alive { get; set; } = true;
    public bool Select { get; set; } = false;

    public ChessPosition Position { get; set; }

    public playerColor color { get; }

    public void draw() {
        if (Alive) {
            (int posX, int posY) = Position.numPos;
            if (Select) {
                Raylib.DrawRectangle(posX*Consts.TILE_SIZE, posY*Consts.TILE_SIZE, Consts.TILE_SIZE, Consts.TILE_SIZE, Color.DarkGreen);
            }
            Raylib.DrawTexture(texture, posX *Consts.TILE_SIZE, posY* Consts.TILE_SIZE, Color.White);

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