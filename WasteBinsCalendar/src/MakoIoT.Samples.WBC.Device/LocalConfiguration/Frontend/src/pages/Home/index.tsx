import { useEffect } from "preact/hooks";
import { useAlert } from "../../components/AlertContext";
import useLocalize from "../../utils/useLocalize ";

export function Home() {
	const { showAlert, hideAlert } = useAlert();
	const localize = useLocalize();

	useEffect(() => {
		// Fetch data when the component mounts
		hideAlert();
	  }, []);

  return <div className="container mt-5">
	<h1>{localize('home.header')}</h1>
	<p>{localize('home.p1')}</p>
	<p>{localize('home.p2')}</p>
	<p>{localize('home.p3')}</p>
  </div>;
}
