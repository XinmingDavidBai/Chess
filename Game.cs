using System.Numerics;
using Raylib_cs;

namespace Chess;

class Game {
    private string name;
    public Game(string name) {
        this.name = name;
    }

    public void Run() {
        Raylib.InitWindow(800, 800, name);
        Board board = new Board();
        while (!Raylib.WindowShouldClose())
        {
            Vector2 mousePosition = Raylib.GetMousePosition();
            (int x, int y) = ((int)mousePosition.X / Consts.TILE_SIZE, (int)mousePosition.Y / Consts.TILE_SIZE);
            bool insideBoard = (x >= 1) && (x <= 8) && (y >= 1) && (y <= 8);
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && insideBoard ) {
                if (board.selectedPiece == null) {
                    board.select(x,y);
                } else {
                    board.move(x,y);
                }
            }
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);
            board.draw();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}