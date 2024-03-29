﻿using System;

[Serializable()]
public class FrameworkException : System.Exception
{
    public FrameworkException() : base() { }
    public FrameworkException(string message) : base(message) { }
    public FrameworkException(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected FrameworkException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}