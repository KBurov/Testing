namespace Testing.Common
{
    public interface IIssuesSettings
    {
        bool IsTimeLimited { get; }

        uint TimeLimit { get; }
    }
}
