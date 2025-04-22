using UnityEngine;
using UnityEngine.Networking;

// Clase que acepta todos los certificados SSL para desarrollo
public class ForceAcceptAll : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;    // Acepta cualquier certificado (solo para desarrollo)
    }
}