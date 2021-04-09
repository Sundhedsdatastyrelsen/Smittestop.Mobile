using System;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using Android.Runtime;
using Java.Util;
using Object = Java.Lang.Object;
using Task = Android.Gms.Tasks.Task;

namespace NDB.Covid19.Droid.Utils
{
    internal static class ExposureNotificationStatusExtensions
    {
        public static Task<AbstractCollection> CastTask(this Task androidTask)
        {
            TaskCompletionSource<AbstractCollection> tcs = new TaskCompletionSource<AbstractCollection>();
            androidTask.AddOnCompleteListener(new MyCompleteListener(t =>
            {
                if (t.Exception == null)
                    tcs.TrySetResult(t.Result.JavaCast<AbstractCollection>());
                else
                    tcs.TrySetException(t.Exception);
            }));
            return tcs.Task;
        }

        private class MyCompleteListener :
            Object,
            IOnCompleteListener
        {
            public MyCompleteListener(Action<Task> onComplete)
            {
                OnCompleteHandler = onComplete;
            }

            private Action<Task> OnCompleteHandler { get; }

            public void OnComplete(Task task)
            {
                OnCompleteHandler?.Invoke(task);
            }
        }
    }
}