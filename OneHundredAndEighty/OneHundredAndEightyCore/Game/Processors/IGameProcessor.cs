#region Usings

using OneHundredAndEightyCore.Domain;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public interface IGameProcessor
    {
        event ProcessorBase.EndMatchDelegate OnMatchEnd;
        void OnThrow(DetectedThrow thrw);
        bool CanUndoThrow { get; }
        void UndoThrow();
    }
}