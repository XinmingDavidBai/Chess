using Raylib_cs;

namespace Chess;

internal class Game(string name)
{
    public void Run() {
        Raylib.InitWindow(800, 800, name);
        var board = new Board();
        while (!Raylib.WindowShouldClose()) {
            if (board.GameState is GameState.BlackWinner or GameState.WhiteWinner) {
                Console.WriteLine("game is ended");
            }
            var mousePosition = Raylib.GetMousePosition();
            var (x, y) = ((int)mousePosition.X / Consts.TILE_SIZE, (int)mousePosition.Y / Consts.TILE_SIZE);
            var insideBoard = x >= 1 && x <= 8 && y >= 1 && y <= 8;
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && insideBoard) {
                if (board.SelectedPiece == null)
                    board.select(x, y);
                else
                    board.move(x, y);
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);
            board.draw();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}