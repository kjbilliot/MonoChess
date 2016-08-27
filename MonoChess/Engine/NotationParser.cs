using MonoChess.Engine.Chess;
using MonoChess.Engine.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine
{
    public class NotationParser
    {
        private enum CommandType { Placement, Movement, Capture, Invalid }

        private const string PiecePlacementPattern = @"[KQNRBP][LD][A-H][1-8]";
        private const string PieceMovementPattern = @"[A-H][1-8]\s[A-H][1-8]";
        private const string PieceCapturePattern = PieceMovementPattern + "\\*";
        private const string InvalidMoveText = "That is not valid movement.";

        private const int pieceData_Type = 0;
        private const int pieceData_Material = 1;
        private const int pieceData_BoardX = 2;
        private const int pieceData_BoardY = 3;

        private const int LowAsciiCharIndex = 65;
        private const int LowAsciiNumIndex = 48;

        public static void ParseString(string command)
        {
            CommandType cmdType = GetCommandType(command);
            string english = "";

            if (cmdType != CommandType.Invalid)
            {
                switch (cmdType)
                {
                    case CommandType.Placement:
                        english = ExecuteAndTranslatePlacement(command);
                        break;
                    case CommandType.Movement:
                        english = ExecuteAndTranslateMovement(command);
                        break;
                    case CommandType.Capture:
                        english = ExecuteAndTranslateCapture(command);
                        break;
                }
                //ChessGame.ChessBoard.WriteBoard();
            }
            else
            {
                Console.WriteLine("Unknown command [" + command + "]\n\tOnly one command can be given per line.");
            }

            Console.WriteLine("[" + command + "] -> " + english);
        }

        private static string ExecuteAndTranslatePlacement(string command)
        {
            string translated = "";

            // Get data from the command.
            char pieceType     = command[pieceData_Type];
            char pieceMaterialId = command[pieceData_Material];
            char pieceBoardX   = command[pieceData_BoardX];
            char pieceBoardY   = command[pieceData_BoardY];

            // Convert chars to ints and store in a Vector2D
            int boardX = pieceBoardX.ToString().ToUpper()[0] - LowAsciiCharIndex;
            int boardY = (pieceBoardY.ToString().ToUpper()[0] - (LowAsciiNumIndex + 1)).Inverted();
            Vector2D position = new Vector2D(boardX, boardY);

            // Get PieceMaterial from char.
            PieceMaterial pieceMaterial = GetPieceMaterialFromChar(pieceMaterialId);

            // Get an instance of the correct piece from the data provided.
            ChessPiece pieceToBePlaced = GetNewPieceFromChar(pieceType, pieceMaterial, position);

            // Actually place the piece if the slot is free.
            var gameBoard = ChessGame.ChessBoard;

            if (gameBoard.IsSpotFree(pieceToBePlaced.BoardPosition.X, pieceToBePlaced.BoardPosition.Y))
            {
                // Actually place the piece on the board.
                gameBoard.Board[pieceToBePlaced.BoardPosition.Y, pieceToBePlaced.BoardPosition.X] = pieceToBePlaced;

                // Make it English now.
                translated += "Placed a " + (pieceToBePlaced.PieceType == PieceMaterial.L ? "white " : "black ");
                translated += pieceToBePlaced.GetType().Name + 
                    " at BoardPosition (" + pieceToBePlaced.BoardPosition.X + ", " + pieceToBePlaced.BoardPosition.Y + ")";
            }
            else
            {
                Console.WriteLine("Can't place piece. Position is not empty!");
            }

            return translated;
        }

        private static PieceMaterial GetPieceMaterialFromChar(char pieceMaterial)
        {
            PieceMaterial p = PieceMaterial.D;

            if (pieceMaterial.ToString().ToUpper() == "L")
            {
                p = PieceMaterial.L;
            }

            return p;
        }

        private static ChessPiece GetNewPieceFromChar(char type, PieceMaterial material, Vector2D position)
        {
            ChessPiece piece = null;
            switch (type.ToString().ToUpper())
            {
                case "Q":
                    piece = new Queen(position, material);
                    break;
                case "K":
                    piece = new King(position, material);
                    break;
                case "P":
                    piece = new Pawn(position, material);
                    break;
                case "N":
                    piece = new Knight(position, material);
                    break;
                case "B":
                    piece = new Bishop(position, material);
                    break;
                case "R":
                    piece = new Rook(position, material);
                    break;
                default:
                    break;
            }
            return piece;
        }

        private static string ExecuteAndTranslateMovement(string command)
        {
            // If the move is invalid, nothing happens and this just happens.
            string translated = InvalidMoveText;

            string[] positions = command.Split(' ');

            int fromX = positions[0][0].ToString().ToUpper()[0] - LowAsciiCharIndex;
            int fromY = (positions[0][1].ToString().ToUpper()[0] - (LowAsciiNumIndex + 1)).Inverted();
            int toX = positions[1][0].ToString().ToUpper()[0] - LowAsciiCharIndex;
            int toY = (positions[1][1].ToString().ToUpper()[0] - (LowAsciiNumIndex + 1)).Inverted();

            Vector2D fromPosition = new Vector2D(fromX, fromY);
            Vector2D toPosition = new Vector2D(toX, toY);

            var chessBoard = ChessGame.ChessBoard.Board;

            ChessPiece pieceAtFromPosition = chessBoard[fromPosition.Y, fromPosition.X];

            // We aren't moving a non-existant piece.
            if (pieceAtFromPosition != null && pieceAtFromPosition.IsCorrectTurn())
            {
                ChessPiece pieceAtToPosition = chessBoard[toPosition.Y, toPosition.X];
                //if (!((pieceAtFromPosition.PieceType == PieceMaterial.L && GameMaster.IsLightCheck)
                //    || (pieceAtFromPosition.PieceType == PieceMaterial.D && GameMaster.IsDarkCheck)))
                //{
                    // If the space we're moving to is empty, and it's a valid position.
                    if (pieceAtToPosition == null && pieceAtFromPosition.IsValidMove(toPosition))
                    {
                        string fromPieceName = pieceAtFromPosition.GetType().Name;
                        translated = "Move the " + fromPieceName + " at " + positions[0] + " to " + positions[1];
                        chessBoard[toPosition.Y, toPosition.X] = chessBoard[fromPosition.Y, fromPosition.X];
                        chessBoard[fromPosition.Y, fromPosition.X] = null;
                        pieceAtFromPosition.BoardPosition = toPosition;
                        pieceAtFromPosition.IsFirstMove = false;
                        GameMaster.AdvanceGame();

                    }
                //}
                //else
                //{
                //    translated = "Can't move piece.";
                //}
            }

            return translated;
        }
        private static string ExecuteAndTranslateCapture(string command)
        {
            string translated = InvalidMoveText;

            string[] positions = command.Split(' ');

            int fromX = positions[0][0].ToString().ToUpper()[0] - LowAsciiCharIndex;
            int fromY = (positions[0][1].ToString().ToUpper()[0] - (LowAsciiNumIndex + 1)).Inverted();
            int toX = positions[1][0].ToString().ToUpper()[0] - LowAsciiCharIndex;
            int toY = (positions[1][1].ToString().ToUpper()[0] - (LowAsciiNumIndex + 1)).Inverted();

            Vector2D fromPosition = new Vector2D(fromX, fromY);
            Vector2D toPosition = new Vector2D(toX, toY);

            var chessBoard = ChessGame.ChessBoard.Board;

            ChessPiece pieceAtFromPosition = chessBoard[fromPosition.Y, fromPosition.X];

            // We aren't moving a non-existant piece.
            if (pieceAtFromPosition != null && pieceAtFromPosition.IsCorrectTurn())
            {
                ChessPiece pieceAtToPosition = chessBoard[toPosition.Y, toPosition.X];

                // If the space we're moving to isn't empty, and it's a valid position, and it's a different color.
                if (pieceAtToPosition != null && pieceAtFromPosition.IsValidMove(toPosition) 
                    && (MovementValidation.PiecesAreSeparateTeams(fromPosition, toPosition)))
                {
                    string fromPieceName = pieceAtFromPosition.GetType().Name;
                    string toPieceName = pieceAtToPosition.GetType().Name;
                    translated = "Move the" + fromPieceName + " at " + positions[0] + " to " + positions[1] + " and capture the "
                        + toPieceName + " that is currently there.";
                    chessBoard[toPosition.Y, toPosition.X] = chessBoard[fromPosition.Y, fromPosition.X];
                    chessBoard[fromPosition.Y, fromPosition.X] = null;
                    pieceAtFromPosition.BoardPosition = toPosition;
                    pieceAtFromPosition.IsFirstMove = false;
                    GameMaster.AdvanceGame();

                }
                else
                {
                    translated = "Can not capture piece.";
                }
            }

            return translated;
        }

        private static CommandType GetCommandType(string command)
        {
            CommandType type = CommandType.Invalid;

            if (command.Matches(PiecePlacementPattern))
            {
                type = CommandType.Placement;
            }
            
            else if (command.Matches(PieceCapturePattern))
            {
                type = CommandType.Capture;
            }
            else if (command.Matches(PieceMovementPattern))
            {
                type = CommandType.Movement;
            }

            return type;
        }
    }
}
