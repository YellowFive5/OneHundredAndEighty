#region Usings

using System.Text;
using System.Windows.Shapes;

#endregion

namespace OneHundredAndEighty
{
    public class Throw // Бросок
    {
        private string Sector { get; } //  Сектор попадания
        public string Multiplier { get; } //  Мультипликатор
        public int? Points { get; } //  Очки броска
        public bool IsFault { get; set; } //  Был ли бросок штрафным
        public bool IsLegWon { get; set; } //  Выигран ли броском лег
        public bool IsSetWon { get; set; } //  Выигран ли броском сет
        public bool IsMatchWon { get; set; } //  Выигран ли броском матч
        public int HandNumber { get; set; } //  Номер броска в подходе
        public string WhoThrow { get; set; } //  Кто бросил 

        public Throw(object o) //  Конструктор очередного броска
        {
            var shape = o as Shape; //  Берем сектор дартса
            Points = int.Parse(shape.Tag.ToString()); //  Сохраняем очки броска

            var x = shape.Name.Substring(1); //  Извлекаем очки сектора попадания
            var s = shape.Name.Substring(0, 1); //  Извлекаем мультипликаток сектора попадания
            switch (s) //  По имени сектора дартса записываем мультипликатор
            {
                case "Z":
                    Multiplier = "Zero";
                    Sector = "Zero";
                    break;
                case "_":
                    Multiplier = "Single";
                    Sector = x;
                    break;
                case "D":
                    Multiplier = "Double";
                    Sector = new StringBuilder().Append("Double ").Append(x).ToString();
                    break;
                case "T":
                    Multiplier = "Tremble";
                    Sector = new StringBuilder().Append("Tremble ").Append(x).ToString();
                    break;
                case "B":
                    Multiplier = "Bull_25";
                    Sector = new StringBuilder().Append("25").ToString();
                    break;
                case "E":
                    Multiplier = "Bull_Eye";
                    Sector = new StringBuilder().Append("Bull Eye").ToString();
                    break;
            }
        }
    }
}