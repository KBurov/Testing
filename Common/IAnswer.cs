namespace Testing.Common
{
    public interface IAnswer
    {
        bool IsCorrect { get; }

        uint OrderNo { get; }
// ReSharper disable InconsistentNaming
        string ContentUA { get; }

        string ContentRU { get; }
// ReSharper restore InconsistentNaming
        bool Selected { get; }

        string ImageFileName { get; }
    }
}
