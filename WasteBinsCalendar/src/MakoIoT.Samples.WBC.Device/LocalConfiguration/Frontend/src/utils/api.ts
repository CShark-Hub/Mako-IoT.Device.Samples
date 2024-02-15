
/**
 * Function to submit form data to the backend.
 */
export async function submitFormData(
  wifiSettings: any,
  calendarSettings: any,
  binNames: any,
  appconfig: any
): Promise<any> {

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
export async function uploadCertificate(
  file: File, 
  appconfig: any
): Promise<any> {
  const formData = new FormData();
  formData.append("httpsCertificate", file);

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
export async function fetchData(appconfig: any): Promise<any> {
  const response = await fetch(`${appconfig.backendUrl}/config`, {
    method: "GET",
  });

  if (!response.ok) {
    throw new Error(`HTTP error! Status: ${response.status}`);
  }

  return response.json();
}


export async function exit(appconfig: any): Promise<any> {
  const response = await fetch(`${appconfig.backendUrl}/exit`, {
    method: "GET",
  });

  if (!response.ok) {
    throw new Error(`HTTP error! Status: ${response.status}`);
  }

  //return response.json();
}

export async function fetchTexts(lang, appconfig: any) {
  // Fetch localized texts from the backend
  // This is a simplified example. Adjust the URL and handling as needed.
  const response = await fetch(`${appconfig.backendUrl}/texts?lang=${lang}`);
  if (!response.ok) {
      throw new Error('Failed to fetch texts');
  }
  return response.json();
}
