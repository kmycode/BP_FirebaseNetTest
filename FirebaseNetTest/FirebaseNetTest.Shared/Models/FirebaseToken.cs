using System;
using System.Collections.Generic;
using System.Text;

namespace FirebaseNetTest.Shared.Models
{
    static class FirebaseToken
    {
		/// <summary>
		/// APIキー
		/// </summary>
		public static string ApiKey { get; } = "<Your API Key>";

		/// <summary>
		/// 認証ドメイン
		/// </summary>
		public static string AuthDomain { get; } = "<Your Domain>";

		/// <summary>
		/// データベースのURL
		/// </summary>
		public static string DatabaseUrl { get; } = "<Your Database Url>";

		/// <summary>
		/// ストレージバケット
		/// </summary>
		public static string StorageBucket { get; } = "<Your Storage Bucket>";
	}
}
