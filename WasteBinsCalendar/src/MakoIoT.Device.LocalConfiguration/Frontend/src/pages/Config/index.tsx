import { FunctionComponent } from "preact";
import { useState, useEffect } from "preact/hooks";
import WiFiSection from "../../components/Form/WiFiSection";
import CalendarSection from "../../components/Form/CalendarSection";
import BinNamesSection from "../../components/Form/BinNamesSection";
import { submitFormData, fetchData, uploadCertificate } from "../../utils/api";
import { useAlert } from "../../components/AlertContext";
import Spinner from "../../components/Spinner";

interface ConfigProps {
  // You can define props if needed, for example, for initial data or API functions
}

const Config: FunctionComponent<ConfigProps> = () => {
  const [wifiSettings, setWifiSettings] = useState({ ssid: "", password: "" });
  const [calendarSettings, setCalendarSettings] = useState({
    url: "",
    timeZone: "",
  });
  const [binNames, setBinNames] = useState({
    white: "",
    brown: "",
    yellow: "",
    green: "",
    blue: "",
    red: "",
  });

  const { showAlert, hideAlert } = useAlert();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Fetch data when the component mounts
    hideAlert();
    fetchData()
      .then((data) => {
        // Assuming 'data' has the shape { wifiSettings, calendarSettings, binNames }
        setWifiSettings(data.wifiSettings);
        setCalendarSettings(data.calendarSettings);
        setBinNames(data.binNames);
        setLoading(false);        
      })
      .catch((error) => {
        console.error("Failed to fetch data:", error);
        showAlert('danger', 'Error loading settings.');
        setLoading(false);
      });
  }, []);

  const handleWiFiChange = (key: keyof typeof wifiSettings, value: string) => {
    setWifiSettings({ ...wifiSettings, [key]: value });
  };

  const handleCalendarChange = (
    key: keyof typeof calendarSettings,
    value: string
  ) => {
    setCalendarSettings({ ...calendarSettings, [key]: value });
  };

  const handleBinNameChange = (color: string, value: string) => {
    setBinNames({ ...binNames, [color]: value });
  };

  const handleSubmit = async (event: Event) => {
    event.preventDefault();
    hideAlert();
    setLoading(true);
    // Process the form data, e.g., send to an API
    try {
      const result = await submitFormData(
        wifiSettings,
        calendarSettings,
        binNames
      );
      console.log("Form submission result:", result);
      showAlert('success', 'Settings updated successfully!');
    } catch (error) {
      console.error("There was a problem submitting the form:", error);
      showAlert('danger', 'Error updating settings.');
    }
    setLoading(false);
    console.log("Form Submitted", { wifiSettings, calendarSettings, binNames });
  };

  return (
    <div className="container mt-5">
      {loading && <Spinner />}
      <h1 className="mb-4">Configuration Settings</h1>
      <form>
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
        <button type="button" className="btn btn-primary mt-3" onClick={handleSubmit}>
          Update Settings
        </button>
      </form>      
    </div>
  );
};

export default Config;
