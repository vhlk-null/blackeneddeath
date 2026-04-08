using System.Diagnostics;
using Mediator;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
    where TResponse : notnull
{
    public async ValueTask<TResponse> Handle(TRequest request, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handler request={Request} - Response={Response} - RequestData={RequestDate}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        Stopwatch timer = new Stopwatch();
        timer.Start();

        TResponse response = await next(request, cancellationToken);

        timer.Stop();

        TimeSpan timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3) 
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}", typeof(TRequest).Name, timeTaken.Seconds);

        logger.LogInformation("[END] Handler {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);

        return response;
    }
}