namespace FIAP.Domain;

/// <summary>
/// Provides predefined constant values for configuration statuses and default settings.
/// </summary>
public static class StatusConfiguration
{
    public const int DefaultPageNumber = 0;
    public const int DefaultPageSize = 10;

    public const int DefaultStatusCode = 200;
    public const int CreatedStatusCode = 201;
    public const int NoContentStatusCode = 204;

    public const int BadRequestErrorCode = 400;
    public const int UnauthorizedErrorCode = 401;
    public const int ForbiddenErrorCode = 403;
    public const int NotFoundErrorCode = 404;

    public const int DefaultErrorCode = 500;
}