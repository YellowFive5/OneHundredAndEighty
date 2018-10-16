using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace OneHundredAndEighty
{
    public class Throw  // Бросок
    {
        public string Sector { get; private set; }  //  Сектор попадания
        public string Multiplier { get; private set; }  //  Мультипликатор
        public int? Points { get; private set; } = null;    //  Очки броска
        public bool IsLegWon { get; set; } //  Выигран ли броском лег
        public bool IsSetWon { get; set; } //  Выигран ли броском сет
        public bool IsMatchWon { get; set; } //  Выигран ли броском матч
        public bool IsFault { get; set; } //  Был ли бросок штрафным
        public int HandNumber { get; set; } //  Номер броска в подходе


        public Throw(object o)  //  Конструктор очередного броска
        {
            Shape S = o as Shape;   //  Берем сектор дартса
            this.Points = Int32.Parse(S.Tag.ToString());    //  Сохнаряем очки броска

            string s = S.Name.Substring(0, 1);  //  Смотрим имя сектора дартса
            switch (s)  //  По имени сектора дартса записываем мультипликатор
            {
                case "Z":
                    this.Multiplier = "Zero";
                    break;
                case "_":
                    this.Multiplier = "Single";
                    break;
                case "D":
                    this.Multiplier = "Double";
                    break;
                case "T":
                    this.Multiplier = "Tremble";
                    break;
                case "B":
                    this.Multiplier = "Bull_25";
                    break;
                case "E":
                    this.Multiplier = "Bull_Eye";
                    break;
            }
            this.Sector = S.Name;   //  Сохранияем имя сектора

        }
    }
}
