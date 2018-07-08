using CodeGenHero.BingoBuzz.DTO.BB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz
{
    public static class Helpers
    {
        /// <summary>
        /// This method checks a bingo instance and selected content for Bingo.
        /// Possible return values:
        /// "Horizontal Bingo!",
        /// "Vertical Bingo!",
        /// "Diagonal Bingo top left to bottom right!",
        /// "Diagonal Bingo  bottom left to top right!",
        /// null (no bingo detected)
        /// </summary>
        /// <param name="bingoInstance">BingoInstance (DTO)</param>
        /// <param name="selectedBingoInstanceContent">List of BingoInstanceContent (DTO)</param>
        /// <returns></returns>
        public static string CheckForBingo(BingoInstance bingoInstance, List<BingoInstanceContent> selectedBingoInstanceContent)
        {
            //vertical bingo
            for (var i = 0; i < bingoInstance.NumberOfRows; i++)
            {
                //iterate through the rows
                //see if you have the same number of selected squares in the same row as you have columns, if so, you have horizontal bingo.
                if (selectedBingoInstanceContent.Where(x => x.Row == i).Count() == bingoInstance.NumberOfColumns)
                {
                    return "Horizontal Bingo!";
                }
            }

            //horizontal bingo
            for (var i = 0; i < bingoInstance.NumberOfColumns; i++)
            {
                //iterate through the columns
                //see if you have the same number of selected squares in the same column as you have rows, if so, you have vertical bingo.
                if (selectedBingoInstanceContent.Where(x => x.Col == i).Count() == bingoInstance.NumberOfRows)
                {
                    return "Vertical Bingo!";
                }
            }

            //this will only work if we have a square board...
            if (bingoInstance.NumberOfColumns == bingoInstance.NumberOfRows)
            {
                int countDiaSqs = 0;
                //diagonal bingo 0,0 to x,x - top left to bottom right
                for (var i = 0; i < bingoInstance.NumberOfColumns; i++)
                {
                    bool isAContender = true;
                    //iterate through the columns
                    //see if the selected squares have a row and column number that match
                    if (selectedBingoInstanceContent.Where(x => x.Row == i && x.Col == i).Any() && isAContender)
                    {
                        countDiaSqs++;
                    }
                    else
                    {
                        isAContender = false;
                    }

                    if (isAContender && countDiaSqs == bingoInstance.NumberOfColumns)
                    {
                        return "Diagonal Bingo top left to bottom right!";
                    }
                }
            }

            //this will only work if we have a square board...
            if (bingoInstance.NumberOfColumns == bingoInstance.NumberOfRows)
            {
                //diagonal bingo bottom left to top right
                //see if the selected squares have a row and col number that add up to the total number of cols -1
                if (selectedBingoInstanceContent.Where(x => x.Row + x.Col == (bingoInstance.NumberOfColumns - 1)).Count() == bingoInstance.NumberOfColumns)
                {
                    return "Diagonal Bingo  bottom left to top right!";
                }
            }
            return null;
        }
    }
}