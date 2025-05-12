namespace Chess.ChessPosition;

public static class ChessPositionTransformer {
    public static int charToInt(char x) {
        return x switch {
            'a' => 1,
            'b' => 2,
            'c' => 3,
            'd' => 4,
            'e' => 5,
            'f' => 6,
            'g' => 7,
            'h' => 8,
            _ => -1
        };
    }

    public static char intToChar(int x) {
        return x switch {
            0 => 'a',
            1 => 'b',
            2 => 'c',
            3 => 'd',
            4 => 'e',
            5 => 'f',
            6 => 'g',
            7 => 'h',
            _ => throw new ArgumentException("hell nah")
        };
    }
}