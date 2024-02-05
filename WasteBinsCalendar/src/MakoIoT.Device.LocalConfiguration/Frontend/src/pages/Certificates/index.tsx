import { FunctionComponent } from "preact";
import { useState, useEffect } from "preact/hooks";
import HttpsCertificateUpload from "../../components/Form/HttpsCertificateUpload";
import { uploadCertificate } from "../../utils/api";
import { useAlert } from "../../components/AlertContext";
import Spinner from "../../components/Spinner";

interface ConfigProps {
  // You can define props if needed, for example, for initial data or API functions
}

const Certificates: FunctionComponent<ConfigProps> = () => {
  const { showAlert, hideAlert } = useAlert();
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    // Fetch data when the component mounts
    hideAlert();
  }, []);

  const handleCertificateUpload = (file: File) => {
    event.preventDefault();
    hideAlert();
    setLoading(true);
    // Function to call the API and upload the file
    uploadCertificate(file)
      .then((response) => {
        setLoading(false);
        console.log("Certificate uploaded successfully:", response);
        showAlert('success', 'Certificate file uploaded successfully!');
      })
      .catch((error) => {
        setLoading(false);
        console.error("Error uploading certificate:", error);
        showAlert('danger', 'Error uploading certificate file.');
      });
      
  };

  return (
    <div className="container mt-5">
      {loading && <Spinner />}
      <h1 className="mb-4">HTTPS Certificates</h1>
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
