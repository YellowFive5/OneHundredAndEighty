namespace OneHundredAndEightyCore.Common
{
    public class WindowSettings
    {
        public double Height { get; }

        public double Width { get; }

        public double PositionLeft { get; }

        public double PositionTop { get; }

        public WindowSettings(double height, 
                              double width,
                              double positionLeft, 
                              double positionTop)
        {
            Height = height;
            Width = width;
            PositionLeft = positionLeft;
            PositionTop = positionTop;
        }
    }
}