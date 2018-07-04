using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Data;
using Xamarin.Forms;

namespace TimerApp.Model
{
    public partial class DatabaseManager //: IDatabaseManager
    {
        private string mDatabase = string.Empty;
        private const string TABLENAME = "Workouts";

        public DatabaseManager()
        {
            try
            {
                var documentsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "workouts");
                var dbpath = Path.Combine(documentsPath, "workouts.db");
                mDatabase = dbpath;// DependencyService.Get<ISaveAndLoad>().CreateDBPath();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        //public string value, name, date, key;
        public List<Workout> LoadWorkouts()
        {
            var sections = getKeyValuePairs("Workouts");
            if (sections==null)
            {
                return null;
            }
            string base64decoded = Base64Decode(sections[0][1]);
            
            List<Workout> results = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Workout>>(base64decoded);

            //foreach (var item in sections)
            //{
            //    var result = new Workout();
            //    result.Id = item[0];
            //    result.Timers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TimerSet>>(item[1]);
            //    result.Playlist = item[2];
            //    result.Name = item[3];
            //    results.Add(result);
            //}
            
            return results;
        }
        public bool writeEntry(string section, string key, object value)
        {
            SqlNonQuery nonquery = new SqlNonQuery();

            if (!(getSections().Contains(section)))
            {
                if (!createTable())
                {
                    return false;
                }
            }

            bool isUpdate = readEntry(section, key) != null;

            SqlStringBuilder builder;
            if (value == null)
            {
                builder = SqlStringBuilderFactory.createDELETE();
                {
                    var withBlock = builder;
                    withBlock.addTable(section);
                    withBlock.addCondition("Key", "=", key);
                }
            }
            else
            {
                builder = isUpdate ? Data.SqlStringBuilderFactory.createUPDATE() : Data.SqlStringBuilderFactory.createINSERT();
                {
                    var table = builder;
                    table.addTable(section);
                    table.addValue("Key", key);
                    table.addValue("Json", value);
                    if (isUpdate)
                    {
                        table.addCondition("Key", "=", key);
                        //table.addCondition("Json", "=", value);
                    }
                }
            }

            string qs = builder.ToString();
            Dictionary<string, object> @params = builder.getParameterValues();
            int result = nonquery.execute(qs, mDatabase, @params);

            return result > 0;
        }
        //public bool writeEntry(string section, string key, object value, string name, string setId)
        //{
        //    SqlNonQuery nonquery = new SqlNonQuery();

        //    if (!(getSections().Contains(section)))
        //    {
        //        if (!createTable())
        //        {
        //            return false;
        //        }
        //    }

        //    bool isUpdate = readEntry(section, key) != null;

        //    SqlStringBuilder builder;
        //    if (value == null)
        //    {
        //        builder = SqlStringBuilderFactory.createDELETE();
        //        {
        //            var withBlock = builder;
        //            withBlock.addTable(section);
        //            withBlock.addCondition("Key", "=", key);
        //        }
        //    }
        //    else
        //    {
        //        builder = isUpdate ? Data.SqlStringBuilderFactory.createUPDATE() : Data.SqlStringBuilderFactory.createINSERT();
        //        {
        //            var table = builder;
        //            table.addTable(section);
        //            table.addValue("Key", key);
        //            table.addValue("Json", value);
        //            table.addValue("Name", name);
        //            table.addValue("SetId", setId);
        //            if (isUpdate)
        //            {
        //                table.addCondition("Key", "=", key);
        //                table.addCondition("Json", "=", value);
        //                table.addCondition("Name", "=", name);
        //                table.addCondition("SetId", "=", setId);
        //            }
        //        }
        //    }

        //    string qs = builder.ToString();
        //    Dictionary<string, object> @params = builder.getParameterValues();
        //    int result = nonquery.execute(qs, mDatabase, @params);

        //    return result > 0;
        //}
        //internal void SaveObsCollOfTimers(ObservableCollection<AtomicTimer> timerList, string name, string setId)
        //{
        //    var values = Newtonsoft.Json.JsonConvert.SerializeObject(timerList);
        //    writeEntry("Timers", Guid.NewGuid().ToString(), values, name, setId);
        //}
        //internal void SaveObsCollOfTimerSets(ObservableCollection<TimerSet> timerSets, string name, string setId, string workoutId)
        //{
        //    var values = Newtonsoft.Json.JsonConvert.SerializeObject(timerSets);
        //    writeEntry("TimerSets", Guid.NewGuid().ToString(), values, name, setId);
        //}
        internal void SaveWorkouts(List<Workout> workouts)
        {
           
            string values = Newtonsoft.Json.JsonConvert.SerializeObject(workouts);
            //dirty hax cuz json macht sachen
            string base64Values = Base64Encode(values);
            writeEntry(TABLENAME, "WORKOUT", base64Values);
            //var sections = getKeyValuePairs(TABLENAME);
            //string decoded = Base64Decode(sections[0][1]);
        }
        private bool createTable()
        {
            SqlNonQuery nonquery = new SqlNonQuery();
            //nonquery.Password = mPassword;

            SqlStringBuilder builder = SqlStringBuilderFactory.createCREATE_TABLE();
            {
                var table = builder;
                table.addTable(TABLENAME);
                table.addColumn("Key", "VARCHAR", 100, false);
                table.addColumn("Json", "VARCHAR", 5000000, false);             
                table.setPrimaryKey("Key");
            }

            string qs = builder.ToString();
            Dictionary<string, object> @params = builder.getParameterValues();
            int result = nonquery.execute(qs, mDatabase, @params);
            return getSections().Contains(TABLENAME);
        }   

        public ArrayList getSections()
        {
            ArrayList tables = getTables();

            ArrayList result = new ArrayList();
            foreach (string entry in tables)
            {
                result.Add(entry);
            }

            return result;
        }

        private ArrayList getTables()
        {
            ArrayList result = new ArrayList();

            SqlQuery query = new SqlQuery();
            //query.Password = mPassword;

            SqlStringBuilder builder = SqlStringBuilderFactory.createSELECT();
            {
                var withBlock = builder;
                withBlock.addTable("sqlite_master");
                withBlock.addColumn("name");
                withBlock.addCondition("type", "=", "table");
            }

            string qs = builder.ToString();
            Dictionary<string, object> @params = builder.getParameterValues();
            List<List<string>> dt = query.execute(qs, mDatabase, @params);
            if (dt == null || dt.Count == 0)
                return result;

            foreach (List<string> row in dt)
                result.Add(row[0]);

            return result;
        }
        
        public List<List<string>> getKeyValuePairs(string section)
        {
            if (!getSections().Contains(section))
                return null;

            SqlQuery query = new SqlQuery();
            //query.Password = mPassword;

            SqlStringBuilder builder = SqlStringBuilderFactory.createSELECT();
            {
                var table = builder;
                table.addTable(section);
                table.addColumn("Key");
                table.addColumn("Json");                
            }
            string qs = builder.ToString();
            Dictionary<string, object> @params = builder.getParameterValues();
            List<List<string>> dt = query.execute(qs, mDatabase, @params);
            if (dt == null || dt.Count == 0)
                return new List<List<string>>();//new List<KeyValuePair<string, string>>();

            return dt;
            //(from row in dt
            //    select new KeyValuePair<string, string>(row[0].ToString(), row[1].ToString())).ToList();
        }

        public object readEntry(string section, string key)
        {
            if (!getSections().Contains(section))
                return null;

            SqlQuery query = new SqlQuery();
            //query.Password = mPassword;

            SqlStringBuilder builder = SqlStringBuilderFactory.createSELECT();
            {
                var withBlock = builder;
                withBlock.addTable(section);
                withBlock.addColumn("Json");
                withBlock.addCondition("Key", "=", key);
            }
            string qs = builder.ToString();
            Dictionary<string, object> @params = builder.getParameterValues();
            List<List<string>> dt = query.execute(qs, mDatabase, @params);
            if (dt == null || dt.Count == 0)
                return null;

            return dt[0][0];
        }

        //private object readEntry(string section, string key)
        //{
        //    throw new NotImplementedException();
        //}
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
    
}
