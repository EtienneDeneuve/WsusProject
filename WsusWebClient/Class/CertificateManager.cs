using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;


namespace GetWsusGroup.Class
{
    internal static class CertificateManager
    {
        static readonly string thumbprint = ConfigurationManager.AppSettings["CertificateThumbprint"].ToString();
        static readonly string certificatefile = ConfigurationManager.AppSettings["CertificatePath"].ToString();
        static readonly string certificatename = ConfigurationManager.AppSettings["CertificateFriendlyName"].ToString();

        internal static bool FindCerticate()
        {
            try
            {
                X509Store store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                if (certCollection.Count != 0)
                {
                    foreach (var certificate in certCollection)
                    {
                        if (certificate.FriendlyName == certificatename)
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception e)
            {
                var msg = $"Certificate Lookup => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }

        }
        internal static void InstallCerticate(string certificatefile)
        {
            try
            {
                var find = FindCerticate();

                if (find == false)
                {
                    X509Store store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(new X509Certificate2(X509Certificate2.CreateFromCertFile(certificatefile)));
                    store.Close();
                    File.Delete(certificatefile);
                }
            }
            catch (Exception e)
            {
                var msg = $"Certificate Install => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }
        }

    }

}

