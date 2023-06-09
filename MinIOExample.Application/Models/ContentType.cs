﻿using System;
using System.Collections.Generic;

namespace MinIOExample.Application.Models;

public class ContentType : ValueObject
{
    // Implemented this way since we don't not realy
    // interested in details of Content-Type header format
    private readonly string _contentType;

    public ContentType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentNullException(nameof(contentType));
        
        _contentType = contentType;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _contentType;
    }

    public override string ToString() => _contentType;
}