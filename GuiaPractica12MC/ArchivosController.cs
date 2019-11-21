using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GuiaPractica12MC
{
    public class ArchivosController
    {
        private BinaryFormatter formatter;

        public ArchivosController()
        {
            formatter = new BinaryFormatter();
        }

        public void Serializar<T>(string archivo, T model)
        {
            var lista = Deserializar<T>(archivo);

            if (lista == null)
            {
                lista = new List<T>();
            }

            FileStream file = new FileStream(archivo, FileMode.Create, FileAccess.Write);
            lista.Add(model);
            formatter.Serialize(file, lista);
            file.Close();
        }

        public void SerializarDiccionario<T>(string archivo, T model)
        {
            var lista = Deserializar<T>(archivo);

            if (lista == null)
            {
                lista = new List<T>();
            }

            FileStream file = new FileStream(archivo, FileMode.Create, FileAccess.Write);
            lista.Add(model);
            formatter.Serialize(file, lista);
            file.Close();
        }

        public void Serializar<T>(string archivo, List<T> lista)
        {
            FileStream file = new FileStream(archivo, FileMode.Create, FileAccess.Write);
            formatter.Serialize(file, lista);
            file.Close();
        }

        public List<T> Deserializar<T>(string archivo)
        {
            FileStream file = new FileStream(archivo, FileMode.OpenOrCreate, FileAccess.Read);

            try
            {
                object resp = formatter.Deserialize(file);
                return resp as List<T>;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                file.Close();
            }
        }
    }
}
