using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FirebaseNetTest.Shared.Common;
using FirebaseNetTest.Shared.Models;

namespace FirebaseNetTest.Shared.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
	{
		private readonly FirebaseModel _firebaseModel = new FirebaseModel();

		public MainViewModel()
		{
			this._firebaseModel.PropertyChanged += this.RaisePropertyChanged;
		}

		#region プロパティ（FirebaseModel）

		/// <summary>
		/// 認証に使うメールアドレス
		/// </summary>
		public string Email
		{
			get
			{
				return this._firebaseModel.Email;
			}
			set
			{
				this._firebaseModel.Email = value;
			}
		}

		/// <summary>
		/// 認証に使うパスワード
		/// </summary>
		public string Password
		{
			get
			{
				return this._firebaseModel.Password;
			}
			set
			{
				this._firebaseModel.Password = value;
			}
		}

		/// <summary>
		/// 認証結果
		/// </summary>
		public string AuthMessage
		{
			get
			{
				return this._firebaseModel.AuthMessage;
			}
		}

		#endregion

		#region コマンド

		/// <summary>
		/// サインイン
		/// </summary>
		public RelayCommand SignInCommand
		{
			get
			{
				return this._signInCommand = this._signInCommand ?? new RelayCommand(async () =>
				{
					await this._firebaseModel.SignInAsync();
				});
			}
		}
		private RelayCommand _signInCommand;

		/// <summary>
		/// サインアップ
		/// </summary>
		public RelayCommand SignUpCommand
		{
			get
			{
				return this._signUpCommand = this._signUpCommand ?? new RelayCommand(async () =>
				{
					await this._firebaseModel.SignUpAsync();
				});
			}
		}
		private RelayCommand _signUpCommand;

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		protected void RaisePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.PropertyChanged?.Invoke(this, e);
		}

		#endregion
	}
}
