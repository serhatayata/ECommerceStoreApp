namespace Monitoring.BackgroundTasks.Models.Enums;

public enum HealthCheckGrpcStatus
{
    UNKNOWN = 0,
    SERVING = 1,
    NOT_SERVING = 2,
    SERVICE_UNKNOWN = 3,
}
