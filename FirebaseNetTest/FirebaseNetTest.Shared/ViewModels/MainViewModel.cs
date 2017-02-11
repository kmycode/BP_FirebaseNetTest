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

		/// <summary>
		/// ストレージのファイルパス
		/// </summary>
		public string StorageFilePath
		{
			get
			{
				return this._firebaseModel.StorageFilePath;
			}
			set
			{
				this._firebaseModel.StorageFilePath = value;
			}
		}

		/// <summary>
		/// ストレージに保存するテキスト
		/// </summary>
		public string StorageSaveText
		{
			get
			{
				return this._firebaseModel.StorageSaveText;
			}
			set
			{
				this._firebaseModel.StorageSaveText = value;
			}
		}

		/// <summary>
		/// ストレージの操作の結果メッセージ
		/// </summary>
		public string StorageMessage
		{
			get
			{
				return this._firebaseModel.StorageMessage;
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

		/// <summary>
		/// ストレージにアップロード
		/// </summary>
		public RelayCommand StorageUploadCommand
		{
			get
			{
				return this._storageUploadCommand = this._storageUploadCommand ?? new RelayCommand(async () =>
				{
					await this._firebaseModel.UploadTextToStorageAsync();
					//await this._firebaseModel.UploadFileToStorageAsync();
				});
			}
		}
		private RelayCommand _storageUploadCommand;

		/// <summary>
		/// ストレージからダウンロード
		/// </summary>
		public RelayCommand StorageDownloadCommand
		{
			get
			{
				return this._storageDownloadCommand = this._storageDownloadCommand ?? new RelayCommand(async () =>
				{
					await this._firebaseModel.DownloadTextFromStorageAsync();
					//await this._firebaseModel.DownloadFileFromStorageAsync();
				});
			}
		}
		private RelayCommand _storageDownloadCommand;

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
