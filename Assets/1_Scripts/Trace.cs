//#define USE_LOGS
using System;
using System.Diagnostics;


public class Trace
{
    [Conditional("USE_LOGS")]
    public static void Msg(object msg)
    {
        UnityEngine.Debug.Log(msg);
//        Debug.(msg);
    }
}

// elsewhere in the code - if USE_LOGS is defined, you'll see this output, otherwise,
// it's stripped from the build
//Trace.Msg( "Hello world" )

