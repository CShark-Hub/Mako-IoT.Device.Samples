import { FunctionComponent } from "preact";
import { useState, useEffect } from "preact/hooks";
import HttpsCertificateUpload from "../../components/Form/HttpsCertificateUpload";
import { uploadCertificate } from "../../utils/api";
import { useAlert } from "../../components/AlertContext";
import Spinner from "../../components/Spinner";
import { useAppConfig } from "../../components/ConfigContext";
import useLocalize from "../../utils/useLocalize ";

interface ConfigProps {
  // You can define props if needed, for example, for initial data or API functions
}

const Certificates: FunctionComponent<ConfigProps> = () => {
  const { showAlert, hideAlert } = useAlert();
  const [loading, setLoading] = useState(false);
  const config = useAppConfig();
  const localize = useLocalize();

  useEffect(() => {
    // Fetch data when the component mounts
    hideAlert();
  }, []);

  const handleCertificateUpload = (file: File) => {
    event.preventDefault();
    hideAlert();
    setLoading(true);
    // Function to call the API and upload the file
    uploadCertificate(file, config)
      .then((response) => {
        setLoading(false);
        console.log("Certificate uploaded successfully:", response);
        showAlert('success', localize('certificates.upload.success'));
      })
      .catch((error) => {
        setLoading(false);
        console.error("Error uploading certificate:", error);
        showAlert('danger', localize('certificates.upload.error'));
      });
      
  };

  return (
    <div className="container mt-5">
      {loading && <Spinner />}
      <h1 className="mb-4">{localize('certificates.header')}</h1>
      <p>{localize('certificates.p1')} <a href="https://www.instructables.com/How-to-Download-the-SSL-Certificate-From-a-Website/">https://www.instructables.com/How-to-Download-the-SSL-Certificate-From-a-Website</a>.</p>
      <p>{localize('certificates.p2')}</p>
      <form>
        <div className="row">
          <div className="col-md-6">
            <HttpsCertificateUpload onUpload={handleCertificateUpload} />
          </div>
        </div>
      </form>
    </div>
  );
};

export default Certificates;
