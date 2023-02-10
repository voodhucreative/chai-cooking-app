using System;

[Serializable()]
public class ApiException : System.Exception
{
    public ApiException() : base() { }
    public ApiException(string message) : base(message) { }
    public ApiException(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected ApiException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}