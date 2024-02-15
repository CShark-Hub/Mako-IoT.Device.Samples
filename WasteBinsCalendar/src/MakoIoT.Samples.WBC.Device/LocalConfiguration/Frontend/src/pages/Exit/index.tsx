import { FunctionComponent } from "preact";
import { useState, useEffect } from "preact/hooks";
import Spinner from "../../components/Spinner";
import { useAlert } from "../../components/AlertContext";
import { exit } from "../../utils/api";
import { useAppConfig } from "../../components/ConfigContext";
import useLocalize from "../../utils/useLocalize ";

interface ConfigProps {
  // You can define props if needed, for example, for initial data or API functions
}

const Exit: FunctionComponent<ConfigProps> = () => {
  const { showAlert, hideAlert } = useAlert();
  const [loading, setLoading] = useState(true);
  const config = useAppConfig();
  const localize = useLocalize();

  useEffect(() => {
    hideAlert();
    exit(config)
      .then((data) => {
        setLoading(false); 
		showAlert('success', localize('exit.success'));       
      })
      .catch((error) => {
        console.error("Failed to fetch data:", error);
        showAlert('danger', localize('exit.error'));
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
