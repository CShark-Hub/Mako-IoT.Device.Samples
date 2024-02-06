import { FunctionComponent } from "preact";
import { useState, useEffect } from "preact/hooks";
import Spinner from "../../components/Spinner";
import { useAlert } from "../../components/AlertContext";
import { exit } from "../../utils/api";

interface ConfigProps {
  // You can define props if needed, for example, for initial data or API functions
}

const Exit: FunctionComponent<ConfigProps> = () => {
  const { showAlert, hideAlert } = useAlert();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Fetch data when the component mounts
    hideAlert();
    exit()
      .then((data) => {
        setLoading(false); 
		showAlert('success', 'The device will now exit configuration mode and reboot. To enter configuration mode again, press configuration button on the device.');       
      })
      .catch((error) => {
        console.error("Failed to fetch data:", error);
        showAlert('danger', 'Error exitign confguration mode.');
        setLoading(false);
      });
  }, []);
  return (
    <div className="container mt-5">
      {loading && <Spinner />}
    </div>
  );
};

export default Exit;
