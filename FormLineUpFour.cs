// Copyright Bergstreiser Software
// 
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BSS.LineUpFour
{
    public partial class FormLineUpFour : Form
    {
        const int NUMBER_OF_ROWS = 6;
        const int NUMBER_OF_COLUMNS = 7;
        const int NUMBER_TO_WIN = 4;
        const int CELL_FACTOR = 256;

        readonly sbyte[,] _field = new sbyte[NUMBER_OF_COLUMNS, NUMBER_OF_ROWS];
        readonly sbyte[] _level = new sbyte[NUMBER_OF_COLUMNS];
        sbyte _site;
        sbyte _winner;
        sbyte _deepMind;

        public FormLineUpFour()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            InitGame(1);
        }

        void InitGame(sbyte site)
        {
            for (var c = 0; c < NUMBER_OF_COLUMNS; ++c)
            {
                _level[c] = NUMBER_OF_ROWS;
                for (var r = 0; r < NUMBER_OF_ROWS; ++r)
                    _field[c, r] = 0;
            }
            _site = site;
            _winner = 0;
        }

        private void OnClickMIStart(object sender, EventArgs e)
        {
            this.InitGame(1);
            this.Invalidate();
        }

        private void OnClickMISwap(object sender, EventArgs e)
        {
            if (_winner != 0)
                return;
            this.MakeTurn(_site);
            _site *= -1;
        }

        private void OnClickMILevel(object sender, EventArgs e)
        {
            miLevel0.Checked = false;
            miLevel1.Checked = false;
            miLevel2.Checked = false;
            miLevel3.Checked = false;
            if (sender == miLevel0)
            {
                _deepMind = 0;
                miLevel0.Checked = true;
            }
            else if (sender == miLevel1)
            {
                _deepMind = 2;
                miLevel1.Checked = true;
            }
            else if (sender == miLevel2)
            {
                _deepMind = 4;
                miLevel2.Checked = true;
            }
            else
            {
                _deepMind = 6;
                miLevel3.Checked = true;
            }
        }


        void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _winner != 0)
                return;

            var cellSize = this.GetCellSize();
            var column = e.X / cellSize;

            if (column >= NUMBER_OF_COLUMNS)
                return;

            if (_field[column, 0] != 0)
                return;

            var row = _level[column];
            if (row == 0)
                return;
            row -= 1;
            _field[column, row] = _site;
            _level[column] = row;
            this.InvalidateCell(column, row);
            this.Refresh();
            if (CheckWinRow(column, row, _site))
            {
                _winner = _site;
                this.Invalidate();
            }
            else
            {
                this.MakeTurn((sbyte) -_site);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var cellSize = this.GetCellSize();
            var paddingSize = cellSize / 8;

            var brushes = new Brush[3];
            brushes[0] = Brushes.Gold;
            brushes[1] = Brushes.DarkSlateBlue;
            brushes[2] = Brushes.Tomato;
            // draw play field
            var r = this.menuStrip.Height;
            for (var i = 0; i < NUMBER_OF_ROWS; ++i)
            {
                // draw play field
                for (var j = 0; j < NUMBER_OF_COLUMNS; ++j)
                {
                    e.Graphics.FillEllipse(brushes[1 + _field[j, i]], cellSize * j + paddingSize, r + paddingSize, cellSize - paddingSize, cellSize - paddingSize);
                }
                r += cellSize;
            }
            // draw text
            if (_winner != 0)
            {
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                using (var brush = new HatchBrush(HatchStyle.Percent75, this.ForeColor))
                {
                    e.Graphics.DrawString(_winner == 2 ? "Drawn." : _winner == _site ? "Won!" : "Lost!", this.Font, brush, this.ClientRectangle, format);
                }
            }
        }

        private sbyte MakeTurn(sbyte site)
        {
            var guess = GuessMove(site, _deepMind);
            sbyte column = -1;
            if (guess == null)
            {
                _winner = 3;
                this.Invalidate();
            }
            else
            {
                column = (sbyte) guess.Item1;
                var row = _level[column];
                row -= 1;
                _field[column, row] = site;
                _level[column] = row;
                this.InvalidateCell(column, row);
                if (CheckWinRow(column, row, site))
                {
                    _winner = site;
                    this.Invalidate();
                }
                Debug.WriteLine(string.Format("Best move: {0}", guess.Item2));
            }
            return column;
        }

        int GetCellSize()
        {
            var clientSize = this.ClientSize;
            clientSize.Height -= this.menuStrip.Height;
            return Math.Max(20, Math.Min(clientSize.Height / NUMBER_OF_ROWS, clientSize.Width / NUMBER_OF_COLUMNS));
        }

        void InvalidateCell(int c, int r)
        {
            var cellSize = this.GetCellSize();
            var offset = this.menuStrip.Height;
            this.Invalidate(new Rectangle(c * cellSize, offset + r * cellSize, cellSize, cellSize));
        }



        Tuple<int, double> GuessMove(sbyte site, int maxDepth)
        {
            int bestGuessColumn = -1;
            double bestGuessValue = 0.0;

            int bestTurnValue = 0;
            int bestTurnGTValue = 0;
            int bestTurnSite = 0;
            for (var column = 0; column < NUMBER_OF_COLUMNS; ++column)
            {
                var row = _level[column];
                if (row == 0)
                    continue;
                row -= 1;
                var myValue = this.CountJoinedRows(column, row, site);
                var opValue = this.CountJoinedRows(column, row, (sbyte) -site);
                var opAdjValue = (opValue & ~(CELL_FACTOR - 1));
                // var cnt4opadj = cnt4op - 3;
                if (bestGuessColumn < 0 || bestTurnValue < myValue
                || (bestTurnValue == myValue && bestTurnGTValue < myValue + opValue))
                {
                    // Next check: this move don't offer better chanse for our opponent
                    if (myValue >= CELL_FACTOR * NUMBER_TO_WIN || row == 0 || this.CountJoinedRows(column, row - 1, (sbyte) -site) < CELL_FACTOR * NUMBER_TO_WIN)
                    {
                        bestGuessColumn = column;
                        bestTurnSite = site;
                        bestTurnValue = myValue;
                        bestTurnGTValue = myValue + opValue;
                        if (myValue >= CELL_FACTOR * NUMBER_TO_WIN)
                        {
                            maxDepth = 0;   // It's all clear.
                            break;
                        }
                    }
                }
                if (bestTurnValue < opAdjValue
                || (bestTurnValue == opAdjValue && bestTurnGTValue < myValue + opValue))
                {
                    bestGuessColumn = column;
                    bestTurnSite = -site;
                    bestTurnValue = opAdjValue;
                    bestTurnGTValue = myValue + opValue;
                }
            }
            // bestGuessValue = (site * bestSumJoinedColumns) / (double) (2 * CELL_FACTOR * NUMBER_TO_WIN);
            bestGuessValue = (bestTurnSite * bestTurnValue) / (double) (CELL_FACTOR * NUMBER_TO_WIN);
            if (--maxDepth >= 0)
            {
                int scenario = 0;
                double sumValue = 0.0;
                double worstValue = 0.0;
                for (var column = 0; column < NUMBER_OF_COLUMNS; ++column)
                {
                    var row = _level[column];
                    if (row == 0)
                        continue;
                    row -= 1;
                    _field[column, row] = site;
                    _level[column] = row;
                    var nextGuess = GuessMove((sbyte) -site, maxDepth);
                    _field[column, row] = 0;
                    row += 1;
                    _level[column] = row;
                    if (nextGuess != null)
                    {
                        sumValue += nextGuess.Item2;
                        if (scenario == 0 || site * worstValue.CompareTo(nextGuess.Item2) > 0)
                        {
                            worstValue = nextGuess.Item2;
                        }
                        if (scenario == 0 || site * bestGuessValue.CompareTo(nextGuess.Item2) < 0)
                        {
                            bestGuessValue = nextGuess.Item2;
                            bestGuessColumn = column;
                        }
                    }
                    scenario += 1;
                }
                if (scenario > 0)
                {
                    sumValue += worstValue;
                    bestGuessValue += sumValue / (2 * scenario + 2);
                }
            }
            if (bestGuessColumn < 0)
                return null;
            return new Tuple<int, double>(bestGuessColumn, bestGuessValue);
        }

        int CountJoinedRows(int column, int row, sbyte site)
        {
            // Direction |
            int maxCnt = 1 + CELL_FACTOR + CountJoinedRows(column, row, 0, +1, site);
            if (maxCnt >= CELL_FACTOR * NUMBER_TO_WIN)
                return maxCnt;
            maxCnt += row;
            if ((maxCnt & (CELL_FACTOR - 1)) < NUMBER_TO_WIN)
                maxCnt = 0;

            // Direction --
            int cnt1 = CountJoinedRows(column, row, +1, 0, site);
            if (cnt1 >= CELL_FACTOR * (NUMBER_TO_WIN - 1))
                return CELL_FACTOR + cnt1;
            int cnt2 = CountJoinedRows(column, row, -1, 0, site);
            if (cnt2 >= CELL_FACTOR * (NUMBER_TO_WIN - 1))
                return CELL_FACTOR + cnt2;
            int cntTotal = 1 + CELL_FACTOR + cnt1 + cnt2;
            if ((cntTotal & (CELL_FACTOR - 1)) >= NUMBER_TO_WIN && maxCnt < cntTotal)
                maxCnt = cntTotal;

            // Direction \ 
            cnt1 = CountJoinedRows(column, row, +1, +1, site);
            if (cnt1 >= CELL_FACTOR * (NUMBER_TO_WIN - 1))
                return CELL_FACTOR + cnt1;
            cnt2 = CountJoinedRows(column, row, -1, -1, site);
            if (cnt2 >= CELL_FACTOR * (NUMBER_TO_WIN - 1))
                return CELL_FACTOR + cnt2;
            cntTotal = 1 + CELL_FACTOR + cnt1 + cnt2;
            if ((cntTotal & (CELL_FACTOR - 1)) >= NUMBER_TO_WIN && maxCnt < cntTotal)
                maxCnt = cntTotal;

            // Direction /
            cnt1 = CountJoinedRows(column, row, +1, -1, site);
            if (cnt1 >= CELL_FACTOR * (NUMBER_TO_WIN - 1))
                return CELL_FACTOR + cnt1;
            cnt2 = CountJoinedRows(column, row, -1, +1, site);
            if (cnt2 >= CELL_FACTOR * (NUMBER_TO_WIN - 1))
                return CELL_FACTOR + cnt2;
            cntTotal = 1 + CELL_FACTOR + cnt1 + cnt2;
            if ((cntTotal & (CELL_FACTOR - 1)) >= NUMBER_TO_WIN && maxCnt < cntTotal)
                maxCnt = cntTotal;
            return maxCnt;
        }

        int CountJoinedRows(int column, int row, int dc, int dr, sbyte site)
        {
            int cntPossible = 0;
            int cntJoined = 0;
            bool gap = false;
            while (true)
            {
                row += dr;
                if (row < 0 || row >= NUMBER_OF_ROWS)
                    break;
                column += dc;
                if (column < 0 || column >= NUMBER_OF_COLUMNS)
                    break;
                var f = _field[column, row];
                if (f == -site)
                    break;
                cntPossible += 1;
                if (cntPossible == NUMBER_TO_WIN)
                    break;
                gap = gap || (f != site);
                if (!gap)
                    cntJoined += 1;
            }
            if (gap)
                cntPossible += CELL_FACTOR / 4;
            return cntJoined * CELL_FACTOR + cntPossible;
        }

        bool CheckWinRow(int column, int row, sbyte site)
        {
            // Direction |
            byte cnt = 1;
            for (var i = row + 1; i < NUMBER_OF_ROWS && _field[column, i] == site; ++i)
                if (++cnt >= NUMBER_TO_WIN)
                    return true;

            // Direction --
            cnt = 1;
            for (var i = column + 1; i < NUMBER_OF_COLUMNS && _field[i, row] == site; ++i)
                if (++cnt >= NUMBER_TO_WIN)
                    return true;
            for (var i = column - 1; i >= 0 && _field[i, row] == site; --i)
                if (++cnt >= NUMBER_TO_WIN)
                    return true;

            // Direction \ 
            cnt = 1;
            for (var i = 1; column + i < NUMBER_OF_COLUMNS && row + i < NUMBER_OF_ROWS && _field[column + i, row + i] == site; ++i)
                if (++cnt >= NUMBER_TO_WIN)
                    return true;
            for (var i = 1; column - i >= 0 && row - i >= 0 && _field[column - i, row - i] == site; ++i)
                if (++cnt >= NUMBER_TO_WIN)
                    return true;

            // Direction /
            cnt = 1;
            for (var i = 1; column + i < NUMBER_OF_COLUMNS && row - i >= 0 && _field[column + i, row - i] == site; ++i)
                if (++cnt >= NUMBER_TO_WIN)
                    return true;
            for (var i = 1; column - i >= 0 && row + i < NUMBER_OF_ROWS && _field[column - i, row + i] == site; ++i)
                if (++cnt >= NUMBER_TO_WIN)
                    return true;
            return false;
        }
    }
}
