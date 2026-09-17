namespace BlogAtor.Framework;

using System.Threading;
using System.Threading.Tasks;

using BlogAtor.Framework.Job;

using Microsoft.Extensions.Logging;

public abstract class ModuleBase : ServiceBase, IModule, IDisposable
{
    private readonly IList<JobContext> _periodicJobs;
    protected ModuleBase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _periodicJobs = new List<JobContext>();
    }
    protected void AddPeriodicJob(Func<Task> jobDelegate, TimeSpan period, System.String? name = null)
    {
        _periodicJobs.Add(new JobContext(name ?? jobDelegate.Method.Name, jobDelegate, period));
    }
    private void JobRunnerWrapper(System.Object? state)
    {
        var jobContext = state as JobContext;
        if (jobContext == null)
        {
            throw new NotImplementedException("Invalid job state");
        }

        if (jobContext.JobTask != null && !jobContext.JobTask.IsCompleted)
        {
            Logger.LogWarning($"Periodic job scheduled before completion <name={jobContext.Name}>");
            return;
        }

        Logger.LogTrace($"Starting periodic job run <name={jobContext.Name}>");
        jobContext.JobTask = Task.Run(jobContext.JobDelegate);
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.LogTrace($"Starting periodic jobs for module <module={this.GetType().Name}>");
        foreach (var jobContext in _periodicJobs)
        {
            var timer = new Timer(JobRunnerWrapper, jobContext, TimeSpan.Zero, jobContext.Period);
            jobContext.Timer = timer;
        }
        await OnStartAsync(cancellationToken);
    }
    protected virtual Task OnStartAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation($"Starting module <module={this.GetType().Name}>");
        return Task.CompletedTask;
    }
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation($"Stopping periodic jobs for module <module={this.GetType().Name}>");
        foreach (var jobContext in _periodicJobs)
        {
            jobContext.Timer?.Change(Timeout.Infinite, 0);
        }
        foreach (var jobContext in _periodicJobs)
        {
            if (jobContext.JobTask != null && !jobContext.JobTask.IsCompleted)
            {
                Logger.LogInformation($"Awaiting periodic jobs termination <module={this.GetType().Name} job={jobContext.Name}>");
                await jobContext.JobTask;
            }
        }
        await OnStopAsync(cancellationToken);
    }
    protected virtual Task OnStopAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation($"Stopping module <module={this.GetType().Name}>");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        foreach (var jobContext in _periodicJobs)
        {
            jobContext.Timer?.Dispose();
        }
    }
}