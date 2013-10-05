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
using Android.Database;


namespace Insurance_Reminder
{
    public class InsuranceDataSource
    {
        private SQLiteDatabase database;
        private SQLiteHelper dbHelper;
        public string[] allColumns = { SQLiteHelper.Column_id,
										SQLiteHelper.Column_Company_Name,
										SQLiteHelper.Column_Premium_Amount,
                                        SQLiteHelper.Column_Due_Date,
                                        SQLiteHelper.Column_Frequency
									   };
        private long mRowId;
        public InsuranceDataSource(Context context)
        {
            dbHelper = new SQLiteHelper(context);
            database = dbHelper.WritableDatabase;
        }

        public long CreateInsuranceReminder(InsuranceReminderBO insure)
        {

            long row_id = -1;

            try
            {
                using (database)
                {
                    ContentValues content = new ContentValues();
                    
                    content.Put(SQLiteHelper.Column_Company_Name, insure.Company_Name);
                    content.Put(SQLiteHelper.Column_Premium_Amount, insure.Premium_Amount);
                    content.Put(SQLiteHelper.Column_Due_Date, insure.Due_Date);
                    content.Put(SQLiteHelper.Column_Frequency, insure.Frequency);
                     row_id = database.Insert(SQLiteHelper.TableName, null, content);
                    if (!row_id.Equals(-1))
                    {
                        mRowId = row_id;
                    }
                    
                }
            }
            catch (SQLiteException ex)
            {
                ex.ToString();
            }
            return row_id;
        }

        public bool DeleteInsuranceReminder(InsuranceReminderBO insurance)
        {
            bool bRtnVal = false;
            try
            {
                long id = insurance.ID;
                database.Delete(SQLiteHelper.TableName, String.Format("{0}={1}", SQLiteHelper.Column_id, id), null);
                bRtnVal = true;
            }
            catch (SQLiteException ex)
            {
                ex.ToString();
            }
            return bRtnVal;
        }

        public List<InsuranceReminderBO> GetAllInsuranceReminders()
        {
            List<InsuranceReminderBO> lst = new List<InsuranceReminderBO>();
            try
            {
                using (database)
                {
                    ICursor cursor = database.Query(SQLiteHelper.TableName, allColumns, null, null, null, null, null);
                    cursor.MoveToFirst();
                    while (!cursor.IsAfterLast)
                    {
                        InsuranceReminderBO insurance = new InsuranceReminderBO();
                        insurance.ID = cursor.GetInt(cursor.GetColumnIndexOrThrow(SQLiteHelper.Column_id));
                        insurance.Company_Name = cursor.GetString(cursor.GetColumnIndexOrThrow(SQLiteHelper.Column_Company_Name));
                        insurance.Premium_Amount = cursor.GetString(cursor.GetColumnIndexOrThrow(SQLiteHelper.Column_Premium_Amount));
                        insurance.Due_Date = cursor.GetString(cursor.GetColumnIndexOrThrow(SQLiteHelper.Column_Due_Date));
                        insurance.Frequency = cursor.GetString(cursor.GetColumnIndexOrThrow(SQLiteHelper.Column_Frequency));
                        lst.Add(insurance);
                        cursor.MoveToNext();
                    }
                    cursor.Close();
                }
            }
            catch (SQLiteException ex)
            {
                ex.ToString();
            }
            return lst;
        }
        public bool UpdateInsuranceReminder(InsuranceReminderBO insurance)
        {
            bool bRtnVal = false;
            try
            {
                using (database)
                {
                    ContentValues content = new ContentValues();
                    content.Put(SQLiteHelper.Column_Company_Name, insurance.Company_Name);
                    content.Put(SQLiteHelper.Column_Premium_Amount, insurance.Premium_Amount);
                    content.Put(SQLiteHelper.Column_Due_Date, insurance.Due_Date);
                    long row_affected = database.Update(SQLiteHelper.TableName, content, String.Format("{0}={1}", SQLiteHelper.Column_id, insurance.ID), null);
                    if (!row_affected.Equals(-1))
                    {
                        bRtnVal = true;
                    }
                    else
                        bRtnVal = false;
                }
            }
            catch (SQLiteException ex)
            {
                ex.ToString();
            }
            return bRtnVal;
        }
    }
}