namespace BlogAtor.Framework.Job;

public class JobContext
{
    public System.String Name { get; private set; }
    public Func<Task> JobDelegate { get; private set; }
    public TimeSpan Period { get; private set; }
    public Timer? Timer { get; set; }
    public Task? JobTask { get; set; }

    public JobContext(System.String name, Func<Task> jobDelegate, TimeSpan period)
    {
        Name = name;
        JobDelegate = jobDelegate;
        Period = period;
    }
}