namespace GoProject
{
    public enum NodeCategory
    {
        @event,
        activity,
        subprocess,
        gateway,
        dataobject,
        datastore,
        privateProcess,
        Pool,
        Lane,
        annotation
    }

    public enum EventType
    {
        Empty = 1,
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
        Parallel = 12,
        Inclusive = 13
    }

    public enum EventDimension
    {
        Solid = 1,
        Dashed = 3,
        Double = 4,
        DoubleDashed = 6,
        DoubleFill = 7,
        RedFill = 8,
        Fill = 9
    }

    public enum GatewayType
    {
        Parallel = 1,
        Inclusive = 2,
        Complex = 3,
        Exclusive = 4,
        Event = 5,
        ExclusiveStart = 6,
        ParallelStart = 7
    }

    public enum TaskType
    {
        EmptyTask = 0,
        Receive = 1,
        User = 2,
        Manual = 3,
        Script = 4,
        Send = 5,
        Service = 6,
        BusinessRule = 7,
        Cancel = 8
    }
}
