import './App.css';
import {Events} from "./components/Events/Events";
import {EventAreas} from './components/EventAreas/EventAreas';
import {EventSeats} from './components/EventSeats/EventSeats';
import {Tickets} from './components/Tickets/Tickets';
import {BrowserRouter, Route, Routes, NavLink} from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <div className="App">
        <header>
          <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-success border-bottom box-shadow mb-3">
            <div className="container">
              <button className="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                              aria-expanded="false" aria-label="Toggle navigation">
                <span className="navbar-toggler-icon"></span>
              </button>
              <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                <ul className="navbar-nav flex-grow-1">
                    <li className="nav-item">
                      <NavLink className="nav-link text-white" to="/">Events</NavLink>
                    </li>
                    <li className="nav-item">
                      <NavLink className="nav-link text-white" to="/eventseats">EventSeats</NavLink>
                    </li>
                    <li className="nav-item">
                      <NavLink className="nav-link text-white" to="/eventareas">EventAreas</NavLink>
                    </li>
                    <li className="nav-item">
                      <NavLink className="nav-link text-white" to="/tickets">Tickets</NavLink>
                    </li>
                  </ul>
              </div>
            </div>
          </nav>
        </header>
        <div className="container">
          <main role="main" className="pb-3">
            <Routes>
              <Route path='/' element={<Events/>} />
              <Route path="/eventseats" element={<EventSeats/>} />
              <Route path="/eventareas" element={<EventAreas/>} />
              <Route path="/tickets" element={<Tickets/>} />
            </Routes>
          </main>
        </div>
        <footer className="border-top footer text-muted">
          <div className="container">
              &copy; 2022 - Footer
          </div>
        </footer>
      </div>
    </BrowserRouter>
  );
}

export default App;
