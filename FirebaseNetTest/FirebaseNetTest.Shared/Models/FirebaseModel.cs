using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;

namespace FirebaseNetTest.Shared.Models
{
	class FirebaseModel : INotifyPropertyChanged
	{
		#region プロパティ

		/// <summary>
		/// ログイン時に使用するメールアドレス
		/// </summary>
		public string Email
		{
			get
			{
				return this._email;
			}
			set
			{
				if (this._email != value)
				{
					this._email = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _email = "test@example.com";

		/// <summary>
		/// ログイン時に使用するパスワード
		/// </summary>
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				if (this._password != value)
				{
					this._password = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _password = "UisK;ls/8";

		/// <summary>
		/// 認証メッセージ
		/// </summary>
		public string AuthMessage
		{
			get
			{
				return this._authMessage;
			}
			private set
			{
				if (this._authMessage != value)
				{
					this._authMessage = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _authMessage;

		#endregion

		#region 変数

		/// <summary>
		/// Firebase認証へのリンク
		/// </summary>
		private FirebaseAuthLink _authLink;

		#endregion

		#region メソッド

		/// <summary>
		/// サインインを行う
		/// </summary>
		public async Task SignInAsync()
		{
			try
			{
				// 認証するためのオブジェクトを作成
				var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseToken.ApiKey));

				// 認証を行い、リンクを取得する
				this._authLink = await auth.SignInWithEmailAndPasswordAsync(this.Email, this.Password);

				this.AuthMessage = "サインインに成功しました";
			}
			catch (FirebaseAuthException ex)
			{
				// エラー発生！
				this.AuthMessage = "エラー発生しました！エラーコード：" + ex.Reason;
			}
		}

		/// <summary>
		/// ユーザ作成を行う
		/// </summary>
		public async Task SignUpAsync()
		{
			try
			{
				// 認証するためのオブジェクトを作成
				var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseToken.ApiKey));

				// サインアップを行い、リンクを取得する
				this._authLink = await auth.CreateUserWithEmailAndPasswordAsync(this.Email, this.Password);

				this.AuthMessage = "ユーザ作成に成功しました";
			}
			catch (FirebaseAuthException ex)
			{
				// エラー発生！
				this.AuthMessage = "エラー発生しました！エラーコード：" + ex.Reason;
			}
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
