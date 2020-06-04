#region Usings

using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Windows.Score
{
    public interface IScoreWindow
    {
        double Left { get; }
        double Top { get; }
        double Height { get; }
        double Width { get; }
        void SetSemaphore(DetectionServiceStatus status);
        void SetThrowNumber(ThrowNumber number);
        void Show();
        void Close();
        void AddPointsTo(int pointsToAdd, Player player);
    }
}