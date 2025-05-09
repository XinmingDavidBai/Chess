namespace Chess.Pieces;
using Raylib_cs;
public class Pawn : IPiece  {

    public Image Image;
    public Texture2D Texture;

    public bool FirstMove { get; set; } = true;
    public Pawn (playerColor newColor, (char,int) position) {
        Color = newColor;
        (char x, int y) = position;
        Position = new ChessPosition(x,y);

        Label = this.Color switch
        {
            playerColor.White => "pawn_white",
            playerColor.Black => "pawn_black",
            _ => Label
        } ?? throw new InvalidOperationException();
        Image = Raylib.LoadImage($"assets/{Label}.png");
        Raylib.ImageResize(ref Image, Consts.TILE_SIZE, Consts.TILE_SIZE);
        Texture = Raylib.LoadTextureFromImage(Image);  
        Raylib.UnloadImage(Image);
    }
    
    public bool Alive { get; set; } = true;
    public bool Select { get; set; } = false;

    public PieceType Type { get; } = PieceType.Pawn;
    public int Value { get; } = 1;
    public ChessPosition Position { get; private set; }

    public void setPosition(int x, int y)
    {
        Position = new ChessPosition(ChessPositionTransformer.intToChar(x - 1), y);
        FirstMove = false;
    } 
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

    public (int, int)[,] allMoves() {
        (int, int)[,] moves = new (int,int)[2,2];
        (int x, int y) = Position.numPos;
        (int, int)[] directions = new (int,int)[2];
            switch (Color)
            {
                case playerColor.White:
                    if (FirstMove)
                    {
                        (directions[0], directions[1]) = ( (0, -1),(0, -2));
                    }
                    else
                    {
                        directions[0]= (0, -1);
                    }
                    break;
                case playerColor.Black:
                    if (FirstMove)
                    {
                        (directions[0], directions[1]) = ( (0, 1),(0, 2));
                    }
                    else
                    {
                        directions[0]= (0, 1);
                    }
                    break;
            }
        
        int c = 0;
        foreach (var (dx, dy) in directions) {
            int nx = x + dx, ny = y + dy;
            if (nx >= 1 && nx <= 8 && ny >= 1 && ny <= 8) {
                moves[0,c] = (nx,ny);
                c++;
            }
        }
        return moves;
    }
}