using System;
using System.Threading;
using Windows.UI.Xaml.Controls;

namespace AiJianShu.ExceptionHandler
{
    /// <summary>
    /// 封装一个SynchronizationContext对象,处理异步线程中的未处理异常.
    /// 通过传递到现有的SynchronizationContext对象实现.
    /// </summary>
    /// <example>
    /// 在App.xaml.cs文件中添加下列代码即可:
    /// <code>
    /// protected override void OnActivated(IActivatedEventArgs args)
    /// {
    ///     EnsureSyncContext();
    ///     ...
    /// }
    /// 
    /// protected override void OnLaunched(LaunchActivatedEventArgs args)
    /// {
    ///     EnsureSyncContext();
    ///     ...
    /// }
    /// 
    /// private void EnsureSyncContext()
    /// {
    ///     var exceptionHandlingSynchronizationContext = ExceptionHandlingSynchronizationContext.Register();
    ///     exceptionHandlingSynchronizationContext.UnhandledException += OnSynchronizationContextUnhandledException;
    /// }
    /// 
    /// private void OnSynchronizationContextUnhandledException(object sender, UnhandledExceptionEventArgs args)
    /// {
    ///     args.Handled = true;
    /// }
    /// </code>
    /// </example>
    internal class ExceptionHandlingSynchronizationContext : SynchronizationContext
    {
        /// <summary>
        /// 注册方法.在OnLaunched和OnActivated中注册
        /// </summary>
        /// <returns></returns>
        public static ExceptionHandlingSynchronizationContext Register()
        {
            var syncContext = Current;
            if (syncContext == null)
            {
                throw new InvalidOperationException("Ensure a synchronization context exists before calling this method.");
            }

            var customSynchronizationContext = syncContext as ExceptionHandlingSynchronizationContext;


            if (customSynchronizationContext == null)
            {
                customSynchronizationContext = new ExceptionHandlingSynchronizationContext(syncContext);
                SetSynchronizationContext(customSynchronizationContext);
            }


            return customSynchronizationContext;
        }

        /// <summary>
        /// 将SynchronizationContext于特定的页面绑定在一起
        /// </summary>
        /// <param name="rootFrame"></param>
        /// <returns></returns>
        public static ExceptionHandlingSynchronizationContext RegisterForFrame(Frame rootFrame)
        {
            if (rootFrame == null)
            {
                throw new ArgumentNullException("rootFrame");
            }

            var synchronizationContext = Register();

            rootFrame.Navigating += (sender, args) => EnsureContext(synchronizationContext);
            rootFrame.Loaded += (sender, args) => EnsureContext(synchronizationContext);

            return synchronizationContext;
        }

        private static void EnsureContext(SynchronizationContext context)
        {
            if (Current != context)
            {
                SetSynchronizationContext(context);
            }
        }


        private readonly SynchronizationContext _syncContext;


        public ExceptionHandlingSynchronizationContext(SynchronizationContext syncContext)
        {
            _syncContext = syncContext;
        }


        public override SynchronizationContext CreateCopy()
        {
            return new ExceptionHandlingSynchronizationContext(_syncContext.CreateCopy());
        }


        public override void OperationCompleted()
        {
            _syncContext.OperationCompleted();
        }


        public override void OperationStarted()
        {
            _syncContext.OperationStarted();
        }


        public override void Post(SendOrPostCallback d, object state)
        {
            _syncContext.Post(WrapCallback(d), state);
        }


        public override void Send(SendOrPostCallback d, object state)
        {
            _syncContext.Send(d, state);
        }


        private SendOrPostCallback WrapCallback(SendOrPostCallback sendOrPostCallback)
        {
            return state =>
            {
                try
                {
                    sendOrPostCallback(state);
                }
                catch (Exception ex)
                {
                    if (!HandleException(ex))
                    {
                        throw ex;
                    }
                }
            };
        }

        private bool HandleException(Exception exception)
        {
            if (UnhandledException == null)
            {
                return false;
            }

            var exWrapper = new AysncUnhandledExceptionEventArgs
            {
                Exception = exception
            };

            UnhandledException(this, exWrapper);

#if DEBUG && !DISABLE_XAML_GENERATED_BREAK_ON_UNHANDLED_EXCEPTION
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
#endif
            return exWrapper.Handled;
        }


        /// <summary>
        /// 监听异常事件,捕获后传给App
        /// </summary>
        public event EventHandler<AysncUnhandledExceptionEventArgs> UnhandledException;
    }

    public class AysncUnhandledExceptionEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public Exception Exception { get; set; }
    }
}
