using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleLibrary
{
    public class GameEngine
    {
        public Board _leftBoard;
        private Board _rightBoard;
        public bool LeftPlayerTurn { get; private set; }
        public delegate void PropertyValueChanged();

        public GameEngine()
        {
            _leftBoard = new Board();
            _rightBoard = new Board();
            LeftPlayerTurn = true;
        }

        public bool PlaceShip(bool leftBoard,
            ShipPlacementDetails shipPlacementDetails)
        {
            Board target = GetBoard(leftBoard);

            bool result = false;
            if (target.IsValidPlacement(shipPlacementDetails))
            {
                Ship ship = GenerateShip(shipPlacementDetails);
                target.PlaceShip(shipPlacementDetails, ship);
                result = true;
            }

            return result;
        }

        private Ship GenerateShip(ShipPlacementDetails shipPlacementDetails)
        {
            Cell[] cells = new Cell[shipPlacementDetails.Size];

            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new Cell();
            }

            Ship ship = new Ship(cells);

            return ship;
        }

        private Board GetBoard(bool leftBoard)
            => leftBoard ? _leftBoard : _rightBoard;

        public ShotResult Shoot(Point shotCell)
        {
            Board target = GetBoard(LeftPlayerTurn);
            ShotResult shotResult = target.Shoot(shotCell);

            if (shotResult == ShotResult.Miss)
            {
                LeftPlayerTurn = !LeftPlayerTurn;
            }

            return shotResult;
        }


    }
}
