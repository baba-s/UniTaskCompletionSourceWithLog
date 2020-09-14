using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Threading;

namespace Kogane
{
	public sealed class UniTaskCompletionSourceWithLog<T> :
		IUniTaskSource<T>,
		IPromise<T>
	{
		//================================================================================
		// 変数(readonly)
		//================================================================================
		private readonly UniTaskCompletionSource<T> m_source = new UniTaskCompletionSource<T>();
		private readonly string                     m_tag;

		//================================================================================
		// プロパティ
		//================================================================================
		public UniTask<T> Task
		{
			get
			{
				return UniTask.Create
				(
					async () =>
					{
						UniTaskCompletionSourceWithLog.OnStartLog?.Invoke( m_tag );
						var result = await m_source.Task;
						UniTaskCompletionSourceWithLog.OnFinishLog?.Invoke( m_tag );
						return result;
					}
				);
			}
		}

		//================================================================================
		// 関数
		//================================================================================
		public UniTaskCompletionSourceWithLog( string tag )
		{
			m_tag = tag;
		}

		[DebuggerHidden]
		public T GetResult( short token )
		{
			return m_source.GetResult( token );
		}

		[DebuggerHidden]
		void IUniTaskSource.GetResult( short token )
		{
			m_source.GetResult( token );
		}

		[DebuggerHidden]
		public UniTaskStatus GetStatus( short token )
		{
			return m_source.GetStatus( token );
		}

		[DebuggerHidden]
		public void OnCompleted
		(
			Action<object> continuation,
			object         state,
			short          token
		)
		{
			m_source.OnCompleted( continuation, state, token );
		}

		[DebuggerHidden]
		public bool TrySetCanceled()
		{
			return TrySetCanceled( default );
		}

		[DebuggerHidden]
		public bool TrySetCanceled( CancellationToken cancellationToken )
		{
			UniTaskCompletionSourceWithLog.OnTrySetCanceledLog?.Invoke( m_tag );
			return m_source.TrySetCanceled( cancellationToken );
		}

		[DebuggerHidden]
		public bool TrySetException( Exception exception )
		{
			UniTaskCompletionSourceWithLog.OnTrySetExceptionLog?.Invoke( m_tag );
			return m_source.TrySetException( exception );
		}

		[DebuggerHidden]
		public bool TrySetResult( T result )
		{
			UniTaskCompletionSourceWithLog.OnTrySetResultLogT?.Invoke( m_tag, result );
			return m_source.TrySetResult( result );
		}

		[DebuggerHidden]
		public UniTaskStatus UnsafeGetStatus()
		{
			return m_source.UnsafeGetStatus();
		}
	}
}