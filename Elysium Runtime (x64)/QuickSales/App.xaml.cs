using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using RMSDataAccessLayer;

namespace QuickSales
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public App()
        //{
        //    this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        //    Bootstrapper bootstrapper = new Bootstrapper();
        //    bootstrapper.Run();
        //}

        private bool Authenticate(string user, string pass)
        {
            RMSModel db = new RMSModel();
            var cashier = (from c in db.Persons.OfType<Cashier>()
                           where c.LoginName == user
                           select c).FirstOrDefault();

            if (cashier != null && cashier.SPassword == pass)
            {
                CashierLog log = db.CreateObject<CashierLog>();
                log.PersonId = cashier.Id;
                log.MachineName = Environment.MachineName;
                log.LoginTime = DateTime.Now;
                log.Status = "LogIn";
                //db = null;
                db.CashierLogs.AddObject(log);
                db.SaveChanges();
                db.Dispose();
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Creates a new instance of <c>App</c>
        /// </summary>
        public App()
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            LoginRoutine();
        }

        public  void LoginRoutine()
        {
            LogInScreen logon = new LogInScreen();
#if DEBUG
            logon.HintVisible = true;
#endif
            bool? res = logon.ShowDialog();
            if (!res ?? true)
            {
                Shutdown(1);
            }
            else 
                if (Authenticate(logon.UserName, logon.Password))
            {
                //StartupContainer();
                LogInScreen logon1 = new LogInScreen();
                logon1.Show();
                logon1.ShowOptions();
            }
            else
            {
                MessageBox.Show(
                    "Application is exiting due to invalid credentials",
                    "Application Exit",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Shutdown(1);
            }
        }

        private void StartupContainer()
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            
            Application.Current.MainWindow = null;

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Bootstrapper bootStrapper = new Bootstrapper();
            bootStrapper.Run();

           
        }
    }
}
