﻿using System.Text.Json;

namespace ExpenseTracker.Base;

public class ApiResponse
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public ApiResponse(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Success = true;
        }
        else
        {
            Success = false;
            Message = message;
        }
    }

    public bool Success { get; set; }
    public string Message { get; set; }
    public DateTime ServerDate { get; set; } = DateTime.UtcNow;
    public Guid ReferenceNo { get; set; } = Guid.NewGuid();
}

public class ApiResponse<T>
{
    public DateTime ServerDate { get; set; } = DateTime.UtcNow;
    public Guid ReferenceNo { get; set; } = Guid.NewGuid();
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Response { get; set; }

    public ApiResponse(bool isSuccess)
    {
        Success = isSuccess;
        Message = isSuccess ? "Success" : "Error";
    }

    public ApiResponse(T data)
    {
        Success = true;
        Response = data;
        Message = "Success";
    }

    public ApiResponse(string message)
    {
        Success = false;
        Message = message;
    }
}