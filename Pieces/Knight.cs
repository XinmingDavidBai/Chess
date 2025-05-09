namespace Chess.Pieces;
using Raylib_cs;
public class Knight : IPiece  {
    public playerColor color;
    public ChessPosition position;
    public bool alive;
    public bool selected;
    public Image image;
    public Texture2D texture;
    public Knight (playerColor color, (char,int) position) {
        selected=false;
        alive=true;
        this.color = color;
        (char x, int y) = position;
        this.position = new ChessPosition(x,y);
        
        switch (this.color) {
            case playerColor.White:
                image = Raylib.LoadImage("assets/knight_white.png");
                break;
            case playerColor.Black:
                image = Raylib.LoadImage("assets/knight_black.png");
                break;
        }
        Raylib.ImageResize(ref image, Consts.TILE_SIZE, Consts.TILE_SIZE);
        texture = Raylib.LoadTextureFromImage(image);  
        Raylib.UnloadImage(image);
    }
    public void kill () {
        alive=false;
    }
    public void draw() {
        if (alive) {
            (int posX, int posY) = position.numPos;
            if (selected) {
                Raylib.DrawRectangle(posX*Consts.TILE_SIZE, posY*Consts.TILE_SIZE, Consts.TILE_SIZE, Consts.TILE_SIZE, Color.DarkGreen);
            }
            Raylib.DrawTexture(texture, posX *Consts.TILE_SIZE, posY* Consts.TILE_SIZE, Color.White);

        } 
    }
    public void move(int x, int y) {
        if (!alive) return;
        position.setNewPos(x,y);
    }


    public playerColor pieceColor() {
        return color;
    }

    public void setSelect() {
        selected = !selected;
    }

    public bool getSelect() {
        return selected;
    }

    public (int, int)[] allMoves() {
        if (!alive) return [];
        (int, int)[] moves = new (int,int)[64];
        (int x, int y) = position.numPos;
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

    public (int, int) getPosition() {
        return position.numPos;
    }
}