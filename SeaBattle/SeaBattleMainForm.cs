using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SeaBattleLibrary;

namespace SeaBattle
{
    public partial class SeaBattleMainForm : Form
    {
        Random rand = new Random();
        GameEngine gameEngine = new GameEngine();
        public SeaBattleMainForm()
        {
            InitializeComponent();
            GenerateShips();
            for (int i = 0; i < 50; i++)
            {
                ShotResult shotResult = gameEngine.Shoot(new SeaBattleLibrary.Point(rand.Next(0, 10), rand.Next(0, 10)));
            }
        }

        public void GenerateShips()
        {
            ShipPlacementDetails[] shipPlacementDetails = new ShipPlacementDetails[10];
            int shipsLeft = 10;
            int currentShipSize = 4;
            int repeated = 0;

            while (shipsLeft > 0)
            {
                int x = rand.Next(0, 10);
                int y = rand.Next(0, 10);
                bool position = rand.NextDouble() >= 0.5;

                while (gameEngine.PlaceShip(false, new ShipPlacementDetails() { IsHorizontal = position, Size = currentShipSize, PlacementCoordinate = new SeaBattleLibrary.Point(x, y) }))
                {
                    shipPlacementDetails[shipsLeft - 1] = new ShipPlacementDetails() { IsHorizontal = position, Size = currentShipSize, PlacementCoordinate = new SeaBattleLibrary.Point(x, y) };

                    switch (currentShipSize)
                    {
                        case 4:
                            --currentShipSize;
                            break;
                        case 3:
                            if (++repeated >= 2)
                            {
                                --currentShipSize;
                                repeated = 0;
                            }
                            break;
                        case 2:
                            if (++repeated >= 3)
                            {
                                --currentShipSize;
                                repeated = 0;
                            }
                            break;
                        default:
                            break;
                    }

                    shipsLeft--;
                    break;
                }
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            DrawGameBoard();
        }

        private void DrawGameBoard()
        {
            var graphics = pictureBoxPlayerLeftZone.CreateGraphics();
            graphics.Clear(Color.White);
            int size = 10;
            int cellWidth = pictureBoxPlayerLeftZone.Width / size;
            int cellHeight = pictureBoxPlayerLeftZone.Height / size;
            int boardWidth = size * cellWidth;
            int boardHeight = size * cellHeight;
            int shift = 1;
            DrawBoard(
                graphics,
                size,
                cellWidth,
                cellHeight,
                boardWidth,
                boardHeight,
                shift);

            DrawCellsIndexes(graphics, size, cellWidth, cellHeight);
        }

        private static void DrawBoard(Graphics graphics, int size, int cellWidth, int cellHeight, int boardWidth, int boardHeight, int shift)
        {
            for (int i = 0; i <= size; i++)
            {
                graphics.DrawLine(
                    Pens.Red,
                    shift + i * cellWidth,
                    shift,
                    shift + i * cellWidth,
                    boardHeight);
                graphics.DrawLine(
                    Pens.Blue,
                    shift,
                    shift + i * cellHeight,
                    boardWidth,
                    shift + i * cellHeight);
            }
        }

        private static void DrawCellsIndexes(Graphics graphics, int size, int cellWidth, int cellHeight)
        {
            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            for (int i = 1; i <= size; i++)
            {
                for (int j = 1; j <= size; j++)
                {
                    var currentX = 1 + i * cellWidth - cellWidth / 4 * 3;
                    var currentY = 1 + j * cellHeight - cellHeight / 4 * 3;
                    graphics.DrawString($"{j - 1} {i - 1}", drawFont, drawBrush,
                        currentX, currentY);
                }
            }
        }

        private void SeaBattleMainForm_Paint(object sender, PaintEventArgs e)
        {
            //buttonStart_Click(sender, e);
        }
    }
}
