using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using PCLStorage;
using System.Linq;
using System.Collections.ObjectModel;

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

		/// <summary>
		/// ストレージのファイルパス
		/// </summary>
		public string StorageFilePath
		{
			get
			{
				return this._storageFilePath;
			}
			set
			{
				if (this._storageFilePath != value)
				{
					this._storageFilePath = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _storageFilePath = "test.txt";

		/// <summary>
		/// ストレージに保存するテキスト
		/// </summary>
		public string StorageSaveText
		{
			get
			{
				return this._storageSaveText;
			}
			set
			{
				if (this._storageSaveText != value)
				{
					this._storageSaveText = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _storageSaveText = "いやっほーい！";

		/// <summary>
		/// ストレージの操作の結果メッセージ
		/// </summary>
		public string StorageMessage
		{
			get
			{
				return this._storageMessage;
			}
			private set
			{
				if (this._storageMessage != value)
				{
					this._storageMessage = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _storageMessage;

		/// <summary>
		/// データベースで参照するパス
		/// </summary>
		public string DatabasePath
		{
			get
			{
				return this._databasePath;
			}
			set
			{
				if (this._databasePath != value)
				{
					this._databasePath = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _databasePath = "sample/path";

		/// <summary>
		/// データベースに保存するテキスト
		/// </summary>
		public string DatabaseSaveText
		{
			get
			{
				return this._databaseSaveText;
			}
			set
			{
				if (this._databaseSaveText != value)
				{
					this._databaseSaveText = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _databaseSaveText = "異世界に行きたい";

		/// <summary>
		/// データベース操作のメッセージ
		/// </summary>
		public string DatabaseMessage
		{
			get
			{
				return this._databaseMessage;
			}
			private set
			{
				if (this._databaseMessage != value)
				{
					this._databaseMessage = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _databaseMessage;

		/// <summary>
		/// データベースにあるデータ
		/// </summary>
		public ICollection<string> DatabaseDatas
		{
			get
			{
				return this._databaseDatas;
			}
			set
			{
				this._databaseDatas = value;
				this.OnPropertyChanged();
			}
		}
		private ICollection<string> _databaseDatas;

		/// <summary>
		/// リアルおままごとの値
		/// </summary>
		public string RealtimeDatabaseValue
		{
			get
			{
				return this._realtimeDatabaseValue;
			}
			private set
			{
				if (this._realtimeDatabaseValue != value)
				{
					this._realtimeDatabaseValue = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _realtimeDatabaseValue;

		#endregion

		#region 変数

		/// <summary>
		/// Firebase認証へのリンク
		/// </summary>
		private FirebaseAuthLink _authLink;

		/// <summary>
		/// リアルタイムでのデータベース監視をおこなうインスタンス（を破棄する権限を持つインスタンス）
		/// </summary>
		private IDisposable _realtimeDatabaseWatcher;

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

				// データベースの監視を開始
				this.StartWatchingRealtimeValue();
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

		/// <summary>
		/// テキストをストレージへ保存
		/// </summary>
		public async Task UploadTextToStorageAsync()
		{
			try
			{
				// ストレージへの参照を作成
				var reference = this.GetStorageReference();

				// ストレージへアップロード
				await reference.PutAsync(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(this.StorageSaveText)));

				this.StorageMessage = "ストレージにアップロードしました";
			}
			catch (FirebaseStorageException ex)
			{
				this.StorageMessage = "エラー発生しました！：" + ex.ResponseData;
			}
		}

		/// <summary>
		/// ファイルをストレージへアップロード
		/// </summary>
		public async Task UploadFileToStorageAsync()
		{
			try
			{
				// ストレージへの参照を作成
				var reference = this.GetStorageReference("test.png");

				// ファイルをストレージへアップロード
				var file = await FileSystem.Current.GetFileFromPathAsync("test.png");
				using (var stream = await file.OpenAsync(FileAccess.Read))
				{
					await reference.PutAsync(stream);
				}

				this.StorageMessage = "ストレージにアップロードしました";
			}
			catch (FirebaseStorageException ex)
			{
				this.StorageMessage = "エラー発生しました！：" + ex.ResponseData;
			}
		}

		/// <summary>
		/// ストレージからテキストをダウンロード
		/// </summary>
		public async Task DownloadTextFromStorageAsync()
		{
			try
			{
				// ストレージへの参照を作成
				var reference = this.GetStorageReference();

				// ストレージからダウンロードURLを取得
				var uri = await reference.GetDownloadUrlAsync();

				// HTTPを介して中身をダウンロード
				var httpClient = new HttpClient();
				var response = await httpClient.GetAsync(uri);
				var storageText = await response.Content.ReadAsStringAsync();

				this.StorageMessage = "ダウンロード完了: " + storageText;
			}
			catch (FirebaseStorageException ex)
			{
				this.StorageMessage = "エラー発生しました！：" + ex.ResponseData;
			}
		}

		/// <summary>
		/// ストレージからファイルをダウンロード
		/// </summary>
		public async Task DownloadFileFromStorageAsync()
		{
			try
			{
				// ストレージへの参照を作成
				var reference = this.GetStorageReference("test.png");

				// ストレージからダウンロードURLを取得
				var uri = await reference.GetDownloadUrlAsync();

				// HTTPを介して中身をダウンロード・ファイルへ保存
				var httpClient = new HttpClient();
				var response = await httpClient.GetAsync(uri);
				using (var stream = await response.Content.ReadAsStreamAsync())
				{
					// ファイルがなければ作成、あれば開く
					var folder = await FileSystem.Current.GetFolderFromPathAsync("./");
					var file = await folder.CreateFileAsync("test_downloaded.png", CreationCollisionOption.OpenIfExists);

					// HTTPのストリームとファイルのストリームをつなげる
					using (var fileStream = await file.OpenAsync(FileAccess.ReadAndWrite))
					{
						stream.CopyTo(fileStream);
					}
				}

				this.StorageMessage = "ダウンロード完了";
			}
			catch (FirebaseStorageException ex)
			{
				this.StorageMessage = "エラー発生しました！：" + ex.ResponseData;
			}
		}

		/// <summary>
		/// ストレージへの参照を取得
		/// </summary>
		/// <returns>設定値に応じたストレージへの参照</returns>
		private FirebaseStorageReference GetStorageReference(string path = null)
		{
			return new FirebaseStorage(
				// FirebaseコンソールのWeb APIのとこに書いてあった、appspot.comでおわるやつ
				FirebaseToken.StorageBucket,
				new FirebaseStorageOptions
				{
					// 認証の時に手に入れたリンクからトークンを抜く
					AuthTokenAsyncFactory = () => Task.FromResult(this._authLink?.FirebaseToken),
				})
				.Child(path ?? this.StorageFilePath);       // 参照先パス（フォルダ名、ファイル名）
		}

		/// <summary>
		/// データベースにテキストを保存
		/// </summary>
		public async Task UploadTextToDatabaseAsync()
		{
			try
			{
				// クエリを取得
				var query = this.GetDatabaseQuery();

				// データを格納する
				await query.PostAsync(new DatabaseData { Value = this.DatabaseSaveText, });

				this.DatabaseMessage = "データ保存に成功しました";
			}
			catch (FirebaseException ex)
			{
				this.DatabaseMessage = "エラー発生しました！：" + ex.ResponseData;
			}
			catch
			{
				this.DatabaseMessage = "エラー発生しました！";
			}
		}

		/// <summary>
		/// データベースからテキストをダウンロード
		/// </summary>
		public async Task DownloadTextFromDatabaseAsync()
		{
			try
			{
				// クエリを取得
				var query = this.GetDatabaseQuery();

				// データを取得する
				var results = await query.OnceAsync<DatabaseData>();
				var resultObjects = results.Select(obj => obj.Object);

				this.DatabaseMessage = "取得完了：" + resultObjects.First().Value;
			}
			catch (FirebaseException ex)
			{
				this.DatabaseMessage = "エラー発生しました！：" + ex.ResponseData;
			}
			catch
			{
				this.DatabaseMessage = "エラー発生しました！";
			}
		}

		/// <summary>
		/// データベースからテキストをダウンロードしてリストを作成
		/// </summary>
		public async Task DownloadTextListFromDatabaseAsync()
		{
			try
			{
				// クエリを取得
				var query = this.GetDatabaseQuery();

				// データを取得する
				var results = await query
					.OrderBy("Value")
					.LimitToFirst(3)
					.OnceAsync<DatabaseData>();
				var resultObjects = results.Select(obj => obj.Object);

				// 値をリストに設定
				this.DatabaseDatas.Clear();
				foreach (var obj in resultObjects)
				{
					this.DatabaseDatas.Add(obj.Value);
				}

				this.DatabaseMessage = "データをリストに入れました：" + this.DatabaseDatas.Count;
			}
			catch (FirebaseException ex)
			{
				this.DatabaseMessage = "エラー発生しました！：" + ex.ResponseData;
			}
			catch
			{
				this.DatabaseMessage = "エラー発生しました！";
			}
		}

		/// <summary>
		/// リアルタイムのデータ監視を開始
		/// </summary>
		public void StartWatchingRealtimeValue()
		{
			// データベースの監視を開始
			this._realtimeDatabaseWatcher =
				this.GetDatabaseQuery("sample/path")
				.AsObservable<DatabaseData>()
				.Subscribe(ev =>
				{
					if (ev?.Object != null)
					{
						switch (ev.EventType)
						{
							case Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate:
								this.RealtimeDatabaseValue = "監視: 値 " + ev.Object.Value + " が追加されました";
								break;
							case Firebase.Database.Streaming.FirebaseEventType.Delete:
								this.RealtimeDatabaseValue = "監視: 値 " + ev.Object.Value + " が削除されました";
								break;
						}
					}
				});

			// 監視を中止（なぜDisposeなのか詳しいことはRxの勉強へGo!）
			// this._realtimeDatabaseWatcher.Dispose();
		}

		/// <summary>
		/// データベースのクエリを取得
		/// </summary>
		/// <returns>設定値に応じたデータベースへの参照</returns>
		private ChildQuery GetDatabaseQuery(string path = null)
		{
			if (this._authLink == null)
			{
				throw new NullReferenceException();
			}
			return new FirebaseClient(
				// FirebaseコンソールのWeb APIのとこに書いてあった、firebaseio.comでおわるやつ
				FirebaseToken.DatabaseUrl,
				new FirebaseOptions
				{
					// 認証の時に手に入れたリンクからトークンを抜く
					AuthTokenAsyncFactory = () => Task.FromResult(this._authLink.FirebaseToken),
				})
				.Child(path ?? this.DatabasePath);		// 参照先パス
		}

		#endregion

		#region 構造体定義

		/// <summary>
		/// 文字列型のラッパ
		/// </summary>
		public class DatabaseData
		{
			public string Value { get; set; }
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
