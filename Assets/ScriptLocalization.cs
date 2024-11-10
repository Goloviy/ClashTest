using UnityEngine;

namespace I2.Loc
{
	public static class ScriptLocalization
	{

		public static class ChapterData
		{
			public static string des_1 		{ get{ return LocalizationManager.GetTranslation ("ChapterData/des.1"); } }
			public static string des_2 		{ get{ return LocalizationManager.GetTranslation ("ChapterData/des.2"); } }
			public static string des_3 		{ get{ return LocalizationManager.GetTranslation ("ChapterData/des.3"); } }
			public static string des_4 		{ get{ return LocalizationManager.GetTranslation ("ChapterData/des.4"); } }
			public static string tit_4 		{ get{ return LocalizationManager.GetTranslation ("ChapterData/tit.4"); } }
			public static string tit_3 		{ get{ return LocalizationManager.GetTranslation ("ChapterData/tit.3"); } }
			public static string tit_1 		{ get{ return LocalizationManager.GetTranslation ("ChapterData/tit.1"); } }
			public static string tit_2 		{ get{ return LocalizationManager.GetTranslation ("ChapterData/tit.2"); } }
		}
	}

    public static class ScriptTerms
	{

		public static class ChapterData
		{
		    public const string des_1 = "ChapterData/des.1";
		    public const string des_2 = "ChapterData/des.2";
		    public const string des_3 = "ChapterData/des.3";
		    public const string des_4 = "ChapterData/des.4";
		    public const string tit_4 = "ChapterData/tit.4";
		    public const string tit_3 = "ChapterData/tit.3";
		    public const string tit_1 = "ChapterData/tit.1";
		    public const string tit_2 = "ChapterData/tit.2";
		}
	}
}