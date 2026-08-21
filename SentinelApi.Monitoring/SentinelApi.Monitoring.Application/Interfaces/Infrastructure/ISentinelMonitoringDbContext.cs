using Microsoft.EntityFrameworkCore;
using SentinelApi.Monitoring.Domain.Entities;

namespace SentinelApi.Monitoring.Application.Interfaces.Infrastructure;

public interface ISentinelMonitoringDbContext
{
    /// <summary>Історія дій користувачів сервісу.</summary>
    DbSet<ActionLog> ActionLogs { get; set; }

    /// <summary>Конфігурації перевірок сервісів.</summary>
    DbSet<Check> Checks { get; set; }

    /// <summary>Результати виконаних перевірок.</summary>
    DbSet<CheckResult> CheckResults { get; set; }

    /// <summary>Сервіси, які підлягають моніторингу.</summary>
    DbSet<ServiceDefinition> ServiceDefinitions { get; set; }

    /// <summary>
    /// Зберігає накопичені зміни в базі даних.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
