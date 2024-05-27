namespace TestShani
{
    public class QueuedAction
    {
        public string ActionType { get; set; }
        public string ExecutablePath { get; set; }
        public int DelaySeconds { get; set; }
        public DateTime InitiatedAt { get; set; }

        public bool IsPending()
        {
            return DateTime.Now < InitiatedAt.AddSeconds(DelaySeconds);
        }

        public override string ToString()
        {
            return $"{ActionType} - Path: {ExecutablePath}, Delay: {DelaySeconds}s, Initiated At: {InitiatedAt}";
        }
    }
}
