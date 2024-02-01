let appconfig = null;

const loadConfig = async () => {
  if (!appconfig) {
    const response = await fetch("/appconfig.json");
    if (!response.ok) {
      throw new Error(`Failed to load configuration: ${response.statusText}`);
    }
    appconfig = await response.json();
  }
  return appconfig;
};

/**
 * Function to submit form data to the backend.
 */
export async function submitFormData(
  wifiSettings: any,
  calendarSettings: any,
  binNames: any
): Promise<any> {
  if (!appconfig) {
    await loadConfig();
  }
  const response = await fetch(`${appconfig.backendUrl}/config`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ wifiSettings, calendarSettings, binNames }),
  });

  if (!response.ok) {
    throw new Error(`HTTP error! Status: ${response.status}`);
  }

  //return response.json();
}

/**
 * Function to upload HTTPS certificate.
 */
export async function uploadCertificate(file: File): Promise<any> {
  const formData = new FormData();
  formData.append("httpsCertificate", file);

  if (!appconfig) {
    await loadConfig();
  }
  const response = await fetch(`${appconfig.backendUrl}/cert`, {
    method: "POST",
    body: formData,
  });

  if (!response.ok) {
    throw new Error(`HTTP error! Status: ${response.status}`);
  }

  //return response.json();
}

/**
 * Function to retrieve data from the backend.
 */
export async function fetchData(): Promise<any> {
  if (!appconfig) {
    await loadConfig();
  }
  const response = await fetch(`${appconfig.backendUrl}/config`, {
    method: "GET",
  });

  if (!response.ok) {
    throw new Error(`HTTP error! Status: ${response.status}`);
  }

  return response.json();
}
