namespace GoProject
{
    public enum EventType
    {
        None = 1,
        Message = 2,
        Timer = 3,
        Escalation = 4,
        Conditional = 5,
        Link = 6,
        Error = 7,
        Cancel = 8,
        Compensation = 9,
        Signal = 10,
        Multiple = 11,
        MultipleParallel = 12,
        Terminate = 13
    }
}