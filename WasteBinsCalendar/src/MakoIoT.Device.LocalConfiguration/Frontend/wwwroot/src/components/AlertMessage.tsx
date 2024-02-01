import { h, FunctionComponent } from 'preact';

interface AlertMessageProps {
  type: 'success' | 'danger';
  message: string;
  onClose: () => void;
}

const AlertMessage: FunctionComponent<AlertMessageProps> = ({ type, message, onClose }) => {
  return (
    <div className={`alert alert-${type} m-2`} role="alert">
      {message}
      <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close" onClick={onClose}></button>
    </div>
  );
};

export default AlertMessage;
