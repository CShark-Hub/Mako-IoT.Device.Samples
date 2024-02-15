import { render } from "preact";
import { LocationProvider, Router, Route } from "preact-iso";

import { Header } from "./components/Header.jsx";
import { Home } from "./pages/Home/index.jsx";
import Config from "./pages/Config/index.jsx";
import { NotFound } from "./pages/_404.jsx";
import Exit from "./pages/Exit/index.js";
import Certificates from "./pages/Certificates/index.js";
import "bootstrap/dist/css/bootstrap.min.css";
import { AlertProvider } from "./components/AlertContext.js";
import { LanguageProvider } from "./components/LanguageContext.js";
import { ConfigProvider } from "./components/ConfigContext.js";
import { TextsProvider } from "./components/TextContext.js";

export function App() {
  return (
    <ConfigProvider>
      <LanguageProvider>
      <TextsProvider>
        <AlertProvider>
          <LocationProvider>
            <Header />		
            <main>
              <Router>
                <Route path="/" component={Home} />
                <Route path="/configuration" component={Config} />
                <Route path="/certificates" component={Certificates} />
                <Route path="/exit" component={Exit} />
                <Route default component={NotFound} />
              </Router>
            </main>
          </LocationProvider>
        </AlertProvider>
        </TextsProvider>
      </LanguageProvider>
    </ConfigProvider>
  );
}

render(<App />, document.getElementById("app"));
