﻿using Newtonsoft.Json;

namespace FileService.Api.Utilities.Results;

public class ErrorResultModel
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
