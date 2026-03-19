using MaterialDesignThemes.Wpf;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Printing;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Runtime.InteropServices.JavaScript.JSType;
using PrintDialog = System.Windows.Controls.PrintDialog;

namespace SFCH.Controller
{

    public static class Util
    {
        public static string RutaEjecutable()
        {
            // Obtiene la ruta completa del ejecutable en ejecución string path = Assembly.GetExecutingAssembly().Location; return path;
            string path = Assembly.GetExecutingAssembly().Location;
            return path;
        }
        public static void RestoreDatabase(DbContext context, string rutaBak, string nuevoNombreBD)
        {
            try
            {
                var databaseName = context.Database.GetDbConnection().Database;

                // Forzar conexión a master
                context.Database.ExecuteSqlRaw("USE master");

                string sql = $@"
                ALTER DATABASE [{nuevoNombreBD}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

                RESTORE DATABASE [{nuevoNombreBD}]
                FROM DISK = N'{rutaBak}'
                WITH REPLACE;

                ALTER DATABASE [{nuevoNombreBD}] SET MULTI_USER;
            ";

                context.Database.ExecuteSqlRaw(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al restaurar la base de datos: " + ex.Message);
            }
        }
        public static void BackupDatabase( DbContext context,   string rutaCarpeta, int diasAConservar = 1)
        {
            try
            {
                // Crear carpeta si no existe
                if (!Directory.Exists(rutaCarpeta))
                    Directory.CreateDirectory(rutaCarpeta);

                var databaseName = context.Database.GetDbConnection().Database;

                // Nombre del archivo
                string fileName = $"Backup_{databaseName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                string fullPath = Path.Combine(rutaCarpeta, fileName);

                // SQL Backup
                string sql = $@"
                BACKUP DATABASE [{databaseName}]
                TO DISK = N'{fullPath}'
                WITH INIT, 
                NAME = 'Full Backup of {databaseName}'";

                // Ejecutar backup
                context.Database.ExecuteSqlRaw(sql);

                // 🧹 Limpieza de backups viejos
                var archivos = Directory.GetFiles(rutaCarpeta, "*.bak");

                foreach (var archivo in archivos)
                {
                    var info = new FileInfo(archivo);

                    if (info.CreationTime < DateTime.Now.AddDays(-diasAConservar))
                    {
                        info.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en BackupDatabase: " + ex.Message);
            }
        }
        public static void BackupDatabase(DbContext context, string rutaCarpeta)
        {
            try
            {
                // Nombre de la base de datos actual
                var databaseName = context.Database.GetDbConnection().Database;

                // Crear nombre del archivo
                string fileName = $"Backup_{databaseName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                string fullPath = Path.Combine(rutaCarpeta, fileName);

                // Comando SQL
                string sql = $@"
                BACKUP DATABASE [{databaseName}]
                TO DISK = N'{fullPath}'
                WITH INIT, 
                NAME = 'Full Backup of {databaseName}'";

                // Ejecutar usando Entity Framework
                context.Database.ExecuteSqlRaw(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al realizar el backup: " + ex.Message);
            }
        }
        public static async Task<string> HacerBackup(
       string servidor,
       string baseDeDatos,
       string usuario = null,
       string password = null,
       string rutaBackup = null)
        {
            try
            {
                // Crear cadena de conexión
                string connectionString;
                if (string.IsNullOrEmpty(usuario))
                {
                    // Windows Authentication
                    connectionString = $"Server={servidor};Trusted_Connection=True;";
                }
                else
                {
                    // SQL Authentication
                    connectionString = $"Server={servidor};User Id={usuario};Password={password};";
                }

                // Definir ruta de backup
                if (string.IsNullOrEmpty(rutaBackup))
                {
                    rutaBackup = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        $"Backup_{baseDeDatos}_{DateTime.Now:yyyyMMdd_HHmm}.bak"
                    );
                }

                // Crear comando SQL
                string sql = $@"
                BACKUP DATABASE [{baseDeDatos}]
                TO DISK = '{rutaBackup}'
                WITH FORMAT,
                     MEDIANAME = 'BackupSQL',
                     NAME = 'Backup completo de {baseDeDatos}';";

                // Ejecutar backup
                using (var conexion = new SqlConnection(connectionString))
                using (var comando = new SqlCommand(sql, conexion))
                {
                    await conexion.OpenAsync();
                    await comando.ExecuteNonQueryAsync();
                }

                return $"✅ Backup exitoso en: {rutaBackup}";
            }
            catch (Exception ex)
            {
                return $"❌ Error: {ex.Message}";
            }
        }

        public static DateTime TiempoEnRed()
        {
            const string ntpServer = "time.windows.com";
            var ntpData = new byte[48];
            ntpData[0] = 0x1B;

            var addresses = System.Net.Dns.GetHostEntry(ntpServer).AddressList;
            var ipEndPoint = new System.Net.IPEndPoint(addresses[0], 123);

            using (var socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
            }

            const byte serverReplyTime = 40;
            ulong intPart = BitConverter.ToUInt32(ntpData.Skip(serverReplyTime).Take(4).Reverse().ToArray(), 0);
            ulong fractPart = BitConverter.ToUInt32(ntpData.Skip(serverReplyTime + 4).Take(4).Reverse().ToArray(), 0);

            var milliseconds = (intPart * 1000 + (fractPart * 1000) / 0x100000000L);
            var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

            return networkDateTime.ToLocalTime();
        }

        public static string ConvertirNumeroALetras(decimal numero)
        {
            string[] unidades = { "", "Un", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Ocho", "Nueve" };
            string[] decenas = { "", "Diez", "Veinte", "Treinta", "Cuarenta", "Cincuenta", "Sesenta", "Setenta", "Ochenta", "Noventa" };
            string[] especiales = { "Once", "Doce", "Trece", "Catorce", "Quince", "Dieciséis", "Diecisiete", "Dieciocho", "Diecinueve" };
            string[] centenas = { "", "Ciento", "Doscientos", "Trescientos", "Cuatrocientos", "Quinientos", "Seiscientos", "Setecientos", "Ochocientos", "Novecientos" };

            int parteEntera = (int)Math.Floor(numero);
            int centavos = (int)Math.Round((numero - parteEntera) * 100);

            if (parteEntera == 0)
                return "Cero " + centavos.ToString("00") + "/100";

            string resultado = "";

            if (parteEntera == 1000000)
                return "Un Millón " + centavos.ToString("00") + "/100";

            if (parteEntera >= 1000000)
            {
                int millones = parteEntera / 1000000;
                resultado += ConvertirGrupo(millones) + (millones == 1 ? " Millón " : " Millones ");
                parteEntera %= 1000000;
            }

            if (parteEntera >= 1000)
            {
                int miles = parteEntera / 1000;
                resultado += ConvertirGrupo(miles) + " Mil ";
                parteEntera %= 1000;
            }

            resultado += ConvertirGrupo(parteEntera);
            resultado = resultado.Trim() + " con " + centavos.ToString("00") + "/100";
            return resultado;
        }

        public static string ConvertirGrupo(int numero)
        {
            string[] unidades = { "", "Un", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Ocho", "Nueve" };
            string[] decenas = { "", "Diez", "Veinte", "Treinta", "Cuarenta", "Cincuenta", "Sesenta", "Setenta", "Ochenta", "Noventa" };
            string[] especiales = { "Once", "Doce", "Trece", "Catorce", "Quince", "Dieciséis", "Diecisiete", "Dieciocho", "Diecinueve" };
            string[] centenas = { "", "Ciento", "Doscientos", "Trescientos", "Cuatrocientos", "Quinientos", "Seiscientos", "Setecientos", "Ochocientos", "Novecientos" };

            if (numero == 0) return "";
            if (numero == 100) return "Cien";

            string resultado = "";

            if (numero >= 100)
            {
                resultado += centenas[numero / 100] + " ";
                numero %= 100;
            }

            if (numero >= 11 && numero <= 19)
            {
                resultado += especiales[numero - 11];
                return resultado;
            }

            if (numero >= 10)
            {
                resultado += decenas[numero / 10];
                if (numero % 10 > 0)
                {
                    resultado += numero / 10 == 2 ? "i" : " y ";
                    resultado += unidades[numero % 10].ToLower();
                }
            }
            else if (numero > 0)
            {
                resultado += unidades[numero];
            }

            return resultado.Trim();
        }

        // Método para hashear una contraseña
        public static string HashPassword(string password, int saltSize = 16, int iterations = 10000, int hashSize = 20)
        {
            // Genera una sal criptográficamente segura
            byte[] salt = new byte[saltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Deriva una clave de la contraseña usando PBKDF2
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                byte[] hash = pbkdf2.GetBytes(hashSize);

                // Combina sal y hash
                byte[] hashBytes = new byte[saltSize + hashSize];
                Array.Copy(salt, 0, hashBytes, 0, saltSize);
                Array.Copy(hash, 0, hashBytes, saltSize, hashSize);

                // Convierte a base64 para almacenar como texto
                string hashedPassword = Convert.ToBase64String(hashBytes);

                return hashedPassword;
            }
        }

        // Método para verificar la contraseña proporcionada por el usuario contra el hash almacenado
        public static bool VerifyPassword(string password, string hashedPassword, int iterations = 10000, int hashSize = 20)
        {
            try
            {
                // Convierte el hash de base64 a bytes
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);

                // Obtén la sal de los primeros bytes
                byte[] salt = new byte[hashBytes.Length - hashSize];
                Array.Copy(hashBytes, 0, salt, 0, salt.Length);

                // Deriva la clave con la contraseña proporcionada
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    byte[] hash = pbkdf2.GetBytes(hashSize);

                    // Compara el hash derivado con el hash almacenado
                    for (int i = 0; i < hashSize; i++)
                    {
                        if (hashBytes[i + salt.Length] != hash[i])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public enum TipoIdentificacion
        {
            Cedula,
            RNC,
            Invalido
        }

        public static TipoIdentificacion ValidarIdentificacionDominicana(string numero)
        {
            // Limpiar y normalizar el número
            string numeroLimpio = Regex.Replace(numero, @"\D", "");

            // Primero validar cédula (11 dígitos)
            if (numeroLimpio.Length == 11 && ValidarCedula(numeroLimpio))
            {
                return TipoIdentificacion.Cedula;
            }

            // Luego validar RNC (9 u 11 dígitos)
            if ((numeroLimpio.Length == 9 || numeroLimpio.Length == 11) && ValidarRnc(numeroLimpio))
            {
                return TipoIdentificacion.RNC;
            }

            return TipoIdentificacion.Invalido;
        }

        public static List<string> CargarImpresoras()
        {
            var impresoras = new List<string>();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                impresoras.Add(printer);
            }

            return impresoras;
        }

        public static List<string> CargarImpresoras(System.Windows.Controls.ComboBox comboBox)
        {
            var impresoras = new List<string>();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                comboBox.Items.Add(printer);
            }
            comboBox.SelectedIndex = 0; // Selecciona la primera impresora por defecto
            return impresoras;
        }

        private static bool ValidarCedula(string cedula)
        {
            // Validación cédula (mismo algoritmo anterior)
            if (cedula.Length != 11) return false;

            int codigoProvincia = int.Parse(cedula.Substring(0, 3));
            if (!((codigoProvincia >= 1 && codigoProvincia <= 32) ||
                (codigoProvincia >= 401 && codigoProvincia <= 431))) return false;

            int suma = 0;
            for (int i = 0; i < 10; i++)
            {
                int digito = int.Parse(cedula[i].ToString());
                int multiplicador = (i % 2 == 0) ? 1 : 2;
                suma += (digito * multiplicador) > 9 ? (digito * multiplicador) - 9 : digito * multiplicador;
            }

            int digitoVerificador = (10 - (suma % 10)) % 10;
            return digitoVerificador == int.Parse(cedula[10].ToString());
        }

        private static bool ValidarRnc(string rnc)
        {
            // Validación RNC (mismo algoritmo anterior)
            if (rnc.Length != 9 && rnc.Length != 11) return false;

            return rnc.Length switch
            {
                9 => ValidarRnc9Digitos(rnc),
                11 => ValidarRnc11Digitos(rnc),
                _ => false
            };
        }

        public static bool ValidarCedulaDominicana(string cedula)
        {
            // Limpiar la cédula removiendo todos los caracteres no numéricos
            string cedulaLimpia = Regex.Replace(cedula, @"\D", "");

            // Verificar longitud y que sea numérica
            if (cedulaLimpia.Length != 11 || !cedulaLimpia.All(char.IsDigit))
                return false;

            // Validar código de provincia
            int codigoProvincia = int.Parse(cedulaLimpia.Substring(0, 3));
            bool provinciaValida = (codigoProvincia >= 1 && codigoProvincia <= 32) ||
                                  (codigoProvincia >= 401 && codigoProvincia <= 431);
            if (!provinciaValida)
                return false;

            // Algoritmo de validación del dígito verificador
            int suma = 0;
            for (int i = 0; i < 10; i++)
            {
                int digito = int.Parse(cedulaLimpia[i].ToString());
                int multiplicador = (i % 2 == 0) ? 1 : 2;
                suma += (digito * multiplicador) switch
                {
                    > 9 => digito * multiplicador - 9,
                    _ => digito * multiplicador
                };
            }

            int digitoVerificadorCalculado = (10 - (suma % 10)) % 10;
            int digitoVerificadorReal = int.Parse(cedulaLimpia[10].ToString());

            return digitoVerificadorCalculado == digitoVerificadorReal;
        }

        public static bool ValidarRncDominicano(string rnc)
        {
            // Limpiar y normalizar el RNC
            string rncLimpio = Regex.Replace(rnc, @"\D", "");

            // Verificar longitud válida (9 o 11 dígitos)
            if (rncLimpio.Length != 9 && rncLimpio.Length != 11)
                return false;

            // Validar según el tipo de RNC
            return rncLimpio.Length switch
            {
                9 => ValidarRnc9Digitos(rncLimpio),
                11 => ValidarRnc11Digitos(rncLimpio),
                _ => false
            };
        }

        private static bool ValidarRnc9Digitos(string rnc)
        {
            // Validación para RNC antiguos (9 dígitos)
            int[] digitos = rnc.Select(c => int.Parse(c.ToString())).ToArray();

            // Calcular suma ponderada
            int[] pesos = { 8, 9, 10, 11, 12, 13, 14, 15 };
            int suma = 0;

            for (int i = 0; i < 8; i++)
            {
                suma += digitos[i] * pesos[i];
            }

            // Calcular dígito verificador
            int digitoVerificador = suma % 11;
            digitoVerificador = digitoVerificador == 10 ? 0 : digitoVerificador;

            return digitoVerificador == digitos[8];
        }

        private static bool ValidarRnc11Digitos(string rnc)
        {
            // Validación para RNC nuevos (11 dígitos)
            int[] digitos = rnc.Select(c => int.Parse(c.ToString())).ToArray();

            // Pesos oficiales DGII para RNC de 11 dígitos
            int[] pesos = { 7, 9, 8, 6, 5, 4, 3, 2, 7, 1 };
            int suma = 0;

            for (int i = 0; i < 10; i++)
            {
                suma += digitos[i] * pesos[i];
            }

            // Calcular dígito verificador
            int digitoVerificador = (11 - (suma % 11)) % 11;
            digitoVerificador = digitoVerificador == 10 ? 0 : digitoVerificador;

            return digitoVerificador == digitos[10];
        }

        public enum TipoPlaca
        {
            Valida,
            Invalida
        }

        public static (bool valida, TipoPlaca? tipo) ValidarPlacaDominicana(string placa)
        {
            // Normalizar: quitar espacios y convertir a mayúsculas
            string placaLimpia = Regex.Replace(placa.ToUpper(), @"\s", "");

            // Patrones oficiales actualizados 2023
            var patrones = new Dictionary<Regex, TipoPlaca>
            {
                // Vehículos particulares (nuevo formato: 2 letras - 4 números)
                { new Regex(@"^[A-Z]{2}-\d{4}$"), TipoPlaca.Valida },

                // Vehículos particulares (formato antiguo: 3 letras - 3 números)
                { new Regex(@"^[A-Z]{3}-\d{3}$"), TipoPlaca.Valida },

                // Transporte público (T - 3 letras - 3 números)
                { new Regex(@"^T-[A-Z]{3}-\d{3}$"), TipoPlaca.Valida },

                // Vehículos gubernamentales (G - 4 números)
                { new Regex(@"^G-\d{4}$"), TipoPlaca.Valida },

                // Motocicletas (M - 3 letras - 3 números)
                { new Regex(@"^M-[A-Z]{3}-\d{3}$"), TipoPlaca.Valida },

                // Placas diplomáticas (CD - 3 números)
                { new Regex(@"^CD-\d{3}$"), TipoPlaca.Valida },

                // Vehículos especiales/dealer (E - 3 números - 3 letras)
                { new Regex(@"^E-\d{3}-[A-Z]{3}$"), TipoPlaca.Valida }
            };

            foreach (var patron in patrones)
            {
                if (patron.Key.IsMatch(placaLimpia))
                {
                    return (true, patron.Value);
                }
            }

            return (false, TipoPlaca.Invalida);
        }

        public static BitmapImage LoadImage(byte[] imageData)
        {
            try
            {
                using (var ms = new System.IO.MemoryStream(imageData))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            catch (Exception ex)
            {
                return new BitmapImage();
            }
        }
        public static byte[] ConvertImageToByteArray(BitmapImage image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
        public static byte[] CargarImagen()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Todos los archivos|*.*";
                openFileDialog.Title = "Seleccionar imagen";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        return File.ReadAllBytes(openFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return Array.Empty<byte>();
                    }
                }
                return Array.Empty<byte>();
            }
        }
        public static bool GuardarImagenEnCarpetaSeleccionada(System.Windows.Media.Imaging.BitmapImage image, out string savedPath)
        {
            savedPath = string.Empty;
            if (image == null) return false;

            try
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    fbd.Description = "Seleccione la carpeta donde guardar la imagen";
                    fbd.ShowNewFolderButton = true;

                    var result = fbd.ShowDialog();
                    if (result != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath))
                        return false;

                    string carpetaDestino = fbd.SelectedPath;
                    // Asegurar carpeta existe
                    Directory.CreateDirectory(carpetaDestino);

                    // Convertir la imagen a bytes (método existente en la clase Util)
                    byte[] bytes = ConvertImageToByteArray(image);

                    // Generar nombre de archivo único
                    string fileName = $"imagen_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.png";
                    string rutaDestino = Path.Combine(carpetaDestino, fileName);

                    File.WriteAllBytes(rutaDestino, bytes);
                    savedPath = rutaDestino;
                    return true;
                }
            }
            catch
            {
                savedPath = string.Empty;
                return false;
            }
        }

        public static string GuardarImagenEnCarpetaSeleccionada(out string savedPath)
        {
            savedPath = string.Empty;
            try
            {
                using (var ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Title = "Seleccionar imagen para copiar a carpeta predeterminada";
                    ofd.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Todos los archivos|*.*";
                    ofd.Multiselect = false;

                    var result = ofd.ShowDialog();
                    if (result != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(ofd.FileName))
                        return "";

                    string archivoSeleccionado = ofd.FileName;
                    string nombreArchivoOriginal = Path.GetFileName(archivoSeleccionado);
                    // Carpeta predeterminada: Mis Imágenes\SFCH_Images
                    string carpetaPredeterminada = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "SFCH_Images");
                    Directory.CreateDirectory(carpetaPredeterminada);

                    string rutaDestino = GetUniqueFilePath(carpetaPredeterminada, nombreArchivoOriginal);

                    File.Copy(archivoSeleccionado, rutaDestino);
                    savedPath = rutaDestino;
                    return rutaDestino;
                }
            }
            catch
            {
                savedPath = string.Empty;
                return savedPath;
            }
        }

        private static string GetUniqueFilePath(string directory, string filename)
        {
            string fullPath = Path.Combine(directory, filename);
            if (!File.Exists(fullPath)) return fullPath;

            string nameWithoutExt = Path.GetFileNameWithoutExtension(filename);
            string ext = Path.GetExtension(filename);
            int counter = 1;

            string candidate;
            do
            {
                candidate = Path.Combine(directory, $"{nameWithoutExt}_{counter}{ext}");
                counter++;
            } while (File.Exists(candidate));

            return candidate;
        }

        public  static string DescargarImagenDeInternet(string url)
        {
            string carpetaPredeterminada = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), 
                "SFCH_Images");
            
            return DescargarImagenDeInternet(url, carpetaPredeterminada);
        }

        public static string DescargarImagenDeInternet(string url, string carpetaDestino)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                System.Windows.MessageBox.Show("La URL de la imagen no puede estar vacía", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }

            try
            {
                // Asegurar que la carpeta existe
                Directory.CreateDirectory(carpetaDestino);

                // Descargar la imagen
                byte[] imageData;
                using (var client = new System.Net.Http.HttpClient())
                {
                    imageData = client.GetByteArrayAsync(url).Result;
                }

                // Generar nombre único para el archivo
                string nombreArchivo = $"imagen_{DateTime.Now:dd_MM_yyyy}_{Guid.NewGuid():N}.png";
                string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);

                // Guardar la imagen en disco
                File.WriteAllBytes(rutaDestino, imageData);

                // Convertir bytes a BitmapImage y retornar
                return rutaDestino;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al descargar la imagen: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }
        }
    }
}

