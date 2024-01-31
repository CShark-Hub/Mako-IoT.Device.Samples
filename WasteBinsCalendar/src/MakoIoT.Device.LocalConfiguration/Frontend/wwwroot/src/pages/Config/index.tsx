import { h, FunctionComponent } from "preact";
import { useState, useEffect } from "preact/hooks";
import WiFiSection from "../../components/Form/WiFiSection";
import CalendarSection from "../../components/Form/CalendarSection";
import BinNamesSection from "../../components/Form/BinNamesSection";
import { submitFormData, fetchData } from '../../utils/api';

interface ConfigProps {
  // You can define props if needed, for example, for initial data or API functions
}

const Config: FunctionComponent<ConfigProps> = () => {
  const [wifiSettings, setWifiSettings] = useState({ ssid: "", password: "" });
  const [calendarSettings, setCalendarSettings] = useState({
    url: "",
    timeZone: "",
    httpsCertificate: null,
  });
  const [binNames, setBinNames] = useState({
    white: "",
    brown: "",
    yellow: "",
    green: "",
    blue: "",
    red: "",
  });

  useEffect(() => {
    // Fetch data when the component mounts
    fetchData().then(data => {
        // Assuming 'data' has the shape { wifiSettings, calendarSettings, binNames }
        setWifiSettings(data.wifiSettings);
        setCalendarSettings(data.calendarSettings);
        setBinNames(data.binNames);
    }).catch(error => {
        console.error("Failed to fetch data:", error);
    });
  }, []);

  const handleWiFiChange = (key: keyof typeof wifiSettings, value: string) => {
    setWifiSettings({ ...wifiSettings, [key]: value });
  };

  const handleCalendarChange = (
    key: keyof typeof calendarSettings,
    value: string | File
  ) => {
    setCalendarSettings({ ...calendarSettings, [key]: value });
  };

  const handleBinNameChange = (color: string, value: string) => {
    setBinNames({ ...binNames, [color]: value });
  };

  const handleSubmit = async (event: Event) => {
    event.preventDefault();
    // Process the form data, e.g., send to an API
	try {
        const result = await submitFormData(wifiSettings, calendarSettings, binNames);
        console.log('Form submission result:', result);
        // Optionally, show a success message or handle the response further
    } catch (error) {
        console.error('There was a problem submitting the form:', error);
        // Optionally, show an error message
    }

    console.log("Form Submitted", { wifiSettings, calendarSettings, binNames });
  };

  return (
    <div className="container mt-5">
      <h1 className="mb-4">Configuration Settings</h1>
      <form onSubmit={handleSubmit}>
        <div className="row">
          <div className="col-md-6">
            <WiFiSection
              ssid={wifiSettings.ssid}
              password={wifiSettings.password}
              onSSIDChange={(value) => handleWiFiChange("ssid", value)}
              onPasswordChange={(value) => handleWiFiChange("password", value)}
            />
          </div>
        </div>
        <div className="row">
          <div className="col-md-6">
            <CalendarSection
              url={calendarSettings.url}
              timeZone={calendarSettings.timeZone}
              onURLChange={(value) => handleCalendarChange("url", value)}
              onTimeZoneChange={(value) =>
                handleCalendarChange("timeZone", value)
              }
              onCertificateChange={(file) =>
                handleCalendarChange("httpsCertificate", file)
              }
            />
          </div>
        </div>
        <div className="row">
          <div className="col-md-6">
            <BinNamesSection
              binNames={binNames}
              onBinNameChange={handleBinNameChange}
            />
          </div>
        </div>
        <button type="submit" className="btn btn-primary mt-3">
          Submit
        </button>
      </form>
    </div>
  );
};

export default Config;
