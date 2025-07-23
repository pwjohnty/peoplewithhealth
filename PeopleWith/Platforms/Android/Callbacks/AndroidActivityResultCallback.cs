using Java.Lang;
using AndroidX.Activity.Result;
using Object = Java.Lang.Object;

public class AndroidActivityResultCallback : Object, IActivityResultCallback
{
    private readonly Action<Object?> _callback;

    public AndroidActivityResultCallback(Action<Object?> callback)
    {
        _callback = callback;
    }

    public void OnActivityResult(Object? result)
    {
        _callback?.Invoke(result);
    }
}
