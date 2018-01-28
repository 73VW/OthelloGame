using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using OthelloIAG5;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows;

namespace OthelloPedrettiFasmeyer
{
    class SaveManager
    {

        public const String FILES_FILTER = "Space Othello files (*.so)|*.so";

        public static void SerializeObject<T>(T serializable)
        {
            SaveFileDialog sfd = GetSfd();

            if (sfd.ShowDialog() == true)
            { //User chose a file
                if (sfd.FileName != "")
                {
                    try
                    {
                        IFormatter formatter = new BinaryFormatter();
                        Stream stream = new FileStream(sfd.FileName, 
                            FileMode.Create, 
                            FileAccess.Write,
                            FileShare.None);
                        formatter.Serialize(stream, serializable);
                        stream.Close();
                        MessageBox.Show("Partie enregistrée sous le nom " + sfd.FileName);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Impossible d'enregistrer la partie\n"+ex.ToString());
                    }
                }
            }
        }

        public static T DeserializeObject<T>()
        {
            OpenFileDialog ofd = GetOfd();

            if (ofd.ShowDialog() == true)
            {
                if(ofd.FileName!="")
                {
                    try
                    {
                        IFormatter formatter = new BinaryFormatter();
                        Stream stream = new FileStream(ofd.FileName,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.Read);
                        try
                        {
                            return (T)formatter.Deserialize(stream);
                        }
                        catch
                        {
                            return default(T);
                        }
                        finally
                        {
                            stream.Close();
                            MessageBox.Show("Partie chargée depuis " + ofd.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Impossible de charger la partie\n" + ex.ToString());
                    }
                }
            }
            return default(T);
        }

        private static OpenFileDialog GetOfd()
        {
            return new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = FILES_FILTER,
                FilterIndex = 2,
                RestoreDirectory = true
            };
        }

        private static SaveFileDialog GetSfd()
        {
            return new SaveFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = FILES_FILTER,
                FilterIndex = 2,
                RestoreDirectory = true
            };
        }
    }
}
