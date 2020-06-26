#region Usings

using OneHundredAndEightyCore.Domain;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public interface IGameProcessor
    {
        void OnThrow(DetectedThrow thrw);
        event ProcessorBase.EndMatchDelegate OnMatchEnd;
    }
}