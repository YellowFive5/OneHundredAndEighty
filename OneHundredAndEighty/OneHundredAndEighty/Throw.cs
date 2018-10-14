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
        public bool IsThrown { get; private set; }  //  Совершен ли данный бросок
        public string Sector { get; private set; }  //  Сектор попадания
        public string Multiplier { get; private set; }  //  Мультипликатор
        public int? Points { get; private set; } = null;    //  Очки

        public Throw()  //  Пустой бросок
        {

        }

        public Throw(object o)  //  Конструктор очередного броска
        {
            Shape S = o as Shape;   //  Берем сектор дартса
            this.IsThrown = true;   //  Бросок совершен
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
        public void ClearThrow()    //  Очищаем бросок
        {
            this.IsThrown = false;
            this.Sector = "";
            this.Multiplier = "";
            this.Points = null;
        }
    }
}
