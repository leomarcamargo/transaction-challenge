using System.Collections;

namespace Transactions.API.Models;

public record ExceptionResponse(string Type,
    int Code,
    string Message,
    IDictionary? Data);