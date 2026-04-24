using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace SFCH
{
    public partial class App : Application
    {
        public App()
        {
            // Errores en el hilo de UI
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Errores en otros hilos
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ManejarError(e.Exception);
            e.Handled = true; // evita que la app se cierre
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            ManejarError(ex);
        }

        private void ManejarError(Exception ex)
        {
            try
            {
                // Guardar error en archivo
                File.AppendAllText("errores.log",
                    DateTime.Now + "\n" + ex.ToString() + "\n\n");

                // Mensaje amigable
                MessageBox.Show("Ocurrió un error inesperado. Contacte al administrador.",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                MessageBox.Show("Error crítico en la aplicación.",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
