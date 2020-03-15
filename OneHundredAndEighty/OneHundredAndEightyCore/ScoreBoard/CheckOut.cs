#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace OneHundredAndEightyCore.ScoreBoard
{
    public static class CheckOut
    {
        private static readonly string Space = "     ";

        private static readonly SortedList<int, string> ThreeThrows = new SortedList<int, string>()
                                                                      {
                                                                          [2] = "D1",
                                                                          [4] = "D2",
                                                                          [6] = "D3",
                                                                          [8] = "D4",
                                                                          [10] = "D5",
                                                                          [12] = "D6",
                                                                          [14] = "D7",
                                                                          [16] = "D8",
                                                                          [18] = "D9",
                                                                          [20] = "D10",
                                                                          [22] = "D11",
                                                                          [24] = "D12",
                                                                          [26] = "D13",
                                                                          [28] = "D14",
                                                                          [30] = "D15",
                                                                          [32] = "D16",
                                                                          [34] = "D17",
                                                                          [36] = "D18",
                                                                          [38] = "D19",
                                                                          [40] = "D20",
                                                                          [3] = $"1{Space}D1",
                                                                          [5] = $"1{Space}D2",
                                                                          [7] = $"3{Space}D2",
                                                                          [9] = $"1{Space}D4",
                                                                          [11] = $"3{Space}D4",
                                                                          [13] = $"5{Space}D4",
                                                                          [15] = $"7{Space}D4",
                                                                          [17] = $"9{Space}D4",
                                                                          [19] = $"3{Space}D8",
                                                                          [21] = $"5{Space}D8",
                                                                          [23] = $"7{Space}D8",
                                                                          [25] = $"1{Space}D12",
                                                                          [27] = $"3{Space}D12",
                                                                          [29] = $"5{Space}D12",
                                                                          [31] = $"7{Space}D12",
                                                                          [33] = $"1{Space}D16",
                                                                          [35] = $"3{Space}D16",
                                                                          [37] = $"5{Space}D16",
                                                                          [39] = $"7{Space}D6",
                                                                          [41] = $"9{Space}D16",
                                                                          [42] = $"10{Space}D16",
                                                                          [43] = $"3{Space}D20",
                                                                          [44] = $"4{Space}D20",
                                                                          [45] = $"13{Space}D16",
                                                                          [46] = $"6{Space}D20",
                                                                          [47] = $"7{Space}D20",
                                                                          [48] = $"16{Space}D16",
                                                                          [49] = $"17{Space}D16",
                                                                          [50] = $"18{Space}D16",
                                                                          [51] = $"19{Space}D16",
                                                                          [52] = $"20{Space}D16",
                                                                          [53] = $"13{Space}D20",
                                                                          [54] = $"14{Space}D20",
                                                                          [55] = $"15{Space}D20",
                                                                          [56] = $"16{Space}D20",
                                                                          [57] = $"17{Space}D20",
                                                                          [58] = $"18{Space}D20",
                                                                          [59] = $"19{Space}D20",
                                                                          [60] = $"20{Space}D20",
                                                                          [61] = $"T15{Space}D8",
                                                                          [62] = $"T10{Space}D16",
                                                                          [63] = $"T13{Space}D12",
                                                                          [64] = $"T16{Space}D8",
                                                                          [65] = $"T19{Space}D4",
                                                                          [66] = $"T14{Space}D12",
                                                                          [67] = $"T17{Space}D8",
                                                                          [68] = $"T20{Space}D4",
                                                                          [69] = $"T19{Space}D6",
                                                                          [70] = $"T18{Space}D8",
                                                                          [71] = $"T13{Space}16",
                                                                          [72] = $"T16{Space}D12",
                                                                          [73] = $"T19{Space}D8",
                                                                          [74] = $"T14{Space}D16",
                                                                          [75] = $"T17{Space}D12",
                                                                          [76] = $"T20{Space}D8",
                                                                          [77] = $"T19{Space}D10",
                                                                          [78] = $"T18{Space}D12",
                                                                          [79] = $"T19{Space}D11",
                                                                          [80] = $"T20{Space}D10",
                                                                          [81] = $"T19{Space}D12",
                                                                          [82] = $"Bull{Space}D16",
                                                                          [83] = $"T17{Space}D16",
                                                                          [84] = $"T20{Space}D12",
                                                                          [85] = $"T15{Space}D20",
                                                                          [86] = $"T18{Space}D18",
                                                                          [87] = $"T17{Space}D18",
                                                                          [88] = $"T20{Space}D14",
                                                                          [89] = $"T19{Space}D16",
                                                                          [90] = $"T20{Space}D15",
                                                                          [91] = $"T17{Space}D20",
                                                                          [92] = $"T20{Space}D16",
                                                                          [93] = $"T19{Space}D18",
                                                                          [94] = $"T18{Space}D20",
                                                                          [95] = $"T19{Space}D19",
                                                                          [96] = $"T20{Space}D18",
                                                                          [97] = $"T19{Space}D20",
                                                                          [98] = $"T20{Space}D19",
                                                                          [99] = $"T19{Space}10{Space}D16",
                                                                          [100] = $"T20{Space}D20",
                                                                          [101] = $"T20{Space}9{Space}D16",
                                                                          [102] = $"T16{Space}14{Space}D20",
                                                                          [103] = $"T19{Space}6{Space}D20",
                                                                          [104] = $"T16{Space}16{Space}D20",
                                                                          [105] = $"T20{Space}13{Space}D16",
                                                                          [106] = $"T20{Space}6{Space}D20",
                                                                          [107] = $"T19{Space}10{Space}D20",
                                                                          [108] = $"T20{Space}16{Space}D16",
                                                                          [109] = $"T20{Space}17{Space}D16",
                                                                          [110] = $"T20{Space}10{Space}D20",
                                                                          [111] = $"T19{Space}14{Space}D20",
                                                                          [112] = $"T20{Space}20{Space}D16",
                                                                          [113] = $"T19{Space}16{Space}D20",
                                                                          [114] = $"T20{Space}14{Space}D20",
                                                                          [115] = $"T20{Space}15{Space}D20",
                                                                          [116] = $"T20{Space}16{Space}D20",
                                                                          [117] = $"T20{Space}17{Space}D20",
                                                                          [118] = $"T20{Space}18{Space}D20",
                                                                          [119] = $"T19{Space}12{Space}Bull",
                                                                          [120] = $"T20{Space}20{Space}D20",
                                                                          [121] = $"T20{Space}11{Space}Bull",
                                                                          [122] = $"T18{Space}18{Space}Bull",
                                                                          [123] = $"T19{Space}16{Space}Bull",
                                                                          [124] = $"T20{Space}14{Space}Bull",
                                                                          [125] = $"25{Space}T20{Space}D20",
                                                                          [126] = $"T19{Space}19{Space}Bull",
                                                                          [127] = $"T20{Space}17{Space}Bull",
                                                                          [128] = $"18{Space}T20{Space}Bull",
                                                                          [129] = $"19{Space}T20{Space}Bull",
                                                                          [130] = $"T20{Space}20{Space}Bull",
                                                                          [131] = $"T20{Space}T13{Space}D16",
                                                                          [132] = $"25{Space}T19{Space}Bull",
                                                                          [133] = $"T20{Space}T19{Space}D8",
                                                                          [134] = $"T20{Space}T14{Space}D16",
                                                                          [135] = $"25{Space}T20{Space}Bull",
                                                                          [136] = $"T20{Space}T20{Space}D8",
                                                                          [137] = $"T20{Space}T19{Space}D10",
                                                                          [138] = $"T20{Space}T18{Space}D12",
                                                                          [139] = $"T19{Space}T14{Space}D20",
                                                                          [140] = $"T20{Space}T20{Space}D10",
                                                                          [141] = $"T20{Space}T19{Space}D12",
                                                                          [142] = $"T20{Space}T14{Space}D20",
                                                                          [143] = $"T20{Space}T17{Space}D16",
                                                                          [144] = $"T20{Space}T20{Space}D12",
                                                                          [145] = $"T20{Space}T15{Space}D20",
                                                                          [146] = $"T20{Space}T18{Space}D16",
                                                                          [147] = $"T20{Space}T17{Space}D18",
                                                                          [148] = $"T20{Space}T20{Space}D14",
                                                                          [149] = $"T20{Space}T19{Space}D16",
                                                                          [150] = $"T20{Space}T18{Space}D18",
                                                                          [151] = $"T20{Space}T17{Space}D20",
                                                                          [152] = $"T20{Space}T20{Space}D16",
                                                                          [153] = $"T20{Space}T19{Space}D18",
                                                                          [154] = $"T20{Space}T18{Space}D20",
                                                                          [155] = $"T20{Space}T19{Space}D19",
                                                                          [156] = $"T20{Space}T20{Space}D18",
                                                                          [157] = $"T20{Space}T19{Space}D20",
                                                                          [158] = $"T20{Space}T20{Space}D19",
                                                                          [160] = $"T20{Space}T20{Space}D20",
                                                                          [161] = $"T20{Space}T17{Space}Bull",
                                                                          [164] = $"T20{Space}T18{Space}Bull",
                                                                          [167] = $"T20{Space}T19{Space}Bull",
                                                                          [170] = $"T20{Space}T20{Space}Bull",
                                                                      };

        private static readonly SortedList<int, string> TwoThrows = new SortedList<int, string>()
                                                                    {
                                                                        [2] = "D1",
                                                                        [4] = "D2",
                                                                        [6] = "D3",
                                                                        [8] = "D4",
                                                                        [10] = "D5",
                                                                        [12] = "D6",
                                                                        [14] = "D7",
                                                                        [16] = "D8",
                                                                        [18] = "D9",
                                                                        [20] = "D10",
                                                                        [22] = "D11",
                                                                        [24] = "D12",
                                                                        [26] = "D13",
                                                                        [28] = "D14",
                                                                        [30] = "D15",
                                                                        [32] = "D16",
                                                                        [34] = "D17",
                                                                        [36] = "D18",
                                                                        [38] = "D19",
                                                                        [40] = "D20",
                                                                        [3] = $"1{Space}D1",
                                                                        [5] = $"1{Space}D2",
                                                                        [7] = $"3{Space}D2",
                                                                        [9] = $"1{Space}D4",
                                                                        [11] = $"3{Space}D4",
                                                                        [13] = $"5{Space}D4",
                                                                        [15] = $"7{Space}D4",
                                                                        [17] = $"9{Space}D4",
                                                                        [19] = $"3{Space}D8",
                                                                        [21] = $"5{Space}D8",
                                                                        [23] = $"7{Space}D8",
                                                                        [25] = $"1{Space}D12",
                                                                        [27] = $"3{Space}D12",
                                                                        [29] = $"5{Space}D12",
                                                                        [31] = $"7{Space}D12",
                                                                        [33] = $"1{Space}D16",
                                                                        [35] = $"3{Space}D16",
                                                                        [37] = $"5{Space}D16",
                                                                        [39] = $"7{Space}D6",
                                                                        [41] = $"9{Space}D16",
                                                                        [42] = $"10{Space}D16",
                                                                        [43] = $"3{Space}D20",
                                                                        [44] = $"4{Space}D20",
                                                                        [45] = $"13{Space}D16",
                                                                        [46] = $"6{Space}D20",
                                                                        [47] = $"7{Space}D20",
                                                                        [48] = $"16{Space}D16",
                                                                        [49] = $"17{Space}D16",
                                                                        [50] = $"18{Space}D16",
                                                                        [51] = $"19{Space}D16",
                                                                        [52] = $"20{Space}D16",
                                                                        [53] = $"13{Space}D20",
                                                                        [54] = $"14{Space}D20",
                                                                        [55] = $"15{Space}D20",
                                                                        [56] = $"16{Space}D20",
                                                                        [57] = $"17{Space}D20",
                                                                        [58] = $"18{Space}D20",
                                                                        [59] = $"19{Space}D20",
                                                                        [60] = $"20{Space}D20",
                                                                        [61] = $"T15{Space}D8",
                                                                        [62] = $"T10{Space}D16",
                                                                        [63] = $"T13{Space}D12",
                                                                        [64] = $"T16{Space}D8",
                                                                        [65] = $"T19{Space}D4",
                                                                        [66] = $"T14{Space}D12",
                                                                        [67] = $"T17{Space}D8",
                                                                        [68] = $"T20{Space}D4",
                                                                        [69] = $"T19{Space}D6",
                                                                        [70] = $"T18{Space}D8",
                                                                        [71] = $"T13{Space}16",
                                                                        [72] = $"T16{Space}D12",
                                                                        [73] = $"T19{Space}D8",
                                                                        [74] = $"T14{Space}D16",
                                                                        [75] = $"T17{Space}D12",
                                                                        [76] = $"T20{Space}D8",
                                                                        [77] = $"T19{Space}D10",
                                                                        [78] = $"T18{Space}D12",
                                                                        [79] = $"T19{Space}D11",
                                                                        [80] = $"T20{Space}D10",
                                                                        [81] = $"T19{Space}D12",
                                                                        [82] = $"Bull{Space}D16",
                                                                        [83] = $"T17{Space}D16",
                                                                        [84] = $"T20{Space}D12",
                                                                        [85] = $"T15{Space}D20",
                                                                        [86] = $"T18{Space}D18",
                                                                        [87] = $"T17{Space}D18",
                                                                        [88] = $"T20{Space}D14",
                                                                        [89] = $"T19{Space}D16",
                                                                        [90] = $"T20{Space}D15",
                                                                        [91] = $"T17{Space}D20",
                                                                        [92] = $"T20{Space}D16",
                                                                        [93] = $"T19{Space}D18",
                                                                        [94] = $"T18{Space}D20",
                                                                        [95] = $"T19{Space}D19",
                                                                        [96] = $"T20{Space}D18",
                                                                        [97] = $"T19{Space}D20",
                                                                        [98] = $"T20{Space}D19",
                                                                        [100] = $"T20{Space}D20",
                                                                        [101] = $"T17{Space}Bull",
                                                                        [104] = $"T18{Space}Bull",
                                                                        [107] = $"T19{Space}Bull",
                                                                        [110] = $"T20{Space}Bull"
                                                                    };

        private static readonly SortedList<int, string> OneThrow = new SortedList<int, string>()
                                                                   {
                                                                       [2] = "D1",
                                                                       [4] = "D2",
                                                                       [6] = "D3",
                                                                       [8] = "D4",
                                                                       [10] = "D5",
                                                                       [12] = "D6",
                                                                       [14] = "D7",
                                                                       [16] = "D8",
                                                                       [18] = "D9",
                                                                       [20] = "D10",
                                                                       [22] = "D11",
                                                                       [24] = "D12",
                                                                       [26] = "D13",
                                                                       [28] = "D14",
                                                                       [30] = "D15",
                                                                       [32] = "D16",
                                                                       [34] = "D17",
                                                                       [36] = "D18",
                                                                       [38] = "D19",
                                                                       [40] = "D20",
                                                                       [50] = "Bull"
                                                                   };

        public static string Get(int points, int throwNumber)
        {
            switch (throwNumber)
            {
                case 1:
                    if (OneThrow.ContainsKey(points))
                    {
                        return OneThrow[points];
                    }

                    if (TwoThrows.ContainsKey(points))
                    {
                        return TwoThrows[points];
                    }

                    if (ThreeThrows.ContainsKey(points))
                    {
                        return ThreeThrows[points];
                    }

                    break;
                case 2:
                    if (OneThrow.ContainsKey(points))
                    {
                        return OneThrow[points];
                    }

                    if (TwoThrows.ContainsKey(points))
                    {
                        return TwoThrows[points];
                    }

                    break;
                case 3:
                    if (OneThrow.ContainsKey(points))
                    {
                        return OneThrow[points];
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }
    }
}