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
        void Show();
        void Close();
        void SetSemaphore(DetectionServiceStatus status);
        void SetThrowNumber(ThrowNumber number);
        void AddPointsTo(Player player, int pointsToAdd);
        void SetPointsTo(Player player, int pointsToSet);
        void CheckoutShowOrUpdateFor(Player player, string hint);
        void CheckoutHideFor(Player player);
    }
}