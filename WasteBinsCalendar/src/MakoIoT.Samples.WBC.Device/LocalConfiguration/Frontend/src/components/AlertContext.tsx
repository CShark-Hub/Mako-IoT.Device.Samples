import { createContext } from 'preact';
import { useState, useContext } from 'preact/hooks';
import { createPortal } from 'preact/compat';
import AlertMessage from './AlertMessage';

const AlertContext = createContext({
  showAlert: (type: 'success' | 'danger', message: string) => {},
  hideAlert: () => {},
});

export const useAlert = () => useContext(AlertContext);

export const AlertProvider = ({ children }) => {
    const [alert, setAlert] = useState<{ type: 'success' | 'danger'; message: string } | null>(null);
  
    const showAlert = (type: 'success' | 'danger', message: string) => {
      setAlert({ type, message });
  
      // Scroll to the top of the page
      window.scrollTo({
        top: 0,
        behavior: 'smooth' // Optional: adds smooth scrolling
      });
    };
  
    const hideAlert = () => {
      setAlert(null);
    };
  
    return (
      <AlertContext.Provider value={{ showAlert, hideAlert }}>
        {children}
        {alert && createPortal(
          <AlertMessage type={alert.type} message={alert.message} onClose={hideAlert} />,
          document.getElementById('alert-container')
        )}
      </AlertContext.Provider>
    );
  };
