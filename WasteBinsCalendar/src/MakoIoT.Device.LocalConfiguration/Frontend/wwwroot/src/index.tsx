import { render } from "preact";
import { LocationProvider, Router, Route } from "preact-iso";

import { Header } from "./components/Header.jsx";
import { Home } from "./pages/Home/index.jsx";
import Config from "./pages/Config/index.jsx";
import { NotFound } from "./pages/_404.jsx";
import Exit from "./pages/Exit/index.js";
import "bootstrap/dist/css/bootstrap.min.css";
import { AlertProvider } from "./components/AlertContext.js";

export function App() {
  return (
    <AlertProvider>
      <LocationProvider>
        <Header />		
        <main>
          <Router>
            <Route path="/" component={Home} />
            <Route path="/config" component={Config} />
            <Route path="/exit" component={Exit} />
            <Route default component={NotFound} />
          </Router>
        </main>
      </LocationProvider>
    </AlertProvider>
  );
}

render(<App />, document.getElementById("app"));
