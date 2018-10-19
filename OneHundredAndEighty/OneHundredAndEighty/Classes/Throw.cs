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
        public bool IsFault { get; set; }   //  Был ли бросок штрафным
        public bool IsLegWon { get; set; }  //  Выигран ли броском лег
        public bool IsSetWon { get; set; }  //  Выигран ли броском сет
        public bool IsMatchWon { get; set; }    //  Выигран ли броском матч
        public int HandNumber { get; set; } //  Номер броска в подходе
        public string WhoThrow { get; set; }    //  Кто бросил 

        public Throw(object o)  //  Конструктор очередного броска
        {
            Shape S = o as Shape;   //  Берем сектор дартса
            this.Points = Int32.Parse(S.Tag.ToString());    //  Сохраняем очки броска

            string x = S.Name.Substring(1); //  Извлекаем очки сектора попадания
            string s = S.Name.Substring(0, 1);  //  Извлекаем мультипликаток сектора попадания
            switch (s)  //  По имени сектора дартса записываем мультипликатор
            {
                case "Z":
                    this.Multiplier = "Zero";
                    this.Sector = "Zero";
                    break;
                case "_":
                    this.Multiplier = "Single";
                    this.Sector = x;
                    break;
                case "D":
                    this.Multiplier = "Double";
                    this.Sector = new StringBuilder().Append("Double ").Append(x).ToString() ;
                    break;
                case "T":
                    this.Multiplier = "Tremble";
                    this.Sector = new StringBuilder().Append("Tremble ").Append(x).ToString() ;
                    break;
                case "B":
                    this.Multiplier = "Bull_25";
                    this.Sector = new StringBuilder().Append("25").ToString();
                    break;
                case "E":
                    this.Multiplier = "Bull_Eye";
                    this.Sector = new StringBuilder().Append("Bull Eye").ToString();
                    break;
            }
        }
    }
}
