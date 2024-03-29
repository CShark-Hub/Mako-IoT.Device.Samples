﻿
using MakoIoT.Samples.WBC.Device.Configuration;
using nanoFramework.Json;
using nanoFramework.TestFramework;

namespace MakoIoT.Samples.WBC.Device.Tests
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void WasteBinsCalendarConfig_should_serialize()
        {
            var obj = new WasteBinsCalendarConfig
            {
                BinsNames = new()
                        {
                            { "zmieszane", "Black" },
                            { "BIO", "Brown" },
                            { "tworzywa", "Yellow" },
                            { "szkło", "Green" },
                            { "papier", "Blue" },
                            { "SZOP", "Red" }
                        },
                CalendarUrl = "https://somecalendar.com/api",
                ServiceCertificate = @"-----BEGIN CERTIFICATE-----
MIIEWDCCA0CgAwIBAgIPMR1+3eSSAgi72F73Jiu6MA0GCSqGSIb3DQEBCwUAMIGD
MQswCQYDVQQGEwJQTDEiMCAGA1UEChMZVW5pemV0byBUZWNobm9sb2dpZXMgUy5B
LjEnMCUGA1UECxMeQ2VydHVtIENlcnRpZmljYXRpb24gQXV0aG9yaXR5MScwJQYD
VQQDEx5DZXJ0dW0gR2xvYmFsIFNlcnZpY2VzIENBIFNIQTIwHhcNMTYwNjA4MDgw
MTI5WhcNMjYwNjA2MDgwMTI5WjA9MQswCQYDVQQGEwJQTDEVMBMGA1UECgwMaG9t
ZS5wbCBTLkEuMRcwFQYDVQQDDA5DZXJ0eWZpa2F0IFNTTDCCASIwDQYJKoZIhvcN
AQEBBQADggEPADCCAQoCggEBALDozq2WteNT2CsyO5CY9rCN4neWGyj+26XUYS5o
oM+tzFlMrbdNk3J+aI7P8Ab29qlSuPRgpHMi++AopeaOYsiwuVJZqnN2W1PC6huf
b9mVMv3mcxYLCmuEiY4k9N7L4SUMLxPrRlb7ksD/ogaEgMEYHf5QJyEdp2z4Qy2u
C4K/+zpyKpsBeihk5NGnoqTL4LmKNZWyGvJXwq3INuhd4hCpdMgEtX1Mo2TYa8bc
amo02VhHHX875TjxC/l3nEKXNcddLN3VvlnYGu7xvIwmCB0P9VDIB53MofqlN+lM
zcx8XelCSSWiJKFKXCv4bsvqJOTHuejmi6yDZF8icM8zdVsCAwEAAaOCAQwwggEI
MBIGA1UdEwEB/wQIMAYBAf8CAQAwMgYDVR0fBCswKTAnoCWgI4YhaHR0cDovL2Ny
bC5jZXJ0dW0ucGwvZ3NjYXNoYTIuY3JsMG4GCCsGAQUFBwEBBGIwYDAoBggrBgEF
BQcwAYYcaHR0cDovL3N1YmNhLm9jc3AtY2VydHVtLmNvbTA0BggrBgEFBQcwAoYo
aHR0cDovL3JlcG9zaXRvcnkuY2VydHVtLnBsL2dzY2FzaGEyLmNlcjAfBgNVHSME
GDAWgBRUmd2b/+inDqMZnVu+QlffMPyPMjAdBgNVHQ4EFgQUPZG2zBF76+RmEazS
0gfLqaSAczEwDgYDVR0PAQH/BAQDAgEGMA0GCSqGSIb3DQEBCwUAA4IBAQBBGurH
V6ciSTVqiNgltiQilglZfJcBo9MpMYDIFUnPvZKLjeMYYgbySA77cEyevma053Nm
leNWr9burXQqbZ8P9n4iJh3n20E+A6048bsH+0mMY2Xyc1AGy4pclIUNRlPNtYpA
H2jndvUoZP+4RwUjiGLEtX1jhORw4rZb6j+UXVWTBssj7Y+8GBp9MXl9ShIpb3Vb
DIRnxv2KcydLRRAb4pP3mw0rHWLmAZ3UY1Aj6aeeoWh5sKZbmHOpjelwaGyiojSm
dg0GZufYhBeYxQNXpdhWxDRIzAT890wqp77PXKTFMZaS0QaZz7k4o/K/VGJVWWkY
1xuL4Zl8WvEcVWEK
-----END CERTIFICATE-----",
                TimezoneString = "CET-1CEST,M3.5.0,M10.5.0/3"
            };

            var result = JsonSerializer.SerializeObject(obj);

            Assert.IsNotNull(result);
        }
    }
}
