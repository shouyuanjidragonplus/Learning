using UnityEngine.Networking;

namespace GameMain.Asset
{
    public class DownloadCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}

