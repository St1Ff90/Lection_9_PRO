using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleLibrary
{
    public class Board
    {
        private const int DefaultSize = 10;
        public int Size { get; }
        public Cell[,] Cells { get; private set; }

        public Board(int size = DefaultSize)
        {
            Size = size;
            InitializeCells();
        }

        private void InitializeCells()
        {
            Cells = new Cell[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Cells[i, j] = new Cell();
                }
            }
        }

        internal bool IsValidPlacement(
            ShipPlacementDetails shipPlacementDetails)
        {
            int xShift = shipPlacementDetails.IsHorizontal ? 1 : 0;
            int yShift = shipPlacementDetails.IsHorizontal ? 0 : 1;
            bool result = true;
            for (int i = 0; i < shipPlacementDetails.Size; i++)
            {
                int x = shipPlacementDetails.PlacementCoordinate.X + xShift * i;
                int y = shipPlacementDetails.PlacementCoordinate.Y + yShift * i;

                if (x > Size - 1 || y > Size - 1)
                {
                    result = false;
                    break;
                }

                if (i == 0)
                {
                    if (Cells[x, y].Ship != null)
                    {
                        result = false;
                        break;
                    }
                }

                if (x < Size - 1 && Cells[x + 1, y].Ship != null)
                {
                    result = false;
                    break;
                }

                if (y < Size - 1 && Cells[x, y + 1].Ship != null)
                {
                    result = false;
                    break;
                }

                if (x < Size - 1 && y < Size - 1 && Cells[x + 1, y + 1].Ship != null)
                {
                    result = false;
                    break;
                }

                if (x < Size - 1 && y > 0 && Cells[x + 1, y - 1].Ship != null)
                {
                    result = false;
                    break;
                }

                if (x > 0 && y < Size - 1 && Cells[x - 1, y + 1].Ship != null)
                {
                    result = false;
                    break;
                }

                if (x > 0 && y > 0 && Cells[x - 1, y - 1].Ship != null)
                {
                    result = false;
                    break;
                }

                if (shipPlacementDetails.IsHorizontal && i == 0 || !shipPlacementDetails.IsHorizontal)
                {
                    if (x > 0 && Cells[x - 1, y].Ship != null)
                    {
                        result = false;
                        break;
                    }
                }

                if (!shipPlacementDetails.IsHorizontal && i == 0 || shipPlacementDetails.IsHorizontal)
                {
                    if (y > 0 && Cells[x, y - 1].Ship != null)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        internal void PlaceShip(ShipPlacementDetails shipPlacementDetails, Ship ship)
        {
            int xShift = shipPlacementDetails.IsHorizontal ? 1 : 0;
            int yShift = shipPlacementDetails.IsHorizontal ? 0 : 1;

            for (int i = 0; i < shipPlacementDetails.Size; i++)
            {
                int x = shipPlacementDetails.PlacementCoordinate.X + xShift * i;
                int y = shipPlacementDetails.PlacementCoordinate.Y + yShift * i;

                Cells[x, y].Ship = ship;
            }
        }

        internal ShotResult Shoot(Point shotCell)
        {
            if (
                shotCell.X >= Size ||
                shotCell.Y >= Size)
            {
                throw new ArgumentException();
            }

            var targetCell = Cells[shotCell.X, shotCell.Y];
            if (targetCell.IsShooted)
            {
                return ShotResult.Incorrect;
            }

            targetCell.IsShooted = true;
            return targetCell.Ship != null
                ? ShotResult.Damaged
                : ShotResult.Miss;
        }
    }
}
