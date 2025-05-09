namespace Chess.Pieces;
using Raylib_cs;
public class Queen : IPiece  {

    public Image Image;
    public Texture2D Texture;

    public Queen (playerColor newColor, (char,int) position) {
        Color = newColor;
        (char x, int y) = position;
        Position = new ChessPosition(x,y);

        Label = this.Color switch
        {
            playerColor.White => "queen_white",
            playerColor.Black => "queen_black",
            _ => Label
        } ?? throw new InvalidOperationException();
        Image = Raylib.LoadImage($"assets/{Label}.png");
        Raylib.ImageResize(ref Image, Consts.TILE_SIZE, Consts.TILE_SIZE);
        Texture = Raylib.LoadTextureFromImage(Image);  
        Raylib.UnloadImage(Image);
    }
    
    public bool Alive { get; set; } = true;
    public bool Select { get; set; } = false;

    public PieceType Type { get; } = PieceType.Queen;
    public int Value { get; } = 9;
    public ChessPosition Position { get; private set; }
    public void setPosition(int x, int y)
    {
        Position = new ChessPosition(ChessPositionTransformer.intToChar(x - 1), y);
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
        (int, int)[,] moves = new (int,int)[8,7];
        (int x, int y) = Position.numPos;
 
        (int,int) [,] directions = new (int,int)[8, 7]
        {
            {(-1,-1), (-2,-2), (-3,-3), (-4,-4), (-5,-5), (-6,-6), (-7,-7)}, 
            {(1,1), (2,2), (3,3), (4,4), (5,5), (6,6), (7,7)}, 
            {(-1,1), (-2,2), (-3,3), (-4,4), (-5,5), (-6,6), (-7,7)}, 
            {(1,-1), (2,-2), (3,-3), (4,-4), (5,-5), (6,-6), (7,-7)}, 
            {(1,0), (2,0), (3,0), (4,0), (5,0), (6,0), (7,0)}, 
            {(-1,0), (-2,0), (-3,0), (-4,0), (-5,0), (-6,0), (-7,0)}, 
            {(0,1), (0,2), (0,3), (0,4), (0,5), (0,6), (0,7)}, 
            {(0,-1), (0,-2), (0,-3), (0,-4), (0,-5), (0,-6), (0,-7)}, 
        };
        int c = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                (int dx, int dy) = directions[i, j];
                int nx = x + dx, ny = y + dy;
                if (nx >= 1 && nx <= 8 && ny >= 1 && ny <= 8) {
                    moves[i,j] = (nx,ny);
                }
            }
        }
        return moves;
    }
}