using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TimerApp.Model;

namespace TimerApp.Droid
{
    class SaveAndLoad_Android : ISaveAndLoad

    {
        public string CreateDBPath()
        {
            try
            {
                string fileFolder = string.Empty;

                // Zusammensetzen des Dateipfades

                var documentsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "workouts");

                // Sicherstellen, dass der Pfad an den geschrieben werden soll existiert und ggf. angelegt wird

                if (!Directory.Exists(documentsPath))
                {
                    System.IO.Directory.CreateDirectory(documentsPath);
                }

                return Path.Combine(documentsPath, "workouts.db");


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}