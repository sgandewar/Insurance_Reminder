using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Database.Sqlite;

namespace Insurance_Reminder
{
    public class SQLiteHelper :SQLiteOpenHelper
    {
        private static string DataBaseName = "Insurance_db";
        private static int iDbVersion = 2;
        public static string TableName = "Insurance_Table";
        public static string Column_id = "_id";
        public static string Column_Company_Name = "Company_Name";
        public static string Column_Premium_Amount = "Premium_Amount";
        public static string Column_Due_Date = "Due_Date";
        public static string Column_Frequency = "Frequency";

        public SQLiteHelper(Context context)
            : base(context, DataBaseName, null, iDbVersion)
        {
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            string strQueryTableCreate = String.Format("create table if not exists {0} ({1} integer primary key autoincrement,{2} text not null,{3} text not null,{4} text not null,{5} text not null);",
                                                       TableName, Column_id, Column_Company_Name, Column_Premium_Amount, Column_Due_Date, Column_Frequency);
            db.ExecSQL(strQueryTableCreate);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS " + TableName);
            OnCreate(db);
        }
    }
}