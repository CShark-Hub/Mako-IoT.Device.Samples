import { useEffect } from "preact/hooks";
import { useAlert } from "../../components/AlertContext";

export function Home() {
	const { showAlert, hideAlert } = useAlert();

	useEffect(() => {
		// Fetch data when the component mounts
		hideAlert();
	  }, []);

  return <div className="container mt-5">
	<h1>Device configuration mode</h1>
	<p>Here you can configure your device's settings. Go to <a href="/configuration">Configuration</a> section and input your settings.</p>
	<p>Once you're done, click <a href="/exit">Exit</a> to return to normal operation mode.</p>
	<p>Note: While you are connected to this device's WiFi network (AP) you won't have internet access.</p>
  </div>;
}
