using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Threading;

namespace Kogane
{
	public sealed class UniTaskCompletionSourceWithLog :
		IUniTaskSource,
		IPromise
	{
		//================================================================================
		// デリゲート(static)
		//================================================================================
		public static Action<string>         OnStartLog           { get; set; } = tag => UnityEngine.Debug.Log( $"{tag} 開始" );
		public static Action<string>         OnFinishLog          { get; set; } = tag => UnityEngine.Debug.Log( $"{tag} 終了" );
		public static Action<string>         OnTrySetCanceledLog  { get; set; } = tag => UnityEngine.Debug.Log( $"{tag} キャンセル設定" );
		public static Action<string>         OnTrySetExceptionLog { get; set; } = tag => UnityEngine.Debug.Log( $"{tag} 例外設定" );
		public static Action<string>         OnTrySetResultLog    { get; set; } = tag => UnityEngine.Debug.Log( $"{tag} 結果設定" );
		public static Action<string, object> OnTrySetResultLogT   { get; set; } = ( tag, result ) => UnityEngine.Debug.Log( $"{tag} 結果設定：{result.ToString()}" );

		//================================================================================
		// 変数(readonly)
		//================================================================================
		private readonly UniTaskCompletionSource m_source = new UniTaskCompletionSource();
		private readonly string                  m_tag;

		//================================================================================
		// プロパティ
		//================================================================================
		public UniTask Task
		{
			get
			{
				return UniTask.Create
				(
					async () =>
					{
						OnStartLog?.Invoke( m_tag );
						await m_source.Task;
						OnFinishLog?.Invoke( m_tag );
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
		public void GetResult( short token )
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
			OnTrySetCanceledLog?.Invoke( m_tag );
			return m_source.TrySetCanceled( cancellationToken );
		}

		[DebuggerHidden]
		public bool TrySetException( Exception exception )
		{
			OnTrySetExceptionLog?.Invoke( m_tag );
			return m_source.TrySetException( exception );
		}

		[DebuggerHidden]
		public bool TrySetResult()
		{
			OnTrySetResultLog?.Invoke( m_tag );
			return m_source.TrySetResult();
		}

		[DebuggerHidden]
		public UniTaskStatus UnsafeGetStatus()
		{
			return m_source.UnsafeGetStatus();
		}
	}
}