using Android.OS;
using Java.Interop;
using System;

namespace Insurance_Reminder
{
    public class InsuranceReminderBO : Java.Lang.Object, IParcelable
    {
        #region Objects and Properties
        public InsuranceReminderBO()
        {
        }
        private int id;
        private String strCompanyName;
        private String strPremiumAmount;
        private String stDueDate;
        private String strDueTime;
        private String strFrequency;

        public String Frequency
        {
            get { return strFrequency; }
            set { strFrequency = value; }
        }

        public String Time_Due
        {
            get { return strDueTime; }
            set { strDueTime = value; }
        }

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public String Company_Name
        {
            get { return this.strCompanyName; }
            set { this.strCompanyName = value; }
        }

        public String Premium_Amount
        {
            get { return this.strPremiumAmount; }
            set { this.strPremiumAmount = value; }
        }

        public String Due_Date
        {
            get { return this.stDueDate; }
            set { this.stDueDate = value; }
        }

        #endregion

        #region IParcelable implementation

        // The creator creates an instance of the specified object
        private static readonly GenericParcelableCreator<InsuranceReminderBO> _creator
            = new GenericParcelableCreator<InsuranceReminderBO>((parcel) => new InsuranceReminderBO(parcel));

        [ExportField("CREATOR")]
        public static GenericParcelableCreator<InsuranceReminderBO> GetCreator()
        {
            return _creator;
        }

        // Create a new SelectListItem populated with the values in parcel
        private InsuranceReminderBO(Parcel parcel)
        {
            this.ID = parcel.ReadInt();
            this.Company_Name = parcel.ReadString();
            this.Premium_Amount = parcel.ReadString();
            this.Due_Date = parcel.ReadString();
        }

        public int DescribeContents()
        {
            return 0;
        }

        // Save this instance's values to the parcel
        public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
        {
            dest.WriteInt(ID);
            dest.WriteString(Company_Name);
            dest.WriteString(Premium_Amount);
            dest.WriteString(Due_Date);
        }

        // Closest to the 'Java' way of implementing the creator
        /*public sealed class SelectListItemCreator : Java.Lang.Object, IParcelableCreator
        {
            public Java.Lang.Object CreateFromParcel(Parcel source)
            {
                return new SelectListItem(source);
            }

            public Java.Lang.Object[] NewArray(int size)
            {
                return new SelectListItem[size];
            }
        }*/

        #endregion
    }
    #region GenericParcelableCreator
    /// <summary>
    /// Generic Parcelable creator that can be used to create objects from parcels
    /// </summary>
    public sealed class GenericParcelableCreator<T> : Java.Lang.Object, IParcelableCreator
        where T : Java.Lang.Object, new()
    {
        private readonly Func<Parcel, T> _createFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParcelableDemo.GenericParcelableCreator`1"/> class.
        /// </summary>
        /// <param name='createFromParcelFunc'>
        /// Func that creates an instance of T, populated with the values from the parcel parameter
        /// </param>
        public GenericParcelableCreator(Func<Parcel, T> createFromParcelFunc)
        {
            _createFunc = createFromParcelFunc;
        }

        #region IParcelableCreator Implementation

        public Java.Lang.Object CreateFromParcel(Parcel source)
        {
            return _createFunc(source);
        }

        public Java.Lang.Object[] NewArray(int size)
        {
            return new T[size];
        }

        #endregion
    }
    #endregion
}

