import {Events} from "./components/Events/Events";
import {EventAreas} from './components/EventAreas/EventAreas';
import {EventSeats} from './components/EventSeats/EventSeats';
import {Tickets} from './components/Tickets/Tickets';
import {BrowserRouter, Route, Routes, NavLink} from 'react-router-dom';
import jwt from 'jwt-decode';
import Cookies from 'js-cookie'

function getRoles(){
  const jwtToken = Cookies.get('JwtTokenCookie');
  const user = jwt(jwtToken);
  const userData = [];
  const roles = [];
  Object.keys(user).forEach(key => userData.push({name: key, value: user[key]}));
  if(Array.isArray(userData[1].value)){
    Object.keys(userData[1].value).forEach(key => roles.push(userData[1].value[key]));
  }
  return roles;
}

function App() {
  const roles = getRoles();
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
                    {roles.includes('admin') || roles.includes('event manager')?
                      <li className="nav-item">
                        <NavLink className="nav-link text-white" to="/eventseats">EventSeats</NavLink>
                      </li>
                     :null}
                    {roles.includes('admin') || roles.includes('event manager')?
                      <li className="nav-item">
                        <NavLink className="nav-link text-white" to="/eventareas">EventAreas</NavLink>
                      </li>
                     :null}
                    {roles.includes('admin') || roles.includes('event manager')?
                      <li className="nav-item">
                        <NavLink className="nav-link text-white" to="/tickets">Tickets</NavLink>
                      </li>
                     :null}
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
