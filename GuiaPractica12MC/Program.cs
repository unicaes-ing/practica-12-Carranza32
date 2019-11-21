using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GuiaPractica12MC
{
    class Program
    {
        #region atributos
        private static bool isNumber;
        private static ArchivosController archivos;
        private static Dictionary<string, alumno> dAlumnos = new Dictionary<string, alumno>();
        private static BinaryFormatter formatter = new BinaryFormatter();
        private const string N_ARCH = "alumnos.bin";
        #endregion

        [Serializable]
        public struct Mascota
        {
            public string Nombre { get; set; }
            public string Especie { get; set; }
            public string Raza { get; set; }
            public string Sexo { get; set; }
            public int Edad { get; set; }
        }

        [Serializable]
        public struct alumno
        {
            public string carnet;
            public string nombre;
            public string carrera;
            private decimal cum;
            public void serCum(decimal cum)
            {
                if (cum > 0)
                {
                    this.cum = cum;
                }
            }
            public decimal getCum()
            {
                return cum;
            }
        }

        static void Main(string[] args)
        {
            archivos = new ArchivosController();
            int opc = 0;
            bool isNumber;
            do
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Escoja una opcion");
                    Console.WriteLine("1 - Ejercicio 1");
                    Console.WriteLine("2 - Ejercicio 2");
                    Console.WriteLine("3 - Ejercicio 3");
                    Console.WriteLine("4 - Ejercicio 4");
                    Console.WriteLine("5 - Ejercicio 5");
                    Console.WriteLine("6 - Ejercicio 6");
                    Console.WriteLine("0 - Salir");
                    isNumber = int.TryParse(Console.ReadLine(), out opc);
                } while (isNumber == false || opc < 0);
                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        Ejercicio1();
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        Ejercicio2();
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.Clear();
                        Ejercicio3();
                        Console.ReadKey();
                        break;
                    default:
                        break;
                }
            } while (opc != 0);
        }

        private static void Ejercicio1()
        {
            Mascota mascota = new Mascota();
            Console.WriteLine("Ingresar nueva mascota");
            Console.WriteLine("Nombre:");
            mascota.Nombre = Console.ReadLine();
            Console.WriteLine("Especie:");
            mascota.Especie = Console.ReadLine();
            Console.WriteLine("Raza:");
            mascota.Raza = Console.ReadLine();
            Console.WriteLine("Sexo:");
            mascota.Sexo = Console.ReadLine();
            Console.WriteLine("Edad:");
            mascota.Edad = int.Parse(Console.ReadLine());

            archivos.Serializar("mascotas.bin", mascota);
        }

        private static void Ejercicio2()
        {
            Console.WriteLine("Mostrar mascotas");
            var lista = archivos.Deserializar<Mascota>("mascotas.bin");
            if (lista == null)
            {
                lista = new List<Mascota>();
            }
            foreach (var item in lista)
            {
                Console.WriteLine($"Nombre: {item.Nombre}");
                Console.WriteLine($"Especie: {item.Especie}");
                Console.WriteLine($"Raza: {item.Raza}");
                Console.WriteLine($"Sexo: {item.Sexo}");
                Console.WriteLine($"Edad: {item.Edad}");
                Console.WriteLine("=======================================================");
            }
        }

        #region Serializar
        public static bool serializarD(Dictionary<string, alumno> dAlumnos)
        {
            try
            {
                FileStream fs = new FileStream(N_ARCH, FileMode.Create, FileAccess.Write);
                formatter.Serialize(fs, dAlumnos);
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region Deserializar
        public static bool DeserializarD()
        {
            try
            {
                FileStream fs = new FileStream(N_ARCH, FileMode.Open, FileAccess.Read);
                dAlumnos = (Dictionary<string, alumno>)formatter.Deserialize(fs);
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        public static void Ejercicio3()
        {
            if (File.Exists(N_ARCH))
            {
                DeserializarD();
            }
            else
            {
                serializarD(dAlumnos);
            }
            int op;
            do
            {
                Console.Clear();
                Console.WriteLine("===== MENU =====");
                Console.WriteLine(" 1 - Agregar alumno.");
                Console.WriteLine(" 2 - Mostrar alumnos.");
                Console.WriteLine(" 3 - Buscar alumno.");
                Console.WriteLine(" 4 - Editar alumno.(P)");
                Console.WriteLine(" 5 - Eliminar alumno");
                Console.WriteLine(" 6 - Salir");
                op = Convert.ToInt32(Console.ReadLine());

                switch (op)
                {
                    case 1:
                        #region Opccion 1
                        Console.Clear();
                        Console.WriteLine("===== AGREGAR =====");
                        alumno alumn = new alumno();
                        do
                        {
                            Console.WriteLine("Carnet: ");
                            alumn.carnet = Console.ReadLine();
                            if (dAlumnos.ContainsKey(alumn.carnet))
                            {
                                Console.WriteLine("Carnet existente en el registro.");
                            }
                        } while (dAlumnos.ContainsKey(alumn.carnet));
                        Console.WriteLine("Nombre: ");
                        alumn.nombre = Console.ReadLine();
                        Console.WriteLine("Carrera: ");
                        alumn.carrera = Console.ReadLine();
                        Console.WriteLine("CUM: ");
                        alumn.serCum(Convert.ToDecimal(Console.ReadLine()));
                        dAlumnos.Add(alumn.carnet, alumn);
                        serializarD(dAlumnos);
                        Console.WriteLine("Datos almacenados Correctamente");
                        Console.WriteLine("\nPresione <ENTER> para continuar.");
                        Console.ReadKey();
                        #endregion
                        break;
                    case 2:
                        #region Opccion 2
                        try
                        {
                            Console.WriteLine("======================== LISTADO DE ALUMNOS =============================");
                            Console.WriteLine("{0, 10} {1,-25} {2,-30} {3,5}", "Carnet: ", "Nombre: ", "Carrera: ", "CUM: ");
                            Console.WriteLine("=========================================================================");
                            DeserializarD();
                            foreach (KeyValuePair<string, alumno> alumnoG in dAlumnos)
                            {
                                alumno alumns = alumnoG.Value;
                                Console.WriteLine("{0,10} {1,-25} {2,-30} {3,5}",
                                alumns.carnet, alumns.nombre, alumns.carrera, alumns.getCum());
                            }
                            Console.WriteLine("=========================================================================");
                            Console.WriteLine("\nPresione <ENTER> para continuar.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            throw;
                        }
                        Console.ReadKey();
                        #endregion
                        break;
                    case 3:
                        #region Opccion 3
                        string carnetBusc;
                        Console.WriteLine("Ingrese carnet asignado al alumno que desea buscar:");
                        carnetBusc = Console.ReadLine();
                        if (dAlumnos.ContainsKey(carnetBusc))
                        {
                            Console.WriteLine("===== DATOS DE ALUMNO =====");
                            Console.WriteLine("{0, 10} {1,-25} {2,-30} {3,5}", "Carnet: ", "Nombre: ", "Carrera: ", "CUM: ");
                            Console.WriteLine("=========================================================================");
                            DeserializarD();
                            Console.WriteLine("{0,10} {1,-25} {2,-30} {3,5}",
                                dAlumnos[carnetBusc].carnet, dAlumnos[carnetBusc].nombre, dAlumnos[carnetBusc].carrera, dAlumnos[carnetBusc].getCum());
                        }
                        else
                        {
                            Console.WriteLine("El carnet: " + carnetBusc + " NO esta registrado.");
                        }
                        Console.WriteLine("\nPresione <ENTER> para continuar.");
                        Console.ReadKey();
                        #endregion
                        break;
                    case 4:
                        #region Opccion 4
                        string cMod;
                        Console.WriteLine("Ingrese carnet asignado al alumno que desea modificar:");
                        cMod = Console.ReadLine();
                        if (dAlumnos.ContainsKey(cMod))
                        {
                            Console.WriteLine("===== MODIFICAR DATOS DE ALUMNO =====");
                            dAlumnos.Remove(cMod);
                            Console.WriteLine("===== MODIFICAR =====");
                            Console.WriteLine("INGRESE LOS NUEVOS DATOS DEL ALUMNO " + cMod + " :");
                            alumno alumnN = new alumno();
                            do
                            {
                                Console.WriteLine("Carnet: ");
                                alumnN.carnet = Console.ReadLine();
                                if (dAlumnos.ContainsKey(alumnN.carnet))
                                {
                                    Console.WriteLine("Carnet existente en el registro.");
                                }
                            } while (dAlumnos.ContainsKey(alumnN.carnet));
                            Console.WriteLine("Nombre: ");
                            alumnN.nombre = Console.ReadLine();
                            Console.WriteLine("Carrera: ");
                            alumnN.carrera = Console.ReadLine();
                            Console.WriteLine("CUM: ");
                            alumnN.serCum(Convert.ToDecimal(Console.ReadLine()));
                            dAlumnos.Add(alumnN.carnet, alumnN);
                            serializarD(dAlumnos);
                            Console.WriteLine("Datos almacenados Correctamente");
                            Console.WriteLine("\nPresione <ENTER> para continuar.");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("El carnet: " + cMod + " NO esta registrado.");
                        }
                        #endregion
                        break;
                    case 5:
                        #region Opccion 5
                        string cElim;
                        Console.WriteLine("Ingrese carnet asignado al alumno que desea modificar:");
                        cElim = Console.ReadLine();
                        if (dAlumnos.ContainsKey(cElim))
                        {
                            dAlumnos.Remove(cElim);
                        }
                        serializarD(dAlumnos);
                        #endregion
                        break;
                }
            } while (op != 6);
        }
    }
}
