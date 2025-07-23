﻿using Android.Runtime;
using Kotlin.Coroutines;
using Java.Lang;
using AndroidX.Health.Connect.Client;
using AndroidX.Health.Connect.Client.Aggregate;

namespace PeopleWith.Platforms.Android.Callbacks
{
    internal class Continuation : Java.Lang.Object, IContinuation
    {
        public ICoroutineContext Context => EmptyCoroutineContext.Instance;

        private readonly TaskCompletionSource<Java.Lang.Object> _taskCompletionSource;



        public Continuation(TaskCompletionSource<Java.Lang.Object> taskCompletionSource, CancellationToken cancellationToken)
        {
            _taskCompletionSource = taskCompletionSource;
            cancellationToken.Register(() => _taskCompletionSource.TrySetCanceled());
        }


        public void ResumeWith(Java.Lang.Object result)
        {


            //Check if there are any exception. We don't have access to the class Kotlin.Result.Failure. But we can extraxt the exception (Throwable) from the field in the class.
            var exceptionField = result.Class.GetDeclaredFields().FirstOrDefault(x => x.Name.Contains("exception", StringComparison.OrdinalIgnoreCase));
            if (exceptionField != null)
            {
                Throwable exception = exceptionField.Get(result).JavaCast<Java.Lang.Throwable>();
                _taskCompletionSource.TrySetException(new System.Exception(exception.Message));
                return;
            }
            else
            {
                _taskCompletionSource.TrySetResult(result);
            }


        }
    }
}